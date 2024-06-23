using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            var epInfo = await _videoEpisodeService.GetEpisodeInfoAsync(epId, true);
            if (epInfo.IsSuccess == false)
            {
                result.Msg = epInfo.Msg;
                return result;
            }

            var outModel = new GetVideoInfoByEpIdDto(epId, epInfo.Data.EpisodeUrl.ToStrConfuse(7));

            #region 网盘处理

            var cloudParsePrefix = new[]
            {
                "/Cloud/Down", "/player-v2","/api-v2"
            };
            var epUrl = epInfo.Data.EpisodeUrl;
            var cloudDiskParseHost = _configuration.GetValue<string>(KdyWebServiceConst.CloudDiskParseHost);
            var cloudDiskParseHostNew = _configuration.GetValue<string>(KdyWebServiceConst.CloudDiskParseHostNew);
            var desKey = _configuration.GetValue<string>(KdyWebServiceConst.DesKey);
            if (epUrl.EndsWith(".m3u8") ||
                epUrl.EndsWith(".mp4"))
            {
                result = KdyResult.Success(outModel);
            }
            else if (cloudParsePrefix.Any(epUrl.Contains))
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

                if (cloudDiskParseHostNew.IsEmptyExt())
                {
                    outModel.ExtensionParseHost = $"//{cloudDiskParseHost}";
                }
                else
                {
                    outModel.ExtensionParseHost = $"//{cloudDiskParseHostNew}";
                    outModel.IsNewParse = true;
                }

                outModel.PlayUrl = epInfo.Data.EpisodeUrl.ToDesHexExt(desKey);
                result = KdyResult.Success(outModel);
            }
            else
            {
                result = await GetNoCloudParse(outModel, epUrl);
            }
            #endregion

            if (result.IsSuccess == false)
            {
                return KdyResult.Error<GetVideoInfoByEpIdDto>(result.Code, result.Msg);
            }

            var nowIndex = epInfo.Data.VideoEpisodeGroup.Episodes
                .FindIndex(a => a.Id == epId);
            var nextIndex = nowIndex + 1;
            if (epInfo.Data.VideoEpisodeGroup.Episodes.Count > nextIndex)
            {
                outModel.NextEpId = epInfo.Data.VideoEpisodeGroup.Episodes[nextIndex].Id;
            }

            return KdyResult.Success(outModel, "操作成功");
        }

        /// <summary>
        /// 非网盘解析
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult<GetVideoInfoByEpIdDto>> GetNoCloudParse(GetVideoInfoByEpIdDto outModel, string epUrl)
        {
            var reqInput = new KdyWebParseInput()
            {
                DetailUrl = epUrl
            };

            KdyResult<KdyWebParseOut> parseResult = null;
            if (epUrl.Contains("cctv.com"))
            {
                var cctvParse = KdyBaseServiceProvider.HttpContextServiceProvide.GetService<ICctvWebParseService>();
                if (cctvParse == null)
                {
                    return KdyResult.Error<GetVideoInfoByEpIdDto>(KdyResultCode.Error, "站点未配置");
                }

                parseResult = await cctvParse.GetResultAsync(reqInput);
            }
            else if (epUrl.StartsWith("detail,"))
            {
                var parse = KdyBaseServiceProvider.HttpContextServiceProvide.GetService<INormalWebParseService>();
                if (parse == null)
                {
                    return KdyResult.Error<GetVideoInfoByEpIdDto>(KdyResultCode.Error, "站点未配置");
                }

                //detail,http://www.yc2050.com/play/56368/1/31.html|hcc11.cn
                reqInput.DetailUrl = reqInput.DetailUrl.Replace("detail,", "")
                    .Replace("|hcc11.cn", "")
                    .Replace("|hcc11.com", "");

                parseResult = await parse.GetResultAsync(reqInput);
            }

            if (parseResult == null)
            {
                return KdyResult.Error<GetVideoInfoByEpIdDto>(KdyResultCode.Error, "未知站点，请联系管理员");
            }

            if (parseResult.IsSuccess)
            {
                outModel.PlayUrl = parseResult.Data.ResultUrl.ToStrConfuse();
                return KdyResult.Success(outModel);
            }

            return KdyResult.Error<GetVideoInfoByEpIdDto>(parseResult.Code, parseResult.Msg);
        }
    }
}
