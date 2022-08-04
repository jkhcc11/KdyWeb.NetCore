using System.Collections.Generic;

namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// 分页数据返回
    /// </summary>
    /// <typeparam name="TDto">Dto</typeparam>
    public class PageList<TDto>
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="pageSize">分页大小</param>
        public PageList(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数据大小
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        public ICollection<TDto> Data { get; set; }
    }
}
