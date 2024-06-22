using System.Collections.Generic;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 全局资源
    /// </summary>
    public class GetAllResourceDto
    {
        /// <summary>
        /// 全局导航配置
        /// </summary>
        public List<NavItem> NavItems { get; set; } = new();

        /// <summary>
        /// Banner
        /// </summary>
        public List<BannerItem> BannerItems { get; set; } = new();

        /// <summary>
        /// 筛选项
        /// </summary>
        public List<QueryFilterItem> QueryFilterItems { get; set; } = new();

        /// <summary>
        /// 友链
        /// </summary>
        public List<LinkItem> LinkItems { get; set; } = new();

        /// <summary>
        /// 全局通知信息
        /// </summary>
        public string TipMsg { get; set; }
    }

    /// <summary>
    /// 友链
    /// </summary>
    public class LinkItem
    {
        /// <summary>
        /// 友链名
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string LinkUrl { get; set; }
    }

    /// <summary>
    /// Banner Item
    /// </summary>
    public class BannerItem
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string BannerName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 是否站外
        /// </summary>
        public bool IsOutstation { get; set; }
    }

    /// <summary>
    /// 导航Item
    /// </summary>
    public class NavItem
    {
        /// <summary>
        /// 显示名
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// 是否Url直接跳转
        /// </summary>
        public bool IsUrl { get; set; }

        /// <summary>
        /// 值|Url
        /// </summary>
        public string Value { get; set; }
    }

    public enum FilterTypeEnum
    {
        /// <summary>
        /// 分类
        /// </summary>
        Type = 1,

        /// <summary>
        /// 类型
        /// </summary>
        Genre = 2,

        /// <summary>
        /// 国家
        /// </summary>
        Country = 3,
    }

    /// <summary>
    /// 筛选项
    /// </summary>
    public class QueryFilterItem
    {
        /// <summary>
        /// 过滤类型
        /// </summary>
        public FilterTypeEnum FilterType { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// 过滤值
        /// </summary>
        public string FilterValue { get; set; }
    }
}
