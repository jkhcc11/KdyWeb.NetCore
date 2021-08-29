using System.Collections.Generic;
using AutoMapper;
using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 通用站点页面解析 Out
    /// </summary>
    public class NormalPageParseOut: IPageParseOut
    {
        /// <summary>
        /// 搜索结果集合
        /// </summary>
        public IList<NormalPageParseItem> Results { get; set; }

        /// <summary>
        /// 页面特征码
        /// </summary>
        public string PageMd5 { get; set; }

        /// <summary>
        /// 搜索结果
        /// </summary>
        public string ResultName { get; set; }

        /// <summary>
        /// 详情Url
        /// </summary>
        public string DetailUrl { get; set; }

        /// <summary>
        /// 是否完结
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 海报Url
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }
    }

    /// <summary>
    /// 搜索结果Item
    /// </summary>
    [AutoMap(typeof(KdyWebPagePageOut))]
    public class NormalPageParseItem
    {
        /// <summary>
        /// 结果Url
        /// </summary>
        public string ResultUrl { get; set; }

        /// <summary>
        /// 结果名称
        /// </summary>
        public string ResultName { get; set; }
    }
}
