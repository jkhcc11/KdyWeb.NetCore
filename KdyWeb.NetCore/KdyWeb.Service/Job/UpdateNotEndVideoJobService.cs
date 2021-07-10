using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Kdy.StandardJob.JobInput;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

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
        private readonly IKdyRedisCache _kdyRedisCache;

        public UpdateNotEndVideoJobService(IVideoEpisodeService videoEpisodeService, IPageSearchConfigService pageSearchConfigService, IKdyRedisCache kdyRedisCache)
        {
            _videoEpisodeService = videoEpisodeService;
            _pageSearchConfigService = pageSearchConfigService;
            _kdyRedisCache = kdyRedisCache;
        }

        public override async Task ExecuteAsync(UpdateNotEndVideoMainJobInput input)
        {
            var cacheKey = $"{KdyServiceCacheKey.NotEndKey}:{input.MainId}";
            var redisDb = _kdyRedisCache.GetDb(1);
            var cacheV = await redisDb.GetValueAsync<string>(cacheKey);
            if (string.IsNullOrEmpty(cacheV) == false)
            {
                //已有不用更新
                return;
            }

            //标识未完结处理
            await redisDb.SetValueAsync(cacheKey, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), TimeSpan.FromHours(2));

            var host = new Uri(input.SourceUrl).Host;

            //获取实例
            var pageService = await _pageSearchConfigService.GetPageParseInstanceAsync(new GetPageParseInstanceInput()
            {
                BaseHost = host
            });
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
                KdyLog.LogWarning("主键:{input.MainId} 抓取更新失败，{pageResult.Msg},Input:{input},Result:{result}", input.MainId, pageResult.Msg, input, pageResult);

                throw new Exception(pageResult.Msg);
            }

            if (pageResult.Data.PageMd5 == input.VideoContentFeature)
            {
                KdyLog.LogWarning("主键:{input.MainId} 抓取特征码相同，不用更新.{pageResult.Msg},Input:{input},Result:{result}", input.MainId, pageResult.Msg, input, pageResult);


                //KdyLog.Warn($"主键:{input.MainId} 抓取特征码相同，不用更新", new Dictionary<string, object>()
                //{
                //    {"JobInput",input},
                //    {"PageResult",pageResult}
                //}, input.MainId.ToString());
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

            KdyLog.LogTrace("主键:{input.MainId} 抓取更新完成.{pageResult.Msg},JobInput:{input},InputResult:{inputResult},PageResult:{pageResult}", input.MainId, pageResult.Msg, input, inputResult, pageResult);

            //KdyLog.Warn($"主键:{input.MainId} 抓取更新完成，{pageResult.Msg}", new Dictionary<string, object>()
            //{
            //    {"JobInput",input},
            //    {"InputResult",inputResult},
            //    {"PageResult",pageResult}
            //}, input.MainId.ToString());
            #endregion
        }

    }
}
