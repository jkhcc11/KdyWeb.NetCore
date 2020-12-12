using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.IService;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service
{
    /// <summary>
    /// 视频播放 服务实现
    /// </summary>
    public class VideoPlayService : BaseKdyService, IVideoPlayService
    {
        private readonly IVideoEpisodeService _videoEpisodeService;
        private readonly IConfiguration _configuration;

        public VideoPlayService(IUnitOfWork unitOfWork, IVideoEpisodeService videoEpisodeService, IConfiguration configuration) :
            base(unitOfWork)
        {
            _videoEpisodeService = videoEpisodeService;
            _configuration = configuration;
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

            #region 网盘处理
            var epUrl = epInfo.Data.EpisodeUrl;
            var cloudDiskParseHost = _configuration.GetValue<string>(KdyWebServiceConst.CloudDiskParseHost);
            var desKey = _configuration.GetValue<string>(KdyWebServiceConst.DesKey);
            if (epUrl.Contains("/Cloud/Down"))
            {
                //云网盘解析
                if (epUrl.StartsWith("//"))
                {
                    epUrl = $"http:{epUrl}";
                }

                var host = new Uri(epUrl).Host;
                epInfo.Data.EpisodeUrl = epUrl.Replace(host, "")
                    .Replace("http://", "")
                    .Replace("https://", "");

                outModel.ExtensionParseHost = $"//{cloudDiskParseHost}";
                outModel.PlayUrl = epInfo.Data.EpisodeUrl.ToDesHexExt(desKey);
            }
            #endregion

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
