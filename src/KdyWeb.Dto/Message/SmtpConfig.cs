namespace KdyWeb.Dto.Message
{
    /// <summary>
    /// 邮件配置
    /// </summary>
    public class SmtpConfig
    {
        /// <summary>
        /// 发送邮件用户名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public string Smtp { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 是否启用SSL发送
        /// </summary>
        public bool EnableSsl { get; set; }
    }
}
