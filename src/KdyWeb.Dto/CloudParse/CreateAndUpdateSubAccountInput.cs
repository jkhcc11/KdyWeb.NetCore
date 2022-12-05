using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 创建和更新子账号信息
    /// </summary>
    public class CreateAndUpdateSubAccountInput
    {
        /// <summary>
        /// 子账号Id
        /// </summary>
        public long? SubAccountId { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        [StringLength(CloudParseUserChildren.AliasLength)]
        public string Alias { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        [StringLength(CloudParseUserChildren.CookieInfoLength)]
        [Required]
        public string Cookie { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        [EnumDataType(typeof(CloudParseCookieType), ErrorMessage = "类型错误")]
        public CloudParseCookieType SubAccountType { get; set; }
    }
}
