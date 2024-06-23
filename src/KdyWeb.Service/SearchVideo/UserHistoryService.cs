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
                .ThenInclude(a => a.VideoMain)
                .FirstOrDefaultAsync(a => a.Id == input.EpId);
            if (dbEpInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "影片信息错误");
            }

            var userId = LoginUserInfo.GetUserId();
            //历史表 存在时更新时间 要不然重复观看有问题
            var dbHistory = new UserHistory(dbEpInfo.VideoEpisodeGroup.MainId, input.EpId);
            var exitUserHistory = await _userHistoryRepository
                .FirstOrDefaultAsync(a => a.IsDelete == false &&
                               a.EpId == input.EpId &&
                               a.CreatedUserId == userId);
            if (exitUserHistory != null)
            {
                exitUserHistory.SetNewEpName(dbEpInfo.EpisodeName);
                _userHistoryRepository.Update(exitUserHistory);
                await UnitOfWork.SaveChangesAsync();
                return KdyResult.Success("更新成功");
            }

            CreateUserHistoryHandler(dbHistory, input);
            dbHistory.KeyId = dbEpInfo.VideoEpisodeGroup.MainId;
            dbHistory.VodName = dbEpInfo.VideoEpisodeGroup.VideoMain.KeyWord;
            dbHistory.EpName = dbEpInfo.EpisodeName;
            dbHistory.UserName = LoginUserInfo.UserName;

            await _userHistoryRepository.CreateAsync(dbHistory);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 用户播放记录分页查询
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryUserHistoryDto>>> QueryUserHistoryAsync(QueryUserHistoryInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            var result = await _userHistoryRepository.GetQuery()
                .Where(a => a.CreatedUserId == userId)
                .OrderByDescending(a => a.ModifyTime ?? a.CreatedTime)
                .GetDtoPageListAsync<UserHistory, QueryUserHistoryDto>(input);
            return KdyResult.Success(result);
        }

        internal void CreateUserHistoryHandler(UserHistory dbUserHistory, CreateUserHistoryInput input)
        {
            dbUserHistory.EpId = input.EpId;
            dbUserHistory.VodUrl = input.VodUrl;
        }
    }
}
