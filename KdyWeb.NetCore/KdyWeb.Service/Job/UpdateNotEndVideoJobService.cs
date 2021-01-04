using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Kdy.StandardJob.JobInput;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.SearchVideo;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 更新未完结视频Job
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    [AutomaticRetry(Attempts = 3)]
    public class UpdateNotEndVideoJobService : BaseKdyJob<UpdateNotEndVideoMainJobInput>
    {
        private readonly IVideoEpisodeService _videoEpisodeService;
        private readonly IPageSearchConfigService _pageSearchConfigService;
        public UpdateNotEndVideoJobService(IKdyLog kdyLog, IVideoEpisodeService videoEpisodeService, IPageSearchConfigService pageSearchConfigService) : base(kdyLog)
        {
            _videoEpisodeService = videoEpisodeService;
            _pageSearchConfigService = pageSearchConfigService;
        }

        public override void Execute(UpdateNotEndVideoMainJobInput input)
        {
            var host = new Uri(input.SourceUrl).Host;

            //获取实例
            var pageService = KdyAsyncHelper.Run(() => _pageSearchConfigService.GetPageParseInstanceAsync(new GetPageParseInstanceInput()
            {
                BaseHost = host
            }));
            if (pageService.IsSuccess == false)
            {
                return;
            }

            #region 获取最新结果
            var pageResult = KdyAsyncHelper.Run(() => pageService.Data.Instance.GetResultAsync(new NormalPageParseInput()
            {
                ConfigId = pageService.Data.ConfigId,
                Detail = input.SourceUrl
            }));
            if (pageResult.IsSuccess == false)
            {
                KdyLog.Warn($"主键:{input.MainId} 抓取更新失败，{pageResult.Msg}", new Dictionary<string, object>()
                {
                    {"JobInput",input},
                    {"PageResult",pageResult}
                });

                throw new Exception(pageResult.Msg);
            }

            if (pageResult.Data.PageMd5 == input.VideoContentFeature)
            {
                KdyLog.Warn($"主键:{input.MainId} 抓取特征码相同，不用更新", new Dictionary<string, object>()
                {
                    {"JobInput",input},
                    {"PageResult",pageResult}
                });
                return;
            }
            #endregion

            #region 更新或新增剧集

            var ep = pageResult.Data.Results.Select(a => new EditEpisodeItem()
            {
                EpisodeName = a.ResultName,
                EpisodeUrl = a.ResultUrl
            }).ToList();
            var updateInput =
                new UpdateNotEndVideoInput(input.MainId, pageResult.Data.PageMd5, pageResult.Data.IsEnd, ep);
            var inputResult = KdyAsyncHelper.Run(() => _videoEpisodeService.UpdateNotEndVideoAsync(updateInput));

            KdyLog.Warn($"主键:{input.MainId} 抓取更新完成，{pageResult.Msg}", new Dictionary<string, object>()
            {
                {"JobInput",input},
                {"InputResult",inputResult}
            });
            #endregion
        }

    }
}
