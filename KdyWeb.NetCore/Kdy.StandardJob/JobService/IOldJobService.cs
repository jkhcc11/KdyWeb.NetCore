using Kdy.StandardJob.JobInput;

namespace Kdy.StandardJob.JobService
{
    /// <summary>
    ///  旧项目通用job管理 接口
    /// </summary>
    /// <remarks>
    ///  1、所有旧项目需要执行的非即时后台任务
    /// </remarks>
    public interface IOldJobService : IKdyJobFlag
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <remarks>
        /// 图片分发上传
        /// </remarks>
        string UploadImgJob(UploadImgJobInput input);

        /// <summary>
        /// 邮件发送
        /// </summary>
        void SendEmailJob(SendEmailJobInput input);
    }
}
