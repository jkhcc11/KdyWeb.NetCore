using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Entity.VideoConverts
{
    /// <summary>
    /// 转换订单
    /// </summary>
    public class ConvertOrder : BaseEntity<long>
    {
        /// <summary>
        /// 转换订单
        /// </summary>
        /// <param name="checkoutAmount">结算金额</param>
        /// <param name="orderContent">订单内容</param>
        public ConvertOrder(decimal checkoutAmount, string orderContent)
        {
            CheckoutAmount = checkoutAmount;
            OrderContent = orderContent;
            ConvertOrderStatus = ConvertOrderStatus.Init;
        }

        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal CheckoutAmount { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal? ActualAmount { get; set; }

        /// <summary>
        /// 转换订单状态
        /// </summary>
        public ConvertOrderStatus ConvertOrderStatus { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public virtual ICollection<ConvertOrderDetail> OrderDetails { get; set; }

        /// <summary>
        /// 订单内容
        /// </summary>
        /// <remarks>
        /// 一般为链接
        /// </remarks>
        [StringLength(VideoConvertTask.SourceLinkLength)]
        public string OrderContent { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        [StringLength(VideoConvertTask.TaskRemarkLength)]
        public string OrderRemark { get; set; }

        /// <summary>
        /// 设置成功
        /// </summary>
        public void SetFinish(decimal actualAmount)
        {
            ConvertOrderStatus = ConvertOrderStatus.Finish;
            ActualAmount = actualAmount;
        }

        /// <summary>
        /// 设置作废
        /// </summary>
        public void SetInvalid(string remark)
        {
            ConvertOrderStatus = ConvertOrderStatus.Invalid;
            OrderRemark = remark;
        }
    }
}
