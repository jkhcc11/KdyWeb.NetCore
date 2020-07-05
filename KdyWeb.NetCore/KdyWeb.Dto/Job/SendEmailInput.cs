namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 发送邮箱输入
    /// </summary>
    public class SendEmailInput
    {
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
