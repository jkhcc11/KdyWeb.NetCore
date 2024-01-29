using System.Collections.Generic;
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

        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        /// 有些下载需要固定带上附加ID，如 分组ID等，这种不支持跨云盘切换，所以手动指定
        /// </remarks>
        public string BusinessId { get; set; }

        /// <summary>
        /// 旧账号信息
        /// </summary>
        [StringLength(CloudParseUserChildren.OldSubAccountInfoLength)]
        public string OldSubAccountInfo { get; set; }

        /// <summary>
        /// 是否同步至服务器Cookie
        /// </summary>
        public bool IsSyncServerCookie { get; set; }

        /// <summary>
        /// 关联用户Id
        /// </summary>
        /// <remarks>
        /// 多个用户共用子账号信息
        /// </remarks>
        public List<string> RelationalUserArray { get; set; } = new();
    }
}
