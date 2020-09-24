using Kdy.StandardJob;
using Kdy.StandardJob.JobInput;
using Kdy.StandardJob.JobService;
using KdyWeb.BaseInterface.KdyLog;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 图片上传 实现
    /// </summary>
    public class OldJobService : IOldJobService
    {
        private readonly IKdyLog _kdyLog;

        public OldJobService(IKdyLog kdyLog)
        {
            _kdyLog = kdyLog;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <remarks>
        /// 图片分发上传
        /// </remarks>
        public void UploadImgJob(UploadImgJobInput input)
        {
            _kdyLog.Info("正在上传图片", tags: nameof(UploadImgJob));
        }

        /// <summary>
        /// 邮件发送
        /// </summary>
        public void SendEmailJob(UploadImgJobInput input)
        {
            _kdyLog.Info("正在发送邮件", tags: nameof(SendEmailJob));
        }
    }
}
