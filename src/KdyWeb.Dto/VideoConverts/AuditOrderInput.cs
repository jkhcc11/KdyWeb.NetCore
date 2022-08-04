using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 审批订单 input
    /// </summary>
    public class AuditOrderInput
    {
        /// <summary>
        /// 转换订单Id
        /// </summary>
        [Required(ErrorMessage = "缺少订单Id")]
        public long OrderId { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        [Required(ErrorMessage = "确实金额")]
        [Range(0.01, 999, ErrorMessage = "金额范围错误")]
        public decimal ActualAmount { get; set; }
    }
}
