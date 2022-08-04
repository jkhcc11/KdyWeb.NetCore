using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 视频播放记录 服务实现 
    /// </summary>
    public class VideoHistoryService : BaseKdyService, IVideoHistoryService
    {
        private readonly IKdyRepository<VideoHistory, long> _videoHistoryRepository;
        private readonly IKdyRepository<VideoEpisode, long> _videoEpisodeRepository;

        public VideoHistoryService(IUnitOfWork unitOfWork, IKdyRepository<VideoHistory, long> videoHistoryRepository,
            IKdyRepository<VideoEpisode, long> videoEpisodeRepository) : base(unitOfWork)
        {
            _videoHistoryRepository = videoHistoryRepository;
            _videoEpisodeRepository = videoEpisodeRepository;
        }

        /// <summary>
        /// 创建视频播放记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateVideoHistoryAsync(CreateVideoHistoryInput input)
        {
            var dbEpInfo = await _videoEpisodeRepository.GetQuery()
                .Include(a => a.VideoEpisodeGroup)
                .ThenInclude(a => a.VideoMain)
                .FirstOrDefaultAsync(a => a.Id == input.EpId);
            if (dbEpInfo == null)
            {
                return KdyResult.Error(KdyResultCode.ParError, "错误EpId");
            }

            var mainInfo = dbEpInfo.VideoEpisodeGroup.VideoMain;

            var videoHistory = new VideoHistory(mainInfo.Id, dbEpInfo.Id);
            videoHistory.SetVideoInfo(dbEpInfo.EpisodeName, mainInfo.KeyWord, mainInfo.VideoImg);
            await _videoHistoryRepository.CreateAsync(videoHistory);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 查询视频播放记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryVideoHistoryDto>>> QueryVideoHistoryAsync(QueryVideoHistoryInput input)
        {
            if (input.OrderBy == null || input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(VideoMain.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var query = _videoHistoryRepository.GetAsNoTracking();
            if (input.IsShowBan == false)
            {
                query = query.Where(a => a.VideoMain.VideoDouBan > 0 &&
                                         a.VideoMain.IsMatchInfo);
            }

            if (input.Subtype != null)
            {
                query = query.Where(a => a.VideoMain.Subtype == input.Subtype.Value);
            }

            var result = await query
                .GetDtoPageListAsync<VideoHistory, QueryVideoHistoryDto>(input);

            foreach (var item in result.Data)
            {
                item.ImgHandler();
            }

            return KdyResult.Success(result);
        }
    }
}
