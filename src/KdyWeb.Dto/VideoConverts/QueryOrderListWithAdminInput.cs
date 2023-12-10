namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询转码订单列表 input
    /// </summary>
    public class QueryOrderListWithAdminInput : QueryMeOrderListInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
