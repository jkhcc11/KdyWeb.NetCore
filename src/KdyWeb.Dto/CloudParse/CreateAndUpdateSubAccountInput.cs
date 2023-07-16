using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;

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
        /// 账号类型Id
        /// </summary>
        public long SubAccountTypeId { get; set; }
    }
}
