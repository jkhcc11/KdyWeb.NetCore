using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.Job;
using KdyWeb.Utility;

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
            KdyLog.Debug(nameof(SendEmailQueue), $"请求参数：{input.ToJsonStr()}", nameof(Execute));
        }

    }
}
