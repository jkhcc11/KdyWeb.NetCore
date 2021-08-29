using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using HtmlAgilityPack;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 定时影片录入Job
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    [AutomaticRetry(Attempts = 3)]
    public class RecurringVideoJobService : BaseKdyJob<RecurringVideoJobInput>
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        private readonly IKdyRedisCache _kdyRedisCache;

        public RecurringVideoJobService(IKdyRequestClientCommon kdyRequestClientCommon, IKdyRedisCache kdyRedisCache)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
            _kdyRedisCache = kdyRedisCache;
        }

        public override async Task ExecuteAsync(RecurringVideoJobInput input)
        {
            var detailInput = new KdyRequestCommonInput(input.OriginUrl, HttpMethod.Get);
            var detailResult = await _kdyRequestClientCommon.SendAsync(detailInput);
            if (detailResult.IsSuccess == false)
            {
                throw new Exception($"获取详情失败，{detailResult.ErrMsg} 等待重试");
            }

            //开始解析搜索结果
            var hnc = detailResult.Data.GetNodeCollection(input.CaptureDetailXpath);
            if (hnc == null || hnc.Count <= 0)
            {
                KdyLog.LogWarning("影片定时录入失败，未解析到结果,{input.OriginUrl},Input:{input}", input.OriginUrl, input.ToJsonStr());
                return;
            }

            //firstUrl
            var firstUrl = DetailUrlHandler(hnc.First(), input);
            if (string.IsNullOrEmpty(firstUrl))
            {
                return;
            }

            var redisDb = _kdyRedisCache.GetDb();
            var cacheKey = $"capture,{input.OriginUrl}";
            var cacheV = await redisDb.GetValueAsync<string>(cacheKey);
            if (string.IsNullOrEmpty(cacheV) == false &&
                cacheV == firstUrl)
            {
                KdyLog.LogWarning("影片定时录入失败，未发现更新剧集,{input.OriginUrl},Input:{input}", input.OriginUrl, input.ToJsonStr());
                return;
            }

            await redisDb.SetValueAsync(cacheKey, firstUrl, TimeSpan.FromDays(1));

            foreach (var item in hnc)
            {
                var detailUrl = DetailUrlHandler(item, input);
                if (string.IsNullOrEmpty(detailUrl))
                {
                    continue;
                }

                var name = string.Empty;
                if (input.CaptureDetailNameSplit != null &&
                    input.CaptureDetailNameSplit.Any())
                {
                    name = item.SelectSingleNode("./text()")?.InnerText;
                    if (string.IsNullOrEmpty(name) == false)
                    {
                        //名称处理
                        foreach (var split in input.CaptureDetailNameSplit)
                        {
                            var tempArray = name.Split(split, StringSplitOptions.RemoveEmptyEntries);
                            if (tempArray.Length <= 1)
                            {
                                //没有匹配分割
                                continue;
                            }

                            name = tempArray.First();
                            break;
                        }
                    }
                }

                var captureInput = new VideoCaptureJobInput(detailUrl, name, input.ServiceFullName);

                //2秒延迟
                await Task.Delay(2000);
                BackgroundJob.Enqueue<VideoCaptureJobService>(a => a.ExecuteAsync(captureInput));

            }
        }

        /// <summary>
        /// 详情Url处理
        /// </summary>
        /// <param name="aNode">A标签</param>
        /// <param name="input">Input</param>
        /// <returns></returns>
        private string DetailUrlHandler(HtmlNode aNode, RecurringVideoJobInput input)
        {
            if (aNode == null)
            {
                return string.Empty;
            }

            if (aNode.Name == "id")
            {
                //cms xml特殊
                return $"{input.BaseHost}/detailId/{aNode.InnerText.Trim()}";
            }

            var detailUrl = aNode.GetAttributeValue("href", "");
            if (string.IsNullOrEmpty(detailUrl))
            {
                return detailUrl;
            }

            detailUrl = detailUrl.TrimStart('.');
            if (detailUrl.StartsWith("http") == false)
            {
                detailUrl = $"{input.BaseHost}{detailUrl}";
            }

            return detailUrl;
        }
    }
}
