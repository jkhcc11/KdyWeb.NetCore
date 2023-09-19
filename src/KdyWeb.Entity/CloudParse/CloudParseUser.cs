using System;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 云盘用户
    /// </summary>
    public class CloudParseUser : BaseEntity<long>
    {
        /// <summary>
        /// 自有api地址长度
        /// </summary>
        public const int SelfApiUrlLength = 150;

        public const int ApiTokenLength = 50;

        public CloudParseUser(long userId)
        {
            UserId = userId;
            UserStatus = ServerCookieStatus.Init;
            IsHoldLink = false;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 自有Api地址
        /// </summary>
        [StringLength(SelfApiUrlLength)]
        public string? SelfApiUrl { get; set; }

        /// <summary>
        /// 防盗链
        /// </summary>
        public bool IsHoldLink { get; set; }

        /// <summary>
        /// 防盗链Host
        /// </summary>
        public string[]? HoldLinkHost { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public ServerCookieStatus UserStatus { get; set; }

        /// <summary>
        /// Api使用
        /// </summary>
        [StringLength(ApiTokenLength)]
        public string? ApiToken { get; protected set; }

        /// <summary>
        /// 初始化token
        /// </summary>
        public void InitToken()
        {
            ApiToken = $"parse-v2-{Guid.NewGuid():N}";
        }
    }
}
