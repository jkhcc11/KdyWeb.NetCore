namespace KdyWeb.Dto.Message
{
    /// <summary>
    /// 发送邮箱输入
    /// </summary>
    public class SendEmailInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="email">接收者邮箱</param>
        /// <param name="subject">主题</param>
        /// <param name="content">具体内容</param>
        public SendEmailInput(string email, string subject, string content)
        {
            Email = email;
            Subject = subject;
            Content = content;
        }

        /// <summary>
        /// 接收者邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 具体内容
        /// </summary>
        public string Content { get; set; }
    }
}
