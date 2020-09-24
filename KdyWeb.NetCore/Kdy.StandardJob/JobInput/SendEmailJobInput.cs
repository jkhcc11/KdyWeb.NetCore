namespace Kdy.StandardJob.JobInput
{
    /// <summary>
    /// 发送邮件Job 入参
    /// </summary>
    public class SendEmailJobInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="content">内容</param>
        public SendEmailJobInput(string email, string content)
        {
            Email = email;
            Content = content;
        }

        /// <summary>
        /// 接收者邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 具体内容
        /// </summary>
        public string Content { get; set; }
    }
}
