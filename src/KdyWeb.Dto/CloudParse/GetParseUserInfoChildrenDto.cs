using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 获取解析用户子账号列表
    /// </summary>
    [AutoMap(typeof(CloudParseUserChildren))]
    public class GetParseUserInfoChildrenDto : BaseEntityDto<int>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///  Cookie类型
        /// </summary>
        public CloudParseCookieType CookieType { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        public string CookieInfo { get; set; }
    }
}
