using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 用户订阅 服务实现
    /// </summary>
    public class UserSubscribeService : BaseKdyService, IUserSubscribeService
    {
        private readonly IKdyRepository<UserSubscribe, long> _userSubscribeRepository;
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;

        public UserSubscribeService(IUnitOfWork unitOfWork, IKdyRepository<UserSubscribe, long> userSubscribeRepository,
            IKdyRepository<VideoMain, long> videoMainRepository) : base(unitOfWork)
        {
            _userSubscribeRepository = userSubscribeRepository;
            _videoMainRepository = videoMainRepository;
        }

        /// <summary>
        /// 用户收藏查询
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryUserSubscribeDto>>> QueryUserSubscribeAsync(QueryUserSubscribeInput input)
        {
            if (input.OrderBy == null ||
                input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(UserSubscribe.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var result = await _userSubscribeRepository.GetQuery()
                .GetDtoPageListAsync<UserSubscribe, QueryUserSubscribeDto>(input);
            if (result.Data.Any() == false)
            {
                return KdyResult.Success(result);
            }

            var businessIds = result.Data.Select(a => a.BusinessId).ToList();
            var videoMain = await _videoMainRepository.GetQuery()
                .Include(a => a.VideoMainInfo)
                .Where(a => businessIds.Contains(a.Id))
                .ProjectToExt<VideoMain, UserSubscribeBusinessItem>()
                .ToListAsync();

            foreach (var item in result.Data)
            {
                var videoItem = videoMain.FirstOrDefault(a => a.Id == item.BusinessId);
                if (videoItem == null)
                {
                    continue;
                }

                item.BusinessItems = videoItem;
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 创建用户收藏
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateUserSubscribeAsync(CreateUserSubscribeInput input)
        {
            var exit = await _userSubscribeRepository.GetAsNoTracking()
                .AnyAsync(a => a.CreatedUserId == input.UserId &&
                               a.BusinessId == input.BusinessId &&
                               a.UserSubscribeType == input.SubscribeType);
            if (exit)
            {
                return KdyResult.Error(KdyResultCode.Error, "已收藏，无需重复收藏");
            }

            long businessId = 0;
            var feature = string.Empty;
            if (input.SubscribeType == UserSubscribeType.Vod)
            {
                #region 影片处理
                var videoMain = await _videoMainRepository.FirstOrDefaultAsync(a => a.Id == input.BusinessId);
                if (videoMain == null)
                {
                    return KdyResult.Error(KdyResultCode.Error, "影片Id错误");
                }

                businessId = videoMain.Id;
                feature = videoMain.VideoContentFeature;
                #endregion
            }

            var dbSubscribe = new UserSubscribe(businessId, feature, input.SubscribeType)
            {
                CreatedUserId = input.UserId
            };
            await _userSubscribeRepository.CreateAsync(dbSubscribe);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 取消用户收藏
        /// </summary>
        /// <param name="subId">收藏Id</param>
        /// <returns></returns>
        public async Task<KdyResult> CancelUserSubscribeAsync(long subId)
        {
            var dbSubscribe = await _userSubscribeRepository
                .FirstOrDefaultAsync(a => a.Id == subId);
            if (dbSubscribe == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "参数错误");
            }

            _userSubscribeRepository.Delete(dbSubscribe);
            return KdyResult.Success();
        }
    }
}
