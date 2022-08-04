namespace KdyWeb.CloudParse.Input
{
    /// <summary>
    /// 查询参数接口
    /// </summary>
    public class BaseQueryInput<TExtEntity>
    {
        /// <summary>
        /// 页
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 输入Id
        /// </summary>
        public string InputId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        public TExtEntity ExtData { get; set; }
    }
}
