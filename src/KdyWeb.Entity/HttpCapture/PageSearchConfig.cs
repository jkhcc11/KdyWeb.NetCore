using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.HttpCapture
{
    /// <summary>
    /// 站点页面搜索配置
    /// </summary>
    public class PageSearchConfig : BaseEntity<long>
    {
        #region const
        /// <summary>
        /// 服务实例名长度
        /// </summary>
        public const int ServiceFullNameLength = 150;

        /// <summary>
        /// 搜索域名长度
        /// </summary>
        public const int BaseHostLength = 50;
        /// <summary>
        /// 备用域名长度
        /// </summary>
        public const int OtherHostLength = 200;
        /// <summary>
        /// 用户代理长度
        /// </summary>
        public const int UserAgentLength = 300;
        /// <summary>
        /// 搜索路径长度
        /// </summary>
        public const int SearchPathLength = 100;
        /// <summary>
        /// Post数据长度
        /// </summary>
        public const int SearchDataLength = 2000;
        /// <summary>
        /// 通用Xpath长度
        /// </summary>
        public const int XpathLength = 100;
        /// <summary>
        /// 未完结 标识 长度
        /// </summary>
        public const int NotEndKeyLength = 50;

        /// <summary>
        /// 站点名 长度
        /// </summary>
        public const int HostNameLength = 50;
        /// <summary>
        /// 站点备注 长度
        /// </summary>
        public const int HostRemarkLength = 200;
        #endregion

        /// <summary>
        /// 站点页面搜索配置
        /// </summary>
        /// <param name="hostName">站点名</param>
        /// <param name="baseHost">搜索域名</param>
        /// <param name="configHttpMethod">搜索请求方法</param>
        /// <param name="serviceFullName">服务实例名</param>
        public PageSearchConfig(string hostName, string baseHost,
            ConfigHttpMethod configHttpMethod, string serviceFullName)
        {
            HostName = hostName;
            BaseHost = baseHost;
            ConfigHttpMethod = configHttpMethod;
            ServiceFullName = serviceFullName;
            SearchConfigStatus = SearchConfigStatus.Normal;
        }

        /// <summary>
        /// 站点名
        /// </summary>
        [Required]
        [StringLength(HostNameLength)]
        public string HostName { get; protected set; }

        /// <summary>
        /// 站点备注
        /// </summary>
        [StringLength(HostRemarkLength)]
        public string? HostRemark { get; set; }

        /// <summary>
        /// 搜索域名
        /// </summary>
        [StringLength(BaseHostLength)]
        [Required]
        public string BaseHost { get; protected set; }

        /// <summary>
        /// 备用域名
        /// </summary>
        [StringLength(OtherHostLength)]
        public string? OtherHost { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        [StringLength(UserAgentLength)]
        public string? UserAgent { get; set; }

        /// <summary>
        /// 搜索请求方法
        /// </summary>
        /// <remarks>
        /// Get、Post
        /// </remarks>
        public ConfigHttpMethod ConfigHttpMethod { get; protected set; }

        /// <summary>
        /// 搜索路径
        /// </summary>
        /// <remarks>
        /// 若Get直接拼接
        /// </remarks>
        [StringLength(SearchPathLength)]

        public string? SearchPath { get; set; }

        /// <summary>
        /// Post数据
        /// </summary>
        [StringLength(SearchDataLength)]
        public string? SearchData { get; set; }

        /// <summary>
        /// 服务实例名
        /// </summary>
        [Required]
        [StringLength(ServiceFullNameLength)]
        public string ServiceFullName { get; set; }

        #region 搜索结果页
        /// <summary>
        /// 搜索结果匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        [Required]
        public string? SearchXpath { get; set; }

        /// <summary>
        /// 搜索页 完结匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string? EndXpath { get; set; }

        /// <summary>
        /// 未完结 标识
        /// </summary>
        /// <remarks>
        /// 更新至、连载中、连载至等
        /// </remarks>
        [StringLength(NotEndKeyLength)]
        public string? NotEndKey { get; set; }

        /// <summary>
        /// 图片所在A标签属性name
        /// </summary>
        public string[]? ImgAttr { get; set; }

        /// <summary>
        /// 名称所在A标签属性name
        /// </summary>
        public string[]? NameAttr { get; set; }
        #endregion

        #region 详情页
        /// <summary>
        /// 详情匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        [Required]
        public string? DetailXpath { get; set; }

        /// <summary>
        /// 详情图片匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string? DetailImgXpath { get; set; }

        /// <summary>
        /// 详情名称匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string? DetailNameXpath { get; set; }

        /// <summary>
        /// 详情完结匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string? DetailEndXpath { get; set; }

        /// <summary>
        /// 播放地址后缀 
        /// </summary>
        /// <remarks>
        ///  适用于资源站(从前往后匹配)
        /// </remarks>
        public string[]? PlayUrlSuffix { get; set; }

        /// <summary>
        /// 详情页年份Xpath
        /// </summary>
        public string? YearXpath { get; set; }
        #endregion

        /// <summary>
        /// 采集详情地址 
        /// </summary>
        public string[]? CaptureDetailUrl { get; set; }

        /// <summary>
        /// 采集详情匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string? CaptureDetailXpath { get; set; }

        /// <summary>
        /// 采集详情名称处理
        /// </summary>
        /// <remarks>
        ///  xxxxBD高清->xxxx <br/>
        ///  xxxxHD高清->xxxx <br/>
        ///  xxxx更新至36集->xxxx <br/>
        /// </remarks>
        public string[]? CaptureDetailNameSplit { get; set; }

        /// <summary>
        /// 配置状态
        /// </summary>
        public SearchConfigStatus SearchConfigStatus { get; protected set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Ban()
        {
            SearchConfigStatus = SearchConfigStatus.Ban;
        }

        public void Open()
        {
            SearchConfigStatus = SearchConfigStatus.Normal;
        }

        public void SetConfigHttpMethod(ConfigHttpMethod method)
        {
            ConfigHttpMethod = method;
        }

        public void SetHostName(string hostName)
        {
            HostName = hostName;
        }

        public void SetBaseHost(string baseHost)
        {
            BaseHost = baseHost;
        }
    }
}
