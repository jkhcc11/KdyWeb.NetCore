using System.Collections.Generic;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.Job;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 发送邮件队列
    /// </summary>
    public class SendEmailQueue : BaseKdyJob<SendEmailInput>
    {
        public SendEmailQueue(IKdyLog kdyLog) : base(kdyLog)
        {

        }

        public override void Execute(SendEmailInput input)
        {
            KdyLog.Info("开始发送邮件", new Dictionary<string, object>()
            {
                {"ttt",input }
            });
        }

    }
}
