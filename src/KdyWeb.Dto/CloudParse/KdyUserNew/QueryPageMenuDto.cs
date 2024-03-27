using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.KdyUserNew;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询菜单列表
    /// </summary>
    [AutoMap(typeof(KdyMenuNew))]
    public class QueryPageMenuDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 父菜单Id
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long ParentMenuId { get; set; }

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
        /// 是否缓存
        /// </summary>
        public bool IsCache { get; set; }

        /// <summary>
        /// 本地文件路径 不包含views
        /// </summary>
        public string LocalFilePath { get; set; }

        /// <summary>
        /// 排序越大越靠前
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<QueryPageMenuDto> Children { get; set; }

        /// <summary>
        /// 生成菜单树
        /// </summary>
        /// <param name="menuList">所有菜单</param>
        /// <param name="rootMenuId">根节点Id</param>
        /// <returns></returns>
        public static List<QueryPageMenuDto> GenerateMenuTree(IList<QueryPageMenuDto> menuList,
            long rootMenuId = 0)
        {
            var menuTree = menuList
                .Where(menu => menu.ParentMenuId == rootMenuId) // 筛选出根级菜单（或者您想要开始的任何级别）
                .Select(menu =>
                {
                    menu.Children = GenerateMenuTree(menuList, menu.Id); // 递归地分配子项
                    return menu;
                })
                .OrderByDescending(a => a.OrderBy)
                .ToList();

            return menuTree.Any() == false ? null : menuTree;
        }
    }
}
