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
        ///// <summary>
        ///// 豆瓣图片代理Url
        ///// </summary>
        //public const string DouBanProxyUrl = "DouBanProxyUrl";

        /// <summary>
        /// Minio配置文件节点
        /// </summary>
        public const string MinIoConfigKey = "MinioConfig";

        /// <summary>
        /// 微博配置文件节点
        /// </summary>
        public const string WeiBoConfigKey = "WeiBoConfig";

        /// <summary>
        /// 图床由于http异常上传失败时默认图床Id
        /// </summary>
        public const string UploadImgErrDefaultId = "UploadImgErrDefaultId";

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
        /// 内部http请求标识
        /// </summary>
        public const string DefaultUserAgent = "kdy/custom_client 1.0";

        /// <summary>
        /// 自有HostKey
        /// </summary>
        /// <remarks>
        /// 图床或防盗代理
        /// </remarks>
        public const string SelfHostKey = "SelfHostKey";

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

            /// <summary>
            /// 游戏检查
            /// </summary>
            public const string GameCheckConfig = "KdyGameCheck";
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

        /// <summary>
        /// 上传配置
        /// </summary>
        public class UploadConfig
        {
            /// <summary>
            /// 超星puid
            /// </summary>
            public const string UploadConfigCxPUid = "UploadConfig:cx_puid";

            /// <summary>
            /// 超星token
            /// </summary>
            public const string UploadConfigCxToken = "UploadConfig:cx_token";

            /// <summary>
            /// 腾讯文档globalPadId
            /// </summary>
            public const string UploadConfigTxDocId = "UploadConfig:tx_id";

            /// <summary>
            /// 腾讯文档cookie
            /// </summary>
            public const string UploadConfigTxDocCookie = "UploadConfig:tx_cookie";

            /// <summary>
            /// 微博cookie
            /// </summary>
            public const string WeiBoCookie = "UploadConfig:wb_cookie";

            /// <summary>
            /// 保持cookie微博api地址
            /// </summary>
            public const string WeiBoCookieApi = "UploadConfig:wb_api_host";

            /// <summary>
            /// 微博api地址_loginUrl
            /// </summary>
            public const string WeiBoCookieApiLoginUrl = "UploadConfig:wb_api_loginUrl";

            /// <summary>
            /// 微博api地址_cookieKey
            /// </summary>
            public const string WeiBoCookieApiCookieKey = "UploadConfig:wb_api_cookieKey";

            /// <summary>
            /// 微博api地址_successFlag
            /// </summary>
            public const string WeiBoCookieApiSuccessFlag = "UploadConfig:wb_api_successFlag";

            /// <summary>
            /// 图片上传大小限制（MB）
            /// </summary>
            public const string UploadImgMaxSize = "UploadConfig:uploadImg_max_size";
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

        /// <summary>
        /// 未完结更新缓存Key
        /// </summary>

        public const string NotEndKey = "NotEndKey";

        /// <summary>
        /// 游戏检查key
        /// </summary>
        public const string GameCheckKey = "GameCheckCookie";
    }

}
