using AutoMapper;
using KdyWeb.Entity.HttpCapture;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 创建搜索配置 Input
    /// </summary>
    [AutoMap(typeof(PageSearchConfig), ReverseMap = true)]
    public class CreateSearchConfigInput : BaseSearchConfigInput
    {

    }

    /// <summary>
    /// 搜索配置基类
    /// </summary>
    public abstract class BaseSearchConfigInput
    {
        /// <summary>
        /// 服务实例名
        /// </summary>
        [Required(ErrorMessage = "服务实例名必选")]
        [StringLength(PageSearchConfig.ServiceFullNameLength)]
        public string ServiceFullName { get; set; }

        /// <summary>
        /// 站点名
        /// </summary>
        [Required(ErrorMessage = "站点名必选")]
        [StringLength(PageSearchConfig.HostNameLength)]
        public string HostName { get; set; }

        /// <summary>
        /// 搜索域名
        /// </summary>
        [StringLength(PageSearchConfig.BaseHostLength)]
        [Required(ErrorMessage = "搜索域名必选")]
        public string BaseHost { get; set; }

        /// <summary>
        /// 备用域名
        /// </summary>
        [StringLength(PageSearchConfig.OtherHostLength)]
        public string OtherHost { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        [StringLength(PageSearchConfig.UserAgentLength)]
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
        [StringLength(PageSearchConfig.SearchPathLength)]

        public string SearchPath { get; set; }

        /// <summary>
        /// Post数据
        /// </summary>
        [StringLength(PageSearchConfig.SearchDataLength)]
        public string SearchData { get; set; }

        /// <summary>
        /// 搜索结果匹配Xpath
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        [Required(ErrorMessage = "搜索结果匹配Xpath必选")]
        public string SearchXpath { get; set; }

        /// <summary>
        /// 搜索页 完结匹配Xpath
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        public string EndXpath { get; set; }

        /// <summary>
        /// 未完结 标识
        /// </summary>
        /// <remarks>
        /// 更新至、连载中、连载至等
        /// </remarks>
        [StringLength(PageSearchConfig.NotEndKeyLength)]
        public string NotEndKey { get; set; }

        /// <summary>
        /// 图片所在A标签属性name
        /// </summary>
        public string[] ImgAttr { get; set; }

        /// <summary>
        /// 名称所在A标签属性name
        /// </summary>
        public string[] NameAttr { get; set; }

        /// <summary>
        /// 详情匹配Xpath
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        [Required(ErrorMessage = "详情匹配Xpath必选")]
        public string DetailXpath { get; set; }

        /// <summary>
        /// 详情图片匹配Xpath
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        public string DetailImgXpath { get; set; }

        /// <summary>
        /// 详情名称匹配Xpath
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        public string DetailNameXpath { get; set; }

        /// <summary>
        /// 详情完结匹配Xpath
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        public string DetailEndXpath { get; set; }

        /// <summary>
        /// 站点备注
        /// </summary>
        [StringLength(PageSearchConfig.HostRemarkLength)]
        public string HostRemark { get; set; }

        /// <summary>
        /// 详情页年份Xpath
        /// </summary>
        public string YearXpath { get; set; }

        /// <summary>
        /// 播放地址后缀 
        /// </summary>
        /// <remarks>
        ///  适用于资源站(从前往后匹配)
        /// </remarks>
        public string[] PlayUrlSuffix { get; set; }

        /// <summary>
        /// 采集详情地址 
        /// </summary>
        public string[] CaptureDetailUrl { get; set; }

        /// <summary>
        /// 采集详情匹配Xpath
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        public string CaptureDetailXpath { get; set; }

        /// <summary>
        /// 采集详情名称处理
        /// </summary>
        /// <remarks>
        ///  xxxxBD高清->xxxx <br/>
        ///  xxxxHD高清->xxxx <br/>
        ///  xxxx更新至36集->xxxx <br/>
        /// </remarks>
        public string[] CaptureDetailNameSplit { get; set; }

        /// <summary>
        /// 配置状态
        /// </summary>
        public SearchConfigStatus SearchConfigStatus { get; set; }
    }
}
