using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Service.HttpCapture;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 自动匹配资源站资源
    /// </summary>
    /// <remarks>
    ///  1、仅根据关键字匹配资源站，如果匹配成功(关键字和年份一致)，则保存资源
    ///  2、不是人工匹配的且是之前的资源站，就重新匹配资源
    /// </remarks>
    [Queue(HangFireQueue.DouBan)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 20, 30, 40, 50 })]
    public class AutoMatchMovieInfoByZyJobService : BaseKdyJob<AutoMatchDouBanInfoJobInput>
    {
        private readonly IPageSearchConfigService _pageSearchConfigService;
        private readonly IVideoEpisodeService _videoEpisodeService;

        public AutoMatchMovieInfoByZyJobService(IPageSearchConfigService pageSearchConfigService,
            IVideoEpisodeService videoEpisodeService)
        {
            _pageSearchConfigService = pageSearchConfigService;
            _videoEpisodeService = videoEpisodeService;
        }


        /// <summary>
        /// 具体执行 异步
        /// </summary>
        public override async Task ExecuteAsync(AutoMatchDouBanInfoJobInput input)
        {
            var zyEngine = await _pageSearchConfigService.QueryShowPageConfigAsync(new SearchConfigInput()
            {
                KeyWord = nameof(ZyPageParseForJsonService)
            });
            if (zyEngine.IsSuccess == false ||
                zyEngine.Data.Any() == false)
            {
                return;
            }

            #region 搜索资源站并匹配
            var parseInput = new GetPageParseInstanceInput()
            {
                ConfigId = zyEngine.Data.First().Id
            };

            var pageResult = await _pageSearchConfigService.GetPageParseInstanceAsync(parseInput);
            if (pageResult.IsSuccess == false)
            {
                return;
            }

            var result = await pageResult.Data.Instance.GetResultAsync(new NormalPageParseInput()
            {
                ConfigId = zyEngine.Data.First().Id,
                KeyWord = input.VodTitle
            });

            if (result.IsSuccess == false)
            {
                KdyLog.LogWarning("影片关键字：{0}，KeyId:{1},匹配资源失败，资源站匹配资源失败。",
                    input.VodTitle,
                    input.MainId);
                return;
            }

            if (result.Data.ResultName.RemoveSpecialCharacters() != input.VodTitle.RemoveSpecialCharacters() &&
                result.Data.VideoYear != input.VodYear)
            {
                KdyLog.LogWarning("影片关键字：{0}，KeyId:{1},匹配资源失败，资源站匹配资源失败。",
                    input.VodTitle,
                    input.MainId);
                return;
            }
            #endregion

            //更新资源
            var updateNotEndInput = new UpdateNotEndVideoInput(input.MainId,
                result.Data.PageMd5,
                result.Data.IsEnd,
                result.Data.Results
                    .Select(a => new EditEpisodeItem()
                    {
                        EpisodeName = a.ResultName,
                        EpisodeUrl = a.ResultUrl
                    }).ToList())
            {
                SourceUrl = result.Data.DetailUrl
            };
            await _videoEpisodeService.UpdateNotEndVideoAsync(updateNotEndInput);
        }
    }
}
