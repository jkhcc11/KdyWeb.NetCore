using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;

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

        public RecurringVideoJobService(IKdyLog kdyLog, IKdyRequestClientCommon kdyRequestClientCommon, IKdyRedisCache kdyRedisCache) : base(kdyLog)
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
                KdyLog.Warn($"影片定时录入失败，未解析到结果,{input.OriginUrl}", new Dictionary<string, object>()
                {
                    {"Input",input}
                });
                return;
            }

            var md5 = hnc.First().ParentNode.ParentNode.InnerHtml.Md5Ext();
            var redisDb = _kdyRedisCache.GetDb();
            var cacheKey = $"capture,{input.OriginUrl}";
            var cacheV = await redisDb.GetValueAsync<string>(cacheKey);
            if (string.IsNullOrEmpty(cacheV) == false &&
                cacheV == md5)
            {
                KdyLog.Warn($"影片定时录入失败，未发现更新剧集,{input.OriginUrl}", new Dictionary<string, object>()
                {
                    {"Input",input}
                });
                return;
            }

            await redisDb.SetValueAsync(cacheKey, md5, TimeSpan.FromDays(1));

            foreach (var item in hnc)
            {
                var detailUrl = item.GetAttributeValue("href", "");
                if (detailUrl.StartsWith("http") == false)
                {
                    detailUrl = $"{input.BaseHost}{detailUrl}";
                }

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

                var captureInput = new VideoCaptureJobInput()
                {
                    DetailUrl = detailUrl,
                    VideoName = name
                };

                BackgroundJob.Enqueue<VideoCaptureJobService>(a => a.ExecuteAsync(captureInput));
            }
        }
    }
}
