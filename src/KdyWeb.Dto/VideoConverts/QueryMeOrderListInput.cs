using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询我的转码订单列表input
    /// </summary>
    public class QueryMeOrderListInput : BasePageInput
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        [KdyQuery(nameof(ConvertOrder.ConvertOrderStatus), KdyOperator.Equal)]
        public ConvertOrderStatus? OrderStatus { get; set; }
    }
}
