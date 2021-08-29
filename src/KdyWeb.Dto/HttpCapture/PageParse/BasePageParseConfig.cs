using System.Net.Http;
using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{

    /// <summary>
    /// 站点搜索设置基类
    /// </summary>
    public class BaseSearchConfig : ISearchConfig
    {
        public HttpMethod Method { get; set; }

        public string SearchPath { get; set; }

        public string SearchData { get; set; }

        public string SearchXpath { get; set; }

        public string EndXpath { get; set; }

        public string NotEndKey { get; set; }

        /// <summary>
        /// 图片所在A标签属性name
        /// </summary>
        public string[] ImgAttr { get; set; }

        /// <summary>
        /// 名称所在A标签属性name
        /// </summary>
        public string[] NameAttr { get; set; }

    }

    /// <summary>
    /// 站点详情解析基类
    /// </summary>
    public class BasePageConfig : IPageConfig
    {
        public string ImgXpath { get; set; }

        public string DetailXpath { get; set; }

        public string EndXpath { get; set; }

        public string YearXpath { get; set; }

        /// <summary>
        /// 名称xpath
        /// </summary>
        public string NameXpath { get; set; }

        /// <summary>
        /// 播放地址后缀 
        /// </summary>
        /// <remarks>
        ///  适用于资源站(从前往后匹配)
        /// </remarks>
        public string[] PlayUrlSuffix { get; set; }
    }
}
