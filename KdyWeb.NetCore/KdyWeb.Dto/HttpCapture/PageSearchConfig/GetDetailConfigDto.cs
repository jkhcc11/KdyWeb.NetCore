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
    }
}
