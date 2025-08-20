using KdyWeb.IService.SequenceRecord;
using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SequenceRecord;
using KdyWeb.Entity.SequenceRecord;
using KdyWeb.IRepository.SequenceRecord;
using KdyWeb.BaseInterface;
using System.Collections.Generic;
using System.Linq;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.HttpCapture;
using KdyWeb.BaseInterface.KdyOptions;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Entity.SequenceRecord.Enum;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KdyWeb.Service.SequenceRecord
{
    /// <summary>
    /// 接龙使用记录 服务实现
    /// </summary>
    public class SequenceUseRecordService : BaseKdyService, ISequenceUseRecordService
    {
        private readonly ISequenceUseRecordRepository _sequenceUseRecordRepository;
        private readonly IKdyRepository<SequenceUserRecord, long> _sequenceUserRecordRepository;
        private readonly IKdyRepository<VenuesConfig, long> _venuesConfigRepository;
        private readonly IGiftUserConfigRepository _giftUserConfigRepository;
        private readonly ITxDocWebService _txDocWebService;
        private readonly TxDocRecordsOption _txDocRecordsOption;
        private readonly IKdyRedisCache _redisCache;

        public SequenceUseRecordService(IUnitOfWork unitOfWork, ISequenceUseRecordRepository sequenceUseRecordRepository,
            IKdyRepository<SequenceUserRecord, long> sequenceUserRecordRepository, ITxDocWebService txDocWebService,
            IOptions<TxDocRecordsOption> options, IKdyRedisCache redisCache, IKdyRepository<VenuesConfig, long> venuesConfigRepository,
            IGiftUserConfigRepository giftUserConfigRepository) : base(unitOfWork)
        {
            _sequenceUseRecordRepository = sequenceUseRecordRepository;
            _sequenceUserRecordRepository = sequenceUserRecordRepository;
            _txDocWebService = txDocWebService;
            _redisCache = redisCache;
            _venuesConfigRepository = venuesConfigRepository;
            _giftUserConfigRepository = giftUserConfigRepository;
            _txDocRecordsOption = options.Value;
        }

        /// <summary>
        /// 根据腾讯接龙记录创建接龙记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<IList<QueryPageSequenceUseRecordDto>>> CreateSequenceUseRecordByTxDocAsync(CreateSequenceUseRecordByTxDocInput input)
        {
            //1、获取文档接龙记录
            var txDocRecord = await GetSequenceUseRecordCacheByTxDocAsync(input);
            if (txDocRecord.IsSuccess == false)
            {
                return KdyResult.Error<IList<QueryPageSequenceUseRecordDto>>(txDocRecord.Code, txDocRecord.Msg);
            }

            //2、球馆信息
            var dbVenues = await _venuesConfigRepository.FirstOrDefaultAsync(a => a.ShortName == txDocRecord.Data.PlaceTxt);
            if (dbVenues == null)
            {
                return KdyResult.Error<IList<QueryPageSequenceUseRecordDto>>(KdyResultCode.Error, "无效球馆信息");
            }

            //3、根据接龙记录初始化用户初始化记录
            var dbUserRecord = txDocRecord.Data.SequenceRecordItems
            .Select(a => new SequenceUserRecord(a.UId, a.NickName, a.ShowName))
            .ToList();

            //接龙时间为准
            var totalDate = txDocRecord.Data.ActivityTime;

            //4、获取场馆奖励用户信息
            var dbGiftUserConfig = await _giftUserConfigRepository.GetValidGiftUserConfigByVenuesIdAsync(dbVenues.Id,
                totalDate);

            #region 5、计算奖励
            //基础价格
            var basePrice = dbVenues.MaxPrice;
            if (dbUserRecord.Count >= dbVenues.NumberSteps)
            {
                basePrice = dbVenues.MinPrice;
            }

            //vip用户使用记录
            var vipUserUseRecords = await _sequenceUseRecordRepository
                .GetAsNoTracking()
                .Where(a => a.GiftUserType == GiftUserTypeEnum.VipUser &&
                            a.UseDate != totalDate)
                .GroupBy(a => a.UserId)
                .Select(a => new
                {
                    UserId = a.Key,
                    UseCount = a.Count()
                })
                .ToDictionaryAsync(a => a.UserId, a => a.UseCount);

            var dbUseRecord = new List<SequenceUseRecord>();
            if (dbVenues.IsEnableGift)
            {
                //启用奖励计算
                dbUseRecord = TotalUserGift(totalDate, basePrice, dbVenues,
                    dbGiftUserConfig,
                    dbUserRecord);
            }
            else
            {
                #region 未启用奖励的普通用户
                var vipUserIds = dbGiftUserConfig
                        .Where(a => a.GiftUserType == GiftUserTypeEnum.VipUser)
                        .Select(a => a.UserId)
                        .ToList();
                foreach (var normalItem in dbUserRecord.Where(a => vipUserIds.Contains(a.UserId) == false))
                {
                    var currentUseRecord = new SequenceUseRecord(normalItem.UserId,
                        totalDate, basePrice)
                    {
                        UserShowName = normalItem.UserShowName,
                        UserNickName = normalItem.UserNickName,
                        VenuesId = dbVenues.Id,
                        VenuesShortName = dbVenues.ShortName
                    };
                    dbUseRecord.Add(currentUseRecord);
                }
                #endregion
            }

            //vip用户单独
            var vipUser = TotalVipUser(totalDate, basePrice, dbVenues,
                dbGiftUserConfig,
                dbUserRecord,
                vipUserUseRecords);
            if (vipUser.Any())
            {
                dbUseRecord.AddRange(vipUser);
            }

            #endregion

            #region 6、入库
            var isSaveChange = false;
            #region 不存在用户的记录就入库
            var currentUserRecords = dbUserRecord.Select(a => a.UserId).ToList();
            var alreadyUserRecords = await _sequenceUserRecordRepository
                .GetQuery()
                .Where(a => currentUserRecords.Contains(a.UserId))
                .Select(a => a.UserId)
                .ToListAsync();
            var newUserRecords = dbUserRecord
                .Where(a => alreadyUserRecords.Contains(a.UserId) == false)
                .ToList();
            if (newUserRecords.Any())
            {
                isSaveChange = true;
                //根据userId去重
                newUserRecords = newUserRecords.DistinctBy(a => a.UserId).ToList();
                //入库
                await _sequenceUserRecordRepository.CreateAsync(newUserRecords);
            }
            #endregion

            #region 接龙记录新增(特殊)
            if (dbUseRecord.Any())
            {
                isSaveChange = true;
                await _sequenceUseRecordRepository.BatchCreateAsync(dbUseRecord);
            }

            var changeConfig = dbGiftUserConfig.Where(a => a.IsChange).ToList();
            if (changeConfig.Any())
            {
                //只有更新过的
                _giftUserConfigRepository.Update(changeConfig);
            }

            #endregion

            if (isSaveChange)
            {
                await UnitOfWork.SaveChangesAsync();
            }
            #endregion

            var result = dbUseRecord.MapToListExt<QueryPageSequenceUseRecordDto>();
            return KdyResult.Success(result, "操作成功");
        }

        /// <summary>
        /// 分页获取接龙使用记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryPageSequenceUseRecordDto>>> QueryPageSequenceUseRecordAsync(QueryPageSequenceUseRecordInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new()
                {
                    Key = nameof(SequenceUseRecord.UseDate),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var query = _sequenceUseRecordRepository.GetQuery();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.UserShowName.Contains(input.KeyWord) ||
                                         a.UserNickName.Contains(input.KeyWord));
            }

            if (input.UseDate.HasValue)
            {
                //精确过滤
                query = query.Where(a => a.UseDate == input.UseDate);
            }
            else
            {
                if (input.StartTime.HasValue)
                {
                    query = query.Where(a => a.UseDate >= input.StartTime);
                }

                if (input.EndTime.HasValue)
                {
                    query = query.Where(a => a.UseDate <= input.EndTime);
                }
            }


            var pageList = await query
                .GetDtoPageListAsync<SequenceUseRecord, QueryPageSequenceUseRecordDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 分页获取用户接龙列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryPageSequenceUserRecordDto>>> QueryPageSequenceUserRecordAsync(QueryPageSequenceUserRecordInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new()
                {
                    Key = nameof(SequenceUserRecord.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var query = _sequenceUserRecordRepository.GetQuery();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.UserShowName.Contains(input.KeyWord) ||
                                         a.UserNickName.Contains(input.KeyWord));
            }

            var pageList = await query
                .GetDtoPageListAsync<SequenceUserRecord, QueryPageSequenceUserRecordDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 更新接龙使用记录
        /// </summary>
        /// <remarks>
        /// 人工干预调整奖励类型和价格
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> UpdateSequenceUseRecordAsync(UpdateSequenceUseRecordInput input)
        {
            var dbEntity = await _sequenceUseRecordRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效Id");
            }

            if (dbEntity.GiftUserType != input.GiftUserType)
            {
                if (dbEntity.GiftUserType.HasValue &&
                    dbEntity.GiftUserType.Value.IsTotalCount(dbEntity.CurrentPrice) &&
                    input.CurrentPrice > 0)
                {
                    //这里需要根据用户Id把奖励减去
                    await _giftUserConfigRepository.SubtractUseCountAsync(dbEntity.UserId,
                        dbEntity.GiftUserType.Value,
                        dbEntity.VenuesId,
                        dbEntity.UseDate);
                }
                else if (input.GiftUserType.HasValue &&
                          input.GiftUserType.Value.IsTotalCount(input.CurrentPrice))

                {
                    //这里需要根据用户Id把奖励使用加上
                    await _giftUserConfigRepository.AddGiftUseCountAsync(dbEntity.UserId,
                        input.GiftUserType.Value,
                        dbEntity.VenuesId,
                        dbEntity.UseDate);
                }
            }

            dbEntity.UpdateCurrentPrice(input.GiftUserType, input.CurrentPrice);
            _sequenceUseRecordRepository.Update(dbEntity);


            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取所有用户接龙记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<SelectedItemOut>>> GetAllUserRecordAsync()
        {
            var query = _sequenceUserRecordRepository.GetQuery();
            var result = await query
                .Select(a => new SelectedItemOut(a.UserShowName, a.UserId))
                .ToListAsync();
            return KdyResult.Success(result.OrderBy(a => a.Text).ToList());
        }

        #region 私有
        /// <summary>
        /// 获取接龙缓存
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult<GetSequenceRecordsOut>> GetSequenceUseRecordCacheByTxDocAsync(CreateSequenceUseRecordByTxDocInput input)
        {
            var cacheKey = $"{input.TxDocUrl.Md5Ext()}";
            if (input.IsForcedSync.HasValue &&
                input.IsForcedSync.Value)
            {
                await _redisCache.GetCache().RemoveAsync(cacheKey);
            }

            var cacheV = await _redisCache.GetCache().GetValueAsync<KdyResult<GetSequenceRecordsOut>>(cacheKey);
            if (cacheV != null)
            {
                return cacheV;
            }

            var txDocRecords = await _txDocWebService.GetSequenceRecordsAsync(new GetSequenceRecordsInput()
            {
                DocUrl = input.TxDocUrl,
                UserLoginCookie = _txDocRecordsOption.UserLoginCookie
            });
            //成功才缓存
            if (txDocRecords.IsSuccess)
            {
                await _redisCache.GetCache().SetValueAsync(cacheKey, txDocRecords,
                    TimeSpan.FromHours(1));
            }

            return txDocRecords;
        }

        /// <summary>
        /// 统计用户奖励计算（有比赛奖励计算，不涉及vip用户）
        /// </summary>
        /// <param name="totalDate">统计日期</param>
        /// <param name="basePrice">基础价格</param>
        /// <param name="venuesConfig">球馆配置</param>
        /// <param name="giftUserConfig">奖励用户配置</param>
        /// <param name="sequenceUserRecords">当前接龙记录</param>
        /// <returns></returns>
        private List<SequenceUseRecord> TotalUserGift(DateTime totalDate, decimal basePrice, VenuesConfig venuesConfig,
            List<GiftUserConfig> giftUserConfig, List<SequenceUserRecord> sequenceUserRecords)
        {
            //擂台、娱乐赛奖励、扣卡、阶梯价
            var result = new List<SequenceUseRecord>();
            foreach (var userRecord in sequenceUserRecords)
            {
                //当前用户的奖励配置
                var currentUserGiftConfig = giftUserConfig
                    .Where(a => a.UserId == userRecord.UserId)
                    .OrderByDescending(a => a.GiftUserType.GetHashCode())
                    .ToList();
                if (currentUserGiftConfig.Any(a => a.GiftUserType == GiftUserTypeEnum.VipUser))
                {
                    //vip单独计算
                    continue;
                }

                var currentUseRecord = new SequenceUseRecord(userRecord.UserId,
                    totalDate, basePrice)
                {
                    UserShowName = userRecord.UserShowName,
                    UserNickName = userRecord.UserNickName,
                    VenuesId = venuesConfig.Id,
                    VenuesShortName = venuesConfig.ShortName
                };

                if (currentUserGiftConfig.Any() == false ||
                    result.Any(a => a.UserId == userRecord.UserId))
                {
                    //不存在奖励用户或者用户已经在结果，直接跳过奖励只计算一次
                    result.Add(currentUseRecord);
                    continue;
                }

                //按照奖励类型顺序即可，多个奖励只有一个生效  不要是vip
                var firstUserGiftConfig = currentUserGiftConfig.FirstOrDefault();
                if (firstUserGiftConfig != null && firstUserGiftConfig.IsCan())
                {
                    //vip有多个奖励的，就以此来即可
                    currentUseRecord.UpdateCurrentPrice(firstUserGiftConfig.GiftUserType, firstUserGiftConfig.GiftPrice);
                    //需要统计次数的
                    if (firstUserGiftConfig.GiftUserType.IsTotalCount(firstUserGiftConfig.GiftPrice))
                    {
                        firstUserGiftConfig.AddGiftUseCount();
                    }
                }

                result.Add(currentUseRecord);
            }

            return result;
        }

        /// <summary>
        /// vip用户单独计算
        /// </summary>
        /// <param name="totalDate">统计日期</param>
        /// <param name="basePrice">基础价格</param>
        /// <param name="venuesConfig">球馆配置</param>
        /// <param name="giftUserConfig">所有奖励用户配置</param>
        /// <param name="sequenceUserRecords">当前接龙记录</param>
        /// <param name="vipUserUseRecords">vip用户的使用次数</param>
        /// <returns></returns>
        private List<SequenceUseRecord> TotalVipUser(DateTime totalDate, decimal basePrice, VenuesConfig venuesConfig,
            List<GiftUserConfig> giftUserConfig, List<SequenceUserRecord> sequenceUserRecords,
            Dictionary<string, int> vipUserUseRecords)
        {
            var result = new List<SequenceUseRecord>();
            //1、没有存在使用记录的，直接用优惠，存在使用记录的，次数低的用优惠
            //2、如果不能用vip优惠，那么这个人可以用比赛奖励等

            //是否启用vip计算 xx免1等
            //var isTotalVip = sequenceUserRecords.Count >= venuesConfig.VipFreeSteps;
            var vipNumber = sequenceUserRecords.Count / venuesConfig.VipFreeSteps;
            //if (isTotalVip == false)
            //{
            //    //不够直接跳过
            //    return result;
            //}

            //vip奖励用户Id
            var vipUserIds = giftUserConfig
                .Where(a => a.GiftUserType == GiftUserTypeEnum.VipUser)
                .Select(a => a.UserId)
                .ToArray();
            //当前接龙有资格的vip用户列表
            var currentVipUserRecords = sequenceUserRecords
                .Where(a => vipUserIds.Contains(a.UserId))
                .ToList();

            //构造当前接龙vip用户的使用记录
            var currentVipUseRecords = new List<(string userId, int recordCount)>();
            foreach (var currentVipItem in currentVipUserRecords)
            {
                if (vipUserUseRecords.TryGetValue(currentVipItem.UserId, out var record))
                {
                    //存在记录的
                    currentVipUseRecords.Add((currentVipItem.UserId, record));
                    continue;
                }

                currentVipUseRecords.Add((currentVipItem.UserId, 0));
            }

            //排序后然后计算
            foreach (var vipDic in currentVipUseRecords.OrderBy(a => a.recordCount))
            {
                var userRecord = currentVipUserRecords.First(a => a.UserId == vipDic.userId);
                if (result.Count < vipNumber &&
                    result.Any(a => a.UserId == vipDic.userId) == false)
                {
                    //一个vip用户只能算一次
                    #region 未超额，vip费用
                    result.Add(new SequenceUseRecord(userRecord.UserId, totalDate, 0)
                    {
                        UserShowName = userRecord.UserShowName,
                        UserNickName = userRecord.UserNickName,
                        VenuesId = venuesConfig.Id,
                        VenuesShortName = venuesConfig.ShortName,
                        GiftUserType = GiftUserTypeEnum.VipUser,
                        UseRecordRemark = $"已使用：{vipDic.recordCount}次"
                    });
                    #endregion
                }
                else
                {
                    #region 超额，计算普通费用，按照奖励类型顺序即可，多个奖励只有一个生效  不要是vip
                    var currentUseRecord = new SequenceUseRecord(userRecord.UserId, totalDate, basePrice)
                    {
                        UserShowName = userRecord.UserShowName,
                        UserNickName = userRecord.UserNickName,
                        VenuesId = venuesConfig.Id,
                        VenuesShortName = venuesConfig.ShortName
                    };

                    var firstUserGiftConfig = giftUserConfig
                        .Where(a => a.GiftUserType != GiftUserTypeEnum.VipUser &&
                                    a.UserId == vipDic.userId)
                        .OrderByDescending(a => a.GiftUserType.GetHashCode())
                        .FirstOrDefault();
                    if (venuesConfig.IsEnableGift &&
                        firstUserGiftConfig != null)
                    {
                        //启用了奖励计算且vip有多个奖励的，就以此来即可
                        currentUseRecord.UpdateCurrentPrice(firstUserGiftConfig.GiftUserType, firstUserGiftConfig.GiftPrice);
                    }

                    result.Add(currentUseRecord);
                    #endregion
                }
            }

            return result;
        }
        #endregion
    }
}
