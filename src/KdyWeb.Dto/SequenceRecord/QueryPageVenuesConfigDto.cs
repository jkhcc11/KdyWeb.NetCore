namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页查询场馆配置
    /// </summary>
    public class QueryPageVenuesConfigDto
    {
        /// <summary>
        /// 场馆名称
        /// </summary>
        public string VenuesName { get; set; }

        /// <summary>
        /// 是否启用奖励计算
        /// </summary>
        public bool IsEnableGift { get; set; }

        /// <summary>
        /// 缩写
        /// </summary>
        public string ShortName { get; set; }
    }
}
