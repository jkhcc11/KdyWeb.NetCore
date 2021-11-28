using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity;
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
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Qq号
        /// </summary>
        public string UserQq { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public KdyUserStatus UserStatus { get; set; }

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
        /// 头部授权Key
        /// </summary>
        public string AuthKey { get; set; }
    }
}
