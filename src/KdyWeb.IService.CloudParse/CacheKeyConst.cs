﻿using KdyWeb.BaseInterface;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    public class CacheKeyConst
    {
        /// <summary>
        /// 文件名缓存
        /// </summary>
        public const string FileNameCache = "Common:FileName";

        /// <summary>
        /// 解析常量
        /// </summary>
        public class KdyCloudParseConst
        {
            /// <summary>
            /// 天翼企业云|家庭云加密参数
            /// </summary>
            public const string TyCorpDesKey = "parse202";
        }

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

            /// <summary>
            /// 下载缓存
            /// </summary>
            public const string DownCacheKey = Prefix + "DownUrl";
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
        /// 139盘CacheKey
        /// </summary>
        public class Pan139CacheKey
        {
            /// <summary>
            /// 前缀 
            /// </summary>
            private const string Prefix = "Pan139Cloud:";

            /// <summary>
            /// 下载缓存
            /// </summary>
            public const string DownCacheKey = Prefix + "M3u8Url";

            /// <summary>
            /// 下一页标记缓存
            /// </summary>
            public const string AliPageCacheKey = Prefix + "NextPageCacheKey";
        }

        /// <summary>
        /// TxShareCacheKey
        /// </summary>
        public class TxShareCacheKey
        {
            /// <summary>
            /// 前缀 
            /// </summary>
            private const string Prefix = "TxShareCache:";

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

            /// <summary>
            /// 下载缓存
            /// </summary>
            public const string DownCacheKey = Prefix + "DownUrl";

            public const string CropDownCacheKey = Prefix + "CropDownUrl";
            public const string FamilyDownCacheKey = Prefix + "FamilyDownUrl";
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

            /// <summary>
            /// 所有Cookie类型缓存Key
            /// </summary>
            public const string AllCookieTypeKey = "AllCookieTypeKey";

            /// <summary>
            /// 子账号缓存Key
            /// </summary>
            public const string SubAccountCookieKey = "SubAccountCookieKey";

            /// <summary>
            /// 服务器Cookie缓存Key
            /// </summary>
            public const string ServerCookieKey = "ServerCookieKey";

            /// <summary>
            /// 用户信息缓存Key
            /// </summary>
            public const string UserInfoCacheKey = "UserInfoCacheKey";
        }

        /// <summary>
        /// 115盘CacheKey
        /// </summary>
        public class Pan115CacheKey
        {
            /// <summary>
            /// 前缀 
            /// </summary>
            private const string Prefix = "Pan115Cloud:";

            /// <summary>
            /// 下载缓存
            /// </summary>
            public const string DownCacheKey = Prefix + "M3u8Url";
        }

        /// <summary>
        /// 旧网盘类型 转  业务标识
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KdyCustomException">没有匹配到</exception>
        public static string ToBusinessFlag(string oldCloudType)
        {
            //Index=>TyPerson
            //AliCloud=>Ali
            //NSt|St=>St
            switch (oldCloudType)
            {
                case "IndexV2":
                case "Index":
                    {
                        return CloudParseCookieType.TyPerson;
                    }
                case "IndexV4":
                    {
                        return CloudParseCookieType.TyFamily;
                    }
                case "AliCloud":
                    {
                        return CloudParseCookieType.Ali;
                    }
                case "St":
                case "NSt":
                    {
                        return CloudParseCookieType.BitQiu;
                    }
            }

            throw new KdyCustomException("oldCloudType未知");
        }
    }
}
