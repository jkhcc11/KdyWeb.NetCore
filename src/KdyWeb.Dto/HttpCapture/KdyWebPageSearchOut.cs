using System.Collections.Generic;
using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 获取搜索结果Out
    /// </summary>
    public class KdyWebPageSearchOut : IKdyWebPageSearchOut<KdyWebPageSearchOutItem>
    {
        public IList<KdyWebPageSearchOutItem> Items { get; set; }
    }

    /// <summary>
    /// 获取搜索结果Item
    /// </summary>
    public class KdyWebPageSearchOutItem : IKdyWebPageSearchOutItem
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="resultName">搜索结果名</param>
        /// <param name="detailUrl">详情页</param>
        public KdyWebPageSearchOutItem(string resultName, string detailUrl)
        {
            ResultName = resultName;
            DetailUrl = detailUrl;
        }

        public KdyWebPageSearchOutItem()
        {
        }

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
