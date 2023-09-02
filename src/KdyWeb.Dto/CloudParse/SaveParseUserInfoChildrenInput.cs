using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.CloudParse;

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
        [Required]
        [Range(1,long.MaxValue,ErrorMessage = "类型错误")]
        public long CookieTypeId { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        [StringLength(CloudParseUserChildren.CookieInfoLength)]
        public string CookieInfo { get; set; }
    }
}
