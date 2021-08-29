using System.Collections.Generic;

namespace KdyWeb.PageParse
{
    /// <summary>
    /// 获取搜索结果Out 接口
    /// </summary>
    public interface IKdyWebPageSearchOut<TItem>
    where TItem : IKdyWebPageSearchOutItem
    {
        /// <summary>
        /// 结果集合
        /// </summary>
        IList<TItem> Items { get; set; }
    }

    /// <summary>
    /// 获取搜索结果Item 
    /// </summary>
    public interface IKdyWebPageSearchOutItem
    {
        /// <summary>
        /// 是否完结
        /// </summary>
        public bool? IsEnd { get; set; }

        /// <summary>
        /// 搜索结果名称
        /// </summary>
        public string ResultName { get; set; }

        /// <summary>
        /// 详情页
        /// </summary>
        public string DetailUrl { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string ImgUrl { get; set; }
    }
}
