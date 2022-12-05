using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 云盘用户
    /// </summary>
    public class CloudParseUser : BaseEntity<long>
    {
        /// <summary>
        /// 自有api地址长度
        /// </summary>
        public const int SelfApiUrlLength = 150;

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 自有Api地址
        /// </summary>
        [StringLength(SelfApiUrlLength)]
        public string SelfApiUrl { get; set; }

        /// <summary>
        /// 防盗链
        /// </summary>
        public bool IsHoldLink { get; set; }

        /// <summary>
        /// 防盗链Host
        /// </summary>
        public string[] HoldLinkHost { get; set; }
    }
}
