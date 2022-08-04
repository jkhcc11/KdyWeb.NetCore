using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 保存解析用户子账号信息
    /// </summary>
    public class SaveParseUserInfoChildrenInput
    {
        /// <summary>
        ///  Cookie类型
        /// </summary>
        [EnumDataType(typeof(CloudParseCookieType), ErrorMessage = "类型错误")]
        public CloudParseCookieType CookieType { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        [StringLength(CloudParseUserChildren.CookieInfoLength)]
        public string CookieInfo { get; set; }
    }
}
