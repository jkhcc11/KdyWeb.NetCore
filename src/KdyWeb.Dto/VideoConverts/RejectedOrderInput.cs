using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 驳回订单
    /// </summary>
    public class RejectedOrderInput
    {
        /// <summary>
        /// 转换订单Id
        /// </summary>
        [Required(ErrorMessage = "缺少订单Id")]
        public long OrderId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required(ErrorMessage = "缺少备注")]
        public string Remark { get; set; }
    }
}
