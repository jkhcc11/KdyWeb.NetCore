using AutoMapper;
using AutoMapper.Configuration.Annotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;

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
    }
}
