using System.Net.Http;

namespace KdyWeb.PageParse
{
    /// <summary>
    /// 站点页面解析配置 接口
    /// </summary>
    public interface IKdyWebPageParseConfig<TSearchConfig, TPageConfig>
    where TSearchConfig : ISearchConfig
    where TPageConfig : IPageConfig
    {
        /// <summary>
        /// 域名
        /// </summary>
        string BaseHost { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// 搜索配置
        /// </summary>
        TSearchConfig SearchConfig { get; set; }

        /// <summary>
        /// 详情解析配置
        /// </summary>
        TPageConfig PageConfig { get; set; }
    }

    /// <summary>
    /// 搜索配置
    /// </summary>
    public interface ISearchConfig
    {
        /// <summary>
        /// 请求方式
        /// </summary>
        HttpMethod Method { get; set; }

        /// <summary>
        /// 搜索路径
        /// </summary>
        string SearchPath { get; set; }

        /// <summary>
        /// 搜索Post内容
        /// </summary>
        string SearchData { get; set; }

        /// <summary>
        /// 搜索结果匹配Xpath
        /// </summary>
        string SearchXpath { get; set; }

        /// <summary>
        /// 完结xpath
        /// </summary>
        string EndXpath { get; set; }

        /// <summary>
        /// 未完结标识
        /// </summary>
        string NotEndKey { get; set; }

    }

    /// <summary>
    /// 详情解析配置
    /// </summary>
    public interface IPageConfig
    {
        /// <summary>
        ///图片匹配Xpath
        /// </summary>
        string ImgXpath { get; set; }

        /// <summary>
        /// 详情匹配Xpath
        /// </summary>
        string DetailXpath { get; set; }

        /// <summary>
        /// 详情完结xpath
        /// </summary>
        string EndXpath { get; set; }

        /// <summary>
        /// 年份Xpath
        /// </summary>
        string YearXpath { get; set; }
    }
}
