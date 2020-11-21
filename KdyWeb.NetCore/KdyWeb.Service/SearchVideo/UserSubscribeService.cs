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
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.IService.SearchVideo
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
    }
}
