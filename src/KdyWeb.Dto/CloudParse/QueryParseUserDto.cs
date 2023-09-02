using System.ComponentModel.DataAnnotations;
using AutoMapper;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询用户列表
    /// </summary>
    [AutoMap(typeof(CloudParseUser))]
    public class QueryParseUserDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long UserId { get; set; }

        /// <summary>
        /// 自有Api地址
        /// </summary>
        public string SelfApiUrl { get; set; }

        /// <summary>
        /// 防盗链
        /// </summary>
        public bool IsHoldLink { get; set; }

        /// <summary>
        /// 防盗链Host
        /// </summary>
        public string[] HoldLinkHost { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public ServerCookieStatus UserStatus { get; set; }
    }
}
