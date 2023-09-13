using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.VideoConverts.Enum;
using KdyWeb.Utility;

namespace KdyWeb.Entity.VideoConverts
{
    /// <summary>
    /// 影片管理者记录
    /// </summary>
    public class VodManagerRecord : BaseEntity<long>
    {
        /// <summary>
        /// 影片管理者记录
        /// </summary>
        /// <param name="recordType">记录类型</param>
        /// <param name="checkoutAmount">结算金额</param>
        public VodManagerRecord(VodManagerRecordType recordType,
            decimal checkoutAmount)
        {
            RecordType = recordType;
            CheckoutAmount = checkoutAmount;
            ActualAmount = checkoutAmount;
            IsValid = true;
        }

        /// <summary>
        /// 结算金额
        /// </summary>
        [Required]
        public decimal CheckoutAmount { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal ActualAmount { get; set; }

        /// <summary>
        /// 记录类型
        /// </summary>
        [Required]
        public VodManagerRecordType RecordType { get; set; }

        /// <summary>
        /// 是否已结算
        /// </summary>
        public bool IsCheckout { get; set; }

        /// <summary>
        /// 是否为有效记录
        /// </summary>
        /// <remarks>
        /// 重复提交的就是false
        /// </remarks>
        public bool IsValid { get; set; }

        /// <summary>
        /// 业务Id 
        /// </summary>
        [Required]
        public long BusinessId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(VideoConvertTask.TaskRemarkLength)]
        public string? Remark { get; set; }

        /// <summary>
        /// 设置结算金额
        /// </summary>
        /// <param name="checkoutAmount">结算金额</param>
        public void SetCheckoutAmount(decimal checkoutAmount)
        {
            CheckoutAmount = checkoutAmount;
            ActualAmount = checkoutAmount;
        }
    }
}
