using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 创建和更新Cookie类型
    /// </summary>
    public class CreateAndUpdateCookieTypeInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 显示文案
        /// </summary>
        [StringLength(CloudParseCookieType.ShowTextLength)]
        [Required]
        public string ShowText { get; set; }

        /// <summary>
        /// 业务标识
        /// </summary>
        [StringLength(CloudParseCookieType.BusinessFlagLength)]
        [Required]
        public string BusinessFlag { get; set; }
    }
}
