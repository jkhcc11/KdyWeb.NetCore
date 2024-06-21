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

        /// <summary>
        /// 允许前端筛选的类型
        /// </summary>
        public const string AllowGenre = "喜剧、爱情、动作、科幻、悬疑、犯罪、惊悚、冒险、奇幻、恐怖、战争";

        /// <summary>
        /// 类型Array
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllowGenreArray()
        {
            return AllowGenre.Split('、');
        }
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

        /// <summary>
        /// 游戏检查队列
        /// </summary>
        public const string GameCheck = "gamecheck";
    }
}
