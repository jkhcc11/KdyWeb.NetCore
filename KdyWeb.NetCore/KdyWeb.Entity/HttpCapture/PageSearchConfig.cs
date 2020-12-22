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
        public const int SearchDataLength = 100;
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
        /// 站点名
        /// </summary>
        [Required]
        [StringLength(HostNameLength)]
        public string HostName { get; set; }

        /// <summary>
        /// 站点备注
        /// </summary>
        [StringLength(HostRemarkLength)]
        public string HostRemark { get; set; }

        /// <summary>
        /// 搜索域名
        /// </summary>
        [StringLength(BaseHostLength)]
        [Required]
        public string BaseHost { get; set; }

        /// <summary>
        /// 备用域名
        /// </summary>
        [StringLength(OtherHostLength)]
        public string OtherHost { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        [StringLength(UserAgentLength)]
        public string UserAgent { get; set; }

        /// <summary>
        /// 搜索请求方法
        /// </summary>
        /// <remarks>
        /// Get、Post
        /// </remarks>
        public ConfigHttpMethod ConfigHttpMethod { get; set; }

        /// <summary>
        /// 搜索路径
        /// </summary>
        /// <remarks>
        /// 若Get直接拼接
        /// </remarks>
        [StringLength(SearchPathLength)]
       
        public string SearchPath { get; set; }

        /// <summary>
        /// Post数据
        /// </summary>
        [StringLength(SearchDataLength)]
        public string SearchData { get; set; }

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
        public string SearchXpath { get; set; }

        /// <summary>
        /// 搜索页 完结匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string EndXpath { get; set; }

        /// <summary>
        /// 未完结 标识
        /// </summary>
        /// <remarks>
        /// 更新至、连载中、连载至等
        /// </remarks>
        [StringLength(NotEndKeyLength)]
        public string NotEndKey { get; set; }

        /// <summary>
        /// 图片所在A标签属性name
        /// </summary>
        public string[] ImgAttr { get; set; }

        /// <summary>
        /// 名称所在A标签属性name
        /// </summary>
        public string[] NameAttr { get; set; }
        #endregion

        #region 详情页
        /// <summary>
        /// 详情匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        [Required]
        public string DetailXpath { get; set; }

        /// <summary>
        /// 详情图片匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string DetailImgXpath { get; set; }

        /// <summary>
        /// 详情名称匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string DetailNameXpath { get; set; }

        /// <summary>
        /// 详情完结匹配Xpath
        /// </summary>
        [StringLength(XpathLength)]
        public string DetailEndXpath { get; set; }
        #endregion
    }
}
