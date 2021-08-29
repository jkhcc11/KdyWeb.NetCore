using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Job;
using KdyWeb.IService.HttpCapture;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 定时循环请求Url任务Job 
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    public class RecurringUrlJobService : BaseKdyJob<RecurrentUrlJobInput>
    {
        private readonly IRecurrentUrlConfigService _recurrentUrlConfigService;
        public RecurringUrlJobService(IRecurrentUrlConfigService recurrentUrlConfigService)
        {
            _recurrentUrlConfigService = recurrentUrlConfigService;
        }

        public override async Task ExecuteAsync(RecurrentUrlJobInput input)
        {
            var result = await _recurrentUrlConfigService.RecurrentUrlAsync(input);
            KdyLog.LogTrace("定时循环请求Url完成.Input:{input},Result:{result}", input.ToJsonStr(), result.ToJsonStr());
            if (result.Code == KdyResultCode.SystemError)
            {
                throw new Exception(result.Msg);
            }
        }
    }
}
