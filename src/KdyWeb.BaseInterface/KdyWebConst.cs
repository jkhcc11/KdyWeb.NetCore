namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 公用常量
    /// </summary>
    public class KdyBaseConst
    {
        /// <summary>
        /// 旧版Api头部验证
        /// </summary>
        public const string OldApiAuthKey = "OldKdyAuth";

        /// <summary>
        /// 登录Cookie Key
        /// </summary>
        public const string CookieKey = "Kdy666_Status";

        /// <summary>
        /// HttpClient Name
        /// </summary>
        public const string HttpClientName = "KdyWeb";
    }


    /// <summary>
    /// Hangfire队列
    /// </summary>
    public class HangFireQueue
    {
        /// <summary>
        /// 邮件队列
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// 豆瓣队列
        /// </summary>
        public const string DouBan = "douban";

        /// <summary>
        /// 资源获取队列
        /// </summary>
        public const string Capture = "capture";
    }
}
