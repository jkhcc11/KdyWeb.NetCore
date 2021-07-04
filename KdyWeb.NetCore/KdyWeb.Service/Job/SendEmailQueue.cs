using System;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Message;
using KdyWeb.IService.Message;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 发送邮件队列
    /// </summary>
    [Queue(HangFireQueue.Email)]
    public class SendEmailQueue : BaseKdyJob<SendEmailInput>
    {
        private readonly ISendEmailService _sendEmailService;
        public SendEmailQueue(ISendEmailService sendEmailService)
        {
            _sendEmailService = sendEmailService;
        }

        public override void Execute(SendEmailInput input)
        {
            var result = KdyAsyncHelper.Run(() => _sendEmailService.SendEmailAsync(input));
            KdyLog.LogDebug("发送邮件返回{result}", result.ToJsonStr());
            if (result.IsSuccess == false)
            {
                throw new Exception(result.Msg);
            }
        }

    }
}
