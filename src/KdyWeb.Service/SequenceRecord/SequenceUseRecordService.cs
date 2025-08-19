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

            var totalDate = DateTime.Now.Date;
            if (input.TotalDate.HasValue)
            {
                totalDate = input.TotalDate.Value.Date;
            }

            //4、获取场馆奖励用户信息
            var dbGiftUserConfig = await _giftUserConfigRepository.GetValidGiftUserConfigByVenuesIdAsync(dbVenues.Id,
                totalDate);

            #region 5、计算奖励
            List<SequenceUseRecord> dbUseRecord;
            if (dbVenues.IsEnableGift)
            {
                //启用奖励计算
                dbUseRecord = TotalUserGift(totalDate, dbVenues,
                    dbGiftUserConfig,
                    dbUserRecord);
            }
            else
            {
                //未启用奖励计算
                dbUseRecord = TotalUserNormal(totalDate, dbVenues,
                    dbGiftUserConfig.Where(a => a.GiftUserType == GiftUserTypeEnum.VipUser).ToList(),
                    dbUserRecord);
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
                //入库
                await _sequenceUserRecordRepository.CreateAsync(newUserRecords);
            }
            #endregion

            #region 不存在用户接龙记录就入库
            var currentUseRecords = dbUseRecord.Select(a => a.UserId).ToList();
            var alreadyUseRecords = await _sequenceUseRecordRepository
                .GetQuery()
                .Where(a => currentUseRecords.Contains(a.UserId) &&
                            a.UseDate == totalDate)
                .Select(a => a.UserId)
                .ToListAsync();
            var newUseRecords = dbUseRecord
                .Where(a => alreadyUseRecords.Contains(a.UserId) == false)
                .ToList();
            if (newUseRecords.Any())
            {
                isSaveChange = true;
                await _sequenceUseRecordRepository.BatchCreateAsync(newUseRecords);
            }

            #endregion

            if (isSaveChange)
            {
                await UnitOfWork.SaveChangesAsync();
            }
            #endregion

            var result = dbUseRecord.MapToListExt<QueryPageSequenceUseRecordDto>();
            return KdyResult.Success(result);
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
                    Key = nameof(SequenceUseRecord.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var query = _sequenceUseRecordRepository.GetQuery();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.UserShowName.Contains(input.KeyWord) ||
                                         a.UserNickName.Contains(input.KeyWord));
            }

            if (input.StartTime.HasValue)
            {
                query = query.Where(a => a.UseDate >= input.StartTime);
            }

            if (input.EndTime.HasValue)
            {
                query = query.Where(a => a.UseDate <= input.EndTime);
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

        #region 私有
        /// <summary>
        /// 获取接龙缓存
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult<GetSequenceRecordsOut>> GetSequenceUseRecordCacheByTxDocAsync(CreateSequenceUseRecordByTxDocInput input)
        {
            var cacheKey = $"{input.TxDocUrl.Md5Ext()}";
            if (input.IsForcedSync)
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
        /// 统计用户奖励计算（有比赛奖励计算）
        /// </summary>
        /// <returns></returns>
        private List<SequenceUseRecord> TotalUserGift(DateTime totalDate, VenuesConfig venuesConfig, List<GiftUserConfig> giftUserConfig,
            List<SequenceUserRecord> sequenceUserRecords)
        {
            //vip优先、擂台、vip免、娱乐赛奖励、vip卡、阶梯价

            //基础价格
            var basePrice = venuesConfig.MaxPrice;
            if (sequenceUserRecords.Count >= venuesConfig.NumberSteps)
            {
                basePrice = venuesConfig.MinPrice;
            }

            //是否启用vip计算 xx免1等
            var isTotalVip = sequenceUserRecords.Count >= venuesConfig.VipFreeSteps;
            var vipNumber = sequenceUserRecords.Count / venuesConfig.VipFreeSteps;

            var result = new List<SequenceUseRecord>();
            foreach (var userRecord in sequenceUserRecords)
            {
                //当前用户的奖励配置
                var currentUserGiftConfig = giftUserConfig
                    .Where(a => a.UserId == userRecord.UserId)
                    .OrderByDescending(a => a.GiftUserType.GetHashCode())
                    .ToList();
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

                //当前vip数量   可能存在多个vip数量 根据人数阶梯来
                var currentVipCount = result.Count(a => a.GiftUserType is GiftUserTypeEnum.VipUser);
                if (isTotalVip &&
                    currentVipCount < vipNumber &&
                    currentUserGiftConfig.Any(a => a.GiftUserType is GiftUserTypeEnum.VipUser))
                {
                    //vip用户
                    currentUseRecord.UpdateCurrentPrice(GiftUserTypeEnum.VipUser, 0);
                    result.Add(currentUseRecord);
                    continue;
                }

                //按照奖励类型顺序即可，多个奖励只有一个生效  不要是vip
                var firstUserGiftConfig = currentUserGiftConfig
                    .Where(a => a.GiftUserType != GiftUserTypeEnum.VipUser)
                    .OrderByDescending(a => a.GiftUserType.GetHashCode())
                    .FirstOrDefault();
                if (firstUserGiftConfig != null)
                {
                    //vip有多个奖励的，就以此来即可
                    currentUseRecord.UpdateCurrentPrice(firstUserGiftConfig.GiftUserType, firstUserGiftConfig.GiftPrice);
                }

                result.Add(currentUseRecord);
            }

            return result;
        }

        /// <summary>
        /// 统计用户普通计算（没有启用奖励仅普通）
        /// </summary>
        /// <returns></returns>
        private List<SequenceUseRecord> TotalUserNormal(DateTime totalDate, VenuesConfig venuesConfig,
            List<GiftUserConfig> vipGiftUserConfig,
            List<SequenceUserRecord> sequenceUserRecords)
        {
            //基础价格
            var basePrice = venuesConfig.MaxPrice;
            if (sequenceUserRecords.Count >= venuesConfig.NumberSteps)
            {
                basePrice = venuesConfig.MinPrice;
            }

            //是否启用vip计算 xx免1等
            var isTotalVip = sequenceUserRecords.Count >= venuesConfig.VipFreeSteps;
            var vipNumber = sequenceUserRecords.Count / venuesConfig.VipFreeSteps;
            var result = new List<SequenceUseRecord>();
            foreach (var userRecord in sequenceUserRecords)
            {
                var currentUseRecord = new SequenceUseRecord(userRecord.UserId,
                    totalDate, basePrice)
                {
                    UserShowName = userRecord.UserShowName,
                    UserNickName = userRecord.UserNickName,
                    VenuesId = venuesConfig.Id,
                    VenuesShortName = venuesConfig.ShortName
                };

                //当前vip数量   可能存在多个vip数量 根据人数阶梯来
                var currentVipCount = result.Count(a => a.GiftUserType is GiftUserTypeEnum.VipUser);
                if (isTotalVip &&
                    currentVipCount < vipNumber &&
                    vipGiftUserConfig.Any(a => a.GiftUserType is GiftUserTypeEnum.VipUser &&
                                               a.UserId == userRecord.UserId))
                {
                    //vip用户
                    currentUseRecord.UpdateCurrentPrice(GiftUserTypeEnum.VipUser, 0);
                    result.Add(currentUseRecord);
                    continue;
                }

                result.Add(currentUseRecord);
            }

            return result;
        }
        #endregion
    }
}
