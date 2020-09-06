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
    }
}
