using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.Job;
using KdyWeb.IService.HttpCapture;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 定时循环请求Url任务Job 
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    public class RecurringUrlJobService : BaseKdyJob<RecurrentUrlJobInput>
    {
        private readonly IRecurrentUrlConfigService _recurrentUrlConfigService;
        public RecurringUrlJobService(IKdyLog kdyLog, IRecurrentUrlConfigService recurrentUrlConfigService) : base(kdyLog)
        {
            _recurrentUrlConfigService = recurrentUrlConfigService;
        }

        public override async Task ExecuteAsync(RecurrentUrlJobInput input)
        {
            var result = await _recurrentUrlConfigService.RecurrentUrlAsync(input);
            KdyLog.Trace("定时循环请求Url完成", new Dictionary<string, object>()
            {
                {"Input",input},
                {"Result",result}
            });

            if (result.Code == KdyResultCode.SystemError)
            {
                throw new Exception(result.Msg);
            }
        }
    }
}
