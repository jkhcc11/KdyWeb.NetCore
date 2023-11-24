using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 保存解析用户信息
    /// </summary>
    public class SaveParseUserInfoInput
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        public string UserNick { get; set; }

        /// <summary>
        /// Api地址
        /// </summary>
        [Required]
        public string CustomUrl { get; set; }

        /// <summary>
        /// 防盗链
        /// </summary>
        [Required]
        public bool IsHoldLink { get; set; }

        /// <summary>
        /// 防盗链Host
        /// </summary>
        public string[] HoldLinkHost { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(CloudParseUser.RemarkLength)]
        public string Remark { get; set; }
    }
}
