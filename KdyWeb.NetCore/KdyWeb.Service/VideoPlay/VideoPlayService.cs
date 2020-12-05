using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.IService;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;

namespace KdyWeb.Service
{
    /// <summary>
    /// 视频播放 服务实现
    /// </summary>
    public class VideoPlayService : BaseKdyService, IVideoPlayService
    {
        private readonly IVideoEpisodeService _videoEpisodeService;

        public VideoPlayService(IUnitOfWork unitOfWork, IVideoEpisodeService videoEpisodeService) :
            base(unitOfWork)
        {
            _videoEpisodeService = videoEpisodeService;
        }

        /// <summary>
        /// 根据剧集Id获取视频信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetVideoInfoByEpIdDto>> GetVideoInfoByEpIdAsync(long epId)
        {
            var result = KdyResult.Error<GetVideoInfoByEpIdDto>(KdyResultCode.Error, "播放失败");
            var epInfo = await _videoEpisodeService.GetEpisodeInfoAsync(epId);
            if (epInfo.IsSuccess == false)
            {
                result.Msg = epInfo.Msg;
                return result;
            }

            var outModel = new GetVideoInfoByEpIdDto(epId, epInfo.Data.EpisodeUrl.ToStrConfuse());
            var nowIndex = epInfo.Data.VideoEpisodeGroup.Episodes
                .FindIndex(a => a.Id == epId);
            var nextIndex = nowIndex + 1;
            if (epInfo.Data.VideoEpisodeGroup.Episodes.Count > nextIndex)
            {
                outModel.NextEpId = epInfo.Data.VideoEpisodeGroup.Episodes[nextIndex].Id;
            }

            return KdyResult.Success(outModel, "操作成功");
        }
    }
}
