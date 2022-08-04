namespace KdyWeb.IService
{
    public class CacheKeyConst
    {
        /// <summary>
        /// 阿里云盘Cache
        /// </summary>
        public class AliYunCacheKey
        {
            /// <summary>
            /// 前缀
            /// </summary>
            private const string Prefix = "AliYun:";

            /// <summary>
            /// 文件名缓存
            /// </summary>
            public const string FileNameCache = Prefix + "FileName";

            /// <summary>
            /// 刷新Token
            /// </summary>
            public const string AliRefreshToken = Prefix + "RefreshToken";

            /// <summary>
            /// 请求Token
            /// </summary>
            public const string AliReqToken = Prefix + "ReqCloudToken";

            /// <summary>
            /// 下一页标记缓存
            /// </summary>
            public const string AliPageCacheKey = Prefix + "NextPageCacheKey";
        }

        /// <summary>
        /// 胜天CacheKey
        /// </summary>
        public class StCacheKey
        {
            /// <summary>
            /// 前缀 
            /// </summary>
            private const string Prefix = "StCloud:";

            /// <summary>
            /// 用户信息CacheKey
            /// </summary>
            public const string UserInfoCache = Prefix + "UserInfoCache";

            /// <summary>
            /// 下载缓存
            /// </summary>
            public const string DownCacheKey = Prefix + "M3u8Url";
        }

        /// <summary>
        /// 天翼CacheKey
        /// </summary>
        public class TyCacheKey
        {
            /// <summary>
            /// 前缀 
            /// </summary>
            private const string Prefix = "TyCloud:";

            /// <summary>
            /// 用户信息CacheKey
            /// </summary>
            public const string UserInfoCache = Prefix + "UserInfoCache";

            /// <summary>
            /// 用户家庭云信息CacheKey
            /// </summary>
            public const string UserFamilyInfoCache = Prefix + "UserFamilyInfoCache";

            /// <summary>
            /// 用户企业云信息CacheKey
            /// </summary>
            public const string UserCropInfoCache = Prefix + "UserCropInfoCache";

            /// <summary>
            /// 用户企业云文件信息CacheKey
            /// </summary>
            public const string UserCropFileInfoCache = Prefix + "UserCropFileInfoCache";
        }

        /// <summary>
        /// 自定义声明类型
        /// </summary>
        public class KdyCustomClaimType
        {
            /// <summary>
            /// 昵称
            /// </summary>
            public const string UserNickClaimType = "nickname";

            /// <summary>
            /// 更新时间
            /// </summary>
            public const string UpAtTme = "updated_at";

            /// <summary>
            /// 注册时间
            /// </summary>
            public const string RegTme = "reg_time";
        }

        /// <summary>
        /// 缓存名
        /// </summary>
        public class KdyCacheName
        {
            /// <summary>
            /// 验证码缓存
            /// </summary>
            public const string VerificationCodeCache = "EmailCodeCache";

            /// <summary>
            /// 用户注销缓存Key
            /// </summary>
            public const string UserLogoutKey = "UserLogoutKeyCache";
        }
    }
}
