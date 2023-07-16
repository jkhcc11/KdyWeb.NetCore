using AutoMapper;
using AutoMapper.Configuration.Annotations;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 获取解析用户信息 Dto
    /// </summary>
    [AutoMap(typeof(CloudParseUser))]
    public class GetParseUserInfoDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 自有Api地址
        /// </summary>
        [SourceMember(nameof(CloudParseUser.SelfApiUrl))]
        public string CustomUrl { get; set; }

        /// <summary>
        /// 防盗链
        /// </summary>
        public bool IsHoldLink { get; set; }

        /// <summary>
        /// 防盗链Host
        /// </summary>
        public string[] HoldLinkHost { get; set; }

        /// <summary>
        /// 子账号数量
        /// </summary>
        public int SubAccountCount { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public ServerCookieStatus UserStatus { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long UserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string UserNick { get; set; }
    }
}
