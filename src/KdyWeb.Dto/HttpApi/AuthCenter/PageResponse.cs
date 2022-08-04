namespace KdyWeb.Dto.HttpApi.AuthCenter
{
    public abstract class PageResponse
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
