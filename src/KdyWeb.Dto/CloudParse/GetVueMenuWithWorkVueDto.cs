using System.Collections.Generic;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// Admin work权限菜单
    /// </summary>
    public class GetVueMenuWithWorkVueDto
    {
        /// <summary>
        /// 父节点路径
        /// </summary>
        public string ParentPath { get; set; }

        /// <summary>
        /// 菜单Url
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 路由名
        /// </summary>
        /// <remarks>
        ///  这里的名程序需要跟前端页面中的RouteName 一致，否则Cacheable 会失效
        /// </remarks>
        public string RouteName { get; set; }

        /// <summary>
        /// icon前缀
        /// </summary>
        public string IconPrefix { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否为根目录
        /// </summary>
        public bool IsRootPath { get; set; }

        /// <summary>
        /// ??
        /// </summary>
        public string Badge { get; set; }

        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool Cacheable { get; set; }

        /// <summary>
        /// 本地文件路径 不包含views
        /// </summary>
        public string LocalFilePath { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<GetVueMenuWithWorkVueDto> Children { get; set; }
    }
}
