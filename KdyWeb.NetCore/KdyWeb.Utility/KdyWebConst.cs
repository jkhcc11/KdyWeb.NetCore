namespace KdyWeb.Utility
{
    /// <summary>
    /// 公用常量
    /// </summary>
    public class KdyWebConst
    {
        /// <summary>
        /// 密码盐
        /// </summary>
        public const string UserSalt = "20200412@kdyweb";

        /// <summary>
        /// Des Key
        /// </summary>
        public const string DesDecodeKey = "20201205";
    }

    /// <summary>
    /// 服务公用常量
    /// </summary>
    public class KdyWebServiceConst
    {
        /// <summary>
        /// Minio配置文件节点
        /// </summary>
        public const string MinIoConfigKey = "MinioConfig";

        /// <summary>
        /// 微博配置文件节点
        /// </summary>
        public const string WeiBoConfigKey = "WeiBoConfig";

        /// <summary>
        /// 自有图床配置Key
        /// </summary>
        public const string ImgHostKey = "SelfImgHost";

        /// <summary>
        /// 图床默认图片
        /// </summary>
        public const string DefaultImgUrl = "/kdyimg/public/old/8d78d765c1bce92.jpg";

        /// <summary>
        /// 邮件服务器配置
        /// </summary>
        public const string SmtpKey = "SmtpConfig";

        /// <summary>
        /// Des加密密钥
        /// </summary>
        public const string DesKey = "DesKey";

        /// <summary>
        /// 云网盘解析域名
        /// </summary>
        public const string CloudDiskParseHost = "CloudDiskParseHost";

        /// <summary>
        /// 解析配置节点
        /// </summary>
        public class KdyWebParseConfig
        {
            /// <summary>
            /// CCTV解析配置节点
            /// </summary>
            public const string CctvConfig = "KdyWebParse:CctvConfig";

            /// <summary>
            /// 通用解析配置节点
            /// </summary>
            public const string NormalConfig = "KdyWebParse:NormalConfig";
        }

        /// <summary>
        /// Job Cron表达式
        /// </summary>
        public class JobCron
        {
            /// <summary>
            /// 影片采集Cron
            /// </summary>
            public const string RecurringVideoJob = "JobCron:RecurringVideoJob";
        }
    }

    /// <summary>
    /// 缓存常量
    /// </summary>
    public class KdyServiceCacheKey
    {
        /// <summary>
        /// 微博缓存Key
        /// </summary>
        public const string WeiBoCookieKey = "WeiBoCookieKey";

        /// <summary>
        /// 弹幕缓存Key
        /// </summary>
        public const string DanMuKey = "DanMuKey";

        /// <summary>
        /// 视频缓存Key
        /// </summary>
        public const string VideoMainKey = "VideoMainKey";
    }

}
