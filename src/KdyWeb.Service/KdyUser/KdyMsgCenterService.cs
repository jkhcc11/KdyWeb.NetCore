using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyUser;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using KdyWeb.Entity.KdyUserNew;
using KdyWeb.Entity.KdyUserNew.Enum;
using KdyWeb.IRepository.KdyUserNew;
using KdyWeb.IService.KdyUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace KdyWeb.Service.KdyUser
{
    /// <summary>
    /// 消息中心 服务实现
    /// </summary>
    public class KdyMsgCenterService : BaseKdyService, IKdyMsgCenterService
    {
        private const string CheckUserExpiredCacheKey = "CheckUserExpiredCache";
        private readonly IKdyMsgCenterRepository _msgCenterRepository;
        private readonly IKdyRepository<CloudParseUser, long> _cloudParseUserRepository;

        public KdyMsgCenterService(IUnitOfWork unitOfWork, IKdyMsgCenterRepository msgCenterRepository,
            IKdyRepository<CloudParseUser, long> cloudParseUserRepository) : base(unitOfWork)
        {
            _msgCenterRepository = msgCenterRepository;
            _cloudParseUserRepository = cloudParseUserRepository;
        }

        /// <summary>
        /// 根据用户和类型获取未读Top消息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<IReadOnlyList<GetNoReadTopMsgDto>>> GetNoReadTopMsgAsync(GetNoReadTopMsgInput input)
        {
            var userId = LoginUserInfo.UserId;
            var dbResult = await _msgCenterRepository.GetNoReadTopMsgAsync(input.MsgType,
                userId ?? 0,
                10);
            var result = dbResult.MapToExt<IReadOnlyList<KdyMsgCenter>, IReadOnlyList<GetNoReadTopMsgDto>>();
            return KdyResult.Success(result, "操作成功");
        }

        /// <summary>
        ///已读消息
        /// </summary>
        /// <param name="msgId">消息Id</param>
        /// <returns></returns>
        public async Task<KdyResult> ReadMsgAsync(long msgId)
        {
            var entity = await _msgCenterRepository.FirstOrDefaultAsync(a => a.Id == msgId);
            if (entity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效消息Id");
            }

            entity.Read();
            _msgCenterRepository.Update(entity);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 检查用户当前过期状态
        /// </summary>
        /// <remarks>
        /// 解析用户正常且即将过期的提醒
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> CheckUserExpiredAsync()
        {
            var cacheV = await KdyRedisCache.GetCache().GetStringAsync(CheckUserExpiredCacheKey);
            if (string.IsNullOrEmpty(cacheV) == false)
            {
                return KdyResult.Success();
            }

            var checkStartTime = DateTime.Now.Date.AddHours(-1 * CloudParseUser.OverDays);
            var checkEndTime = DateTime.Now.AddDays(1).Date;
            var userIds = await _cloudParseUserRepository
                .GetQuery()
                .Where(a => a.UserStatus == ServerCookieStatus.Normal &&
                            a.ExpirationDateTime.HasValue &&
                            a.ExpirationDateTime >= checkStartTime &&
                            a.ExpirationDateTime <= checkEndTime)
                .Select(a => new
                {
                    a.UserId,
                    a.ExpirationDateTime
                })
                .ToArrayAsync();
            if (userIds.Any())
            {
                #region 检查并创建提醒消息
                var dbMsgEntities = userIds.Select(a => new KdyMsgCenter(MsgTypeEnum.User,
                             BusinessTypeEnum.ParseExpiredTip,
                             "解析到期提醒",
                             $"解析将于{a.ExpirationDateTime.Value.AddDays(CloudParseUser.OverDays):yyyy-MM-dd}失效,记得提前续费")
                {
                    UserId = a.UserId
                })
                         .ToList();

                //未读不用重复创建
                var noReadMsg = await _msgCenterRepository.GetListAsync(a => a.IsRead == false &&
                                                                             a.BusinessType == BusinessTypeEnum.ParseExpiredTip);
                var noReadUserIds = noReadMsg.Select(a => a.UserId).ToArray();
                var canCreate = dbMsgEntities
                    .Where(a => noReadUserIds.Contains(a.UserId) == false)
                    .ToList();
                if (canCreate.Any())
                {
                    await _msgCenterRepository.CreateAsync(dbMsgEntities);
                    await UnitOfWork.SaveChangesAsync();
                }
                #endregion
            }

            //12小时检测一次
            await KdyRedisCache.GetCache().SetStringAsync(CheckUserExpiredCacheKey,
                DateTime.Now.ToLongTimeString(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
                });
            return KdyResult.Success();
        }
    }
}
