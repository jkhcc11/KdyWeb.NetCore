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
    /// 用户播放记录 服务实现 
    /// </summary>
    public class UserHistoryService : BaseKdyService, IUserHistoryService
    {
        private readonly IKdyRepository<UserHistory, long> _userHistoryRepository;
        private readonly IKdyRepository<VideoEpisode, long> _videoEpisodeRepository;

        public UserHistoryService(IKdyRepository<UserHistory, long> userHistoryRepository, IUnitOfWork unitOfWork,
            IKdyRepository<VideoEpisode, long> videoEpisodeRepository) : base(unitOfWork)
        {
            _userHistoryRepository = userHistoryRepository;
            _videoEpisodeRepository = videoEpisodeRepository;
        }

        /// <summary>
        /// 创建用户播放记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateUserHistoryAsync(CreateUserHistoryInput input)
        {
            //剧集信息
            var dbEpInfo = await _videoEpisodeRepository.GetQuery()
                .Include(a => a.VideoEpisodeGroup)
                .FirstOrDefaultAsync(a => a.Id == input.EpId);
            if (dbEpInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "影片信息错误");
            }

            //历史表
            var dbInput = new UserHistory(dbEpInfo.VideoEpisodeGroup.MainId, input.EpId);
            var exit = await _userHistoryRepository.GetAsNoTracking()
                .AnyAsync(a => a.IsDelete == false &&
                               a.EpId == input.EpId &&
                               a.CreatedUserId == input.UserId);
            if (exit)
            {
                return KdyResult.Error(KdyResultCode.Error, "已存在");
            }

            CreateUserHistoryHandler(dbInput, input);
            await _userHistoryRepository.CreateAsync(dbInput);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 用户播放记录分页查询
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryUserHistoryDto>>> QueryUserHistoryAsync(QueryUserHistoryInput input)
        {
            if (input.OrderBy == null ||
                input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(UserHistory.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var result = await _userHistoryRepository.GetQuery()
                .GetDtoPageListAsync<UserHistory, QueryUserHistoryDto>(input);
            return KdyResult.Success(result);
        }

        internal void CreateUserHistoryHandler(UserHistory dbUserHistory, CreateUserHistoryInput input)
        {
            dbUserHistory.EpId = input.EpId;
            dbUserHistory.VodUrl = input.VodUrl;
            dbUserHistory.KeyId = 1;
            dbUserHistory.VodName = "测试名称";
            dbUserHistory.EpName = "极速";
            dbUserHistory.UserName = LoginUserInfo.UserName;
        }
    }
}
