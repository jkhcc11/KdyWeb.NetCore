using System;
using System.Net.Http;
using Hangfire;
using Kdy.StandardJob.JobInput;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 定时循环请求Url任务Job 
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    public class RecurringUrlJobService : BaseKdyJob<RecurringUrlJobInput>
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        public RecurringUrlJobService(IKdyLog kdyLog, IKdyRequestClientCommon kdyRequestClientCommon) : base(kdyLog)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        public override void Execute(RecurringUrlJobInput input)
        {
            var reqInput = new KdyRequestCommonInput(input.RequestUrl, HttpMethod.Post)
            {
                ExtData = new KdyRequestCommonExtInput()
                {
                    PostData = "kdytask=hcc11.cn"
                }
            };

            var reqResult = KdyAsyncHelper.Run(() => _kdyRequestClientCommon.SendAsync(reqInput));
            KdyLog.Debug($"循环Url返回：{reqResult.ToJsonStr()}");
            if (reqResult.IsSuccess == false)
            {
                throw new Exception(reqResult.ErrMsg);
            }
        }
    }
}
