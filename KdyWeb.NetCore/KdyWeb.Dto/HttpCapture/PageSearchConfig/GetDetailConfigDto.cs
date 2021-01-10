using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 获取配置详情 Dto
    /// </summary>
    [AutoMap(typeof(PageSearchConfig))]
    public class GetDetailConfigDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 服务实例名
        /// </summary>
        public string ServiceFullName { get; set; }

        /// <summary>
        /// 站点名
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 搜索域名
        /// </summary>
        public string BaseHost { get; set; }

        /// <summary>
        /// 备用域名
        /// </summary>
        public string OtherHost { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 搜索请求方法
        /// </summary>
        /// <remarks>
        /// Get、Post
        /// </remarks>
        public ConfigHttpMethod ConfigHttpMethod { get; set; }

        /// <summary>
        /// 搜索请求方法Str
        /// </summary>
        public string ConfigHttpMethodStr => ConfigHttpMethod.GetDisplayName();

        /// <summary>
        /// 搜索路径
        /// </summary>
        /// <remarks>
        /// 若Get直接拼接
        /// </remarks>

        public string SearchPath { get; set; }

        /// <summary>
        /// Post数据
        /// </summary>
        public string SearchData { get; set; }

        /// <summary>
        /// 搜索结果匹配Xpath
        /// </summary>
        public string SearchXpath { get; set; }

        /// <summary>
        /// 搜索页 完结匹配Xpath
        /// </summary>
        public string EndXpath { get; set; }

        /// <summary>
        /// 未完结 标识
        /// </summary>
        /// <remarks>
        /// 更新至、连载中、连载至等
        /// </remarks>
        public string NotEndKey { get; set; }

        /// <summary>
        /// 图片所在A标签属性name
        /// </summary>
        public string[] ImgAttr { get; set; }

        /// <summary>
        /// 图片所在A标签属性name Str
        /// </summary>
        public string ImgAttrStr => string.Join(",", ImgAttr);

        /// <summary>
        /// 名称所在A标签属性name
        /// </summary>
        public string[] NameAttr { get; set; }

        /// <summary>
        /// 名称所在A标签属性name Str
        /// </summary>
        public string NameAttrStr => string.Join(",", NameAttr);

        /// <summary>
        /// 详情匹配Xpath
        /// </summary>
        public string DetailXpath { get; set; }

        /// <summary>
        /// 详情图片匹配Xpath
        /// </summary>
        public string DetailImgXpath { get; set; }

        /// <summary>
        /// 详情名称匹配Xpath
        /// </summary>
        public string DetailNameXpath { get; set; }

        /// <summary>
        /// 详情完结匹配Xpath
        /// </summary>
        public string DetailEndXpath { get; set; }

        /// <summary>
        /// 站点备注
        /// </summary>
        public string HostRemark { get; set; }

        /// <summary>
        /// 播放地址后缀 
        /// </summary>
        /// <remarks>
        ///  适用于资源站(从前往后匹配)
        /// </remarks>
        public string[] PlayUrlSuffix { get; set; }

        /// <summary>
        /// 详情页年份Xpath
        /// </summary>
        public string YearXpath { get; set; }

        /// <summary>
        /// 采集详情地址 
        /// </summary>
        public string[] CaptureDetailUrl { get; set; }

        /// <summary>
        /// 采集详情匹配Xpath
        /// </summary>
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

        /// <summary>
        /// 配置状态Str
        /// </summary>
        public string SearchConfigStatusStr => SearchConfigStatus.GetDisplayName();
    }
}
