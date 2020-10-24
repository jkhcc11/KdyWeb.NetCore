namespace Kdy.StandardJob.JobInput
{
    /// <summary>
    /// 用户反馈Job 入参
    /// </summary>
    public class UserFeedBackJobInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="url">源Url</param>
        /// <param name="videoName">视频信息</param>
        /// <param name="userEmail">用户Email</param>
        public UserFeedBackJobInput(string url, string videoName, string userEmail)
        {
            Url = url;
            VideoName = videoName;
            UserEmail = userEmail;
        }

        /// <summary>
        /// 反馈Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 视频信息
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 用户Email
        /// </summary>
        public string UserEmail { get; set; }
    }
}
