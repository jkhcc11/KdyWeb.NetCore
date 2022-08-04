using System.Collections.Generic;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询我的转码订单列表 dto
    /// </summary>
    [AutoMap(typeof(ConvertOrder))]
    public class QueryMeOrderListDto : CreatedUserDto<long>
    {
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
        public List<QueryMeOrderListDtoItem> OrderDetails { get; set; }

        /// <summary>
        /// 订单内容
        /// </summary>
        /// <remarks>
        /// 一般为链接
        /// </remarks>
        public string OrderContent { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string OrderRemark { get; set; }
    }

    /// <summary>
    /// 订单详情
    /// </summary>
    [AutoMap(typeof(ConvertOrderDetail))]
    public class QueryMeOrderListDtoItem
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }
    }
}
