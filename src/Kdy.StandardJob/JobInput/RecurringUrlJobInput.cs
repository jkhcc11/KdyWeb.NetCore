namespace Kdy.StandardJob.JobInput
{
    /// <summary>
    /// 添加循环请求Url Input
    /// </summary>
    public class RecurringUrlJobInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="jobId">JobId</param>
        /// <param name="requestUrl">任务Url</param>
        /// <param name="cron">定时Cron</param>
        public RecurringUrlJobInput(string jobId, string requestUrl, string cron)
        {
            JobId = jobId;
            RequestUrl = requestUrl;
            Cron = cron;
        }

        /// <summary>
        /// 延迟JobId 为空则创建 不为空则更新原Job
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 定时Cron
        /// </summary>
        public string Cron { get; set; }
    }
}
