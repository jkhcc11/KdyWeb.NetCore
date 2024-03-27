﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.KdyUserNew;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// Admin work权限菜单
    /// </summary>
    [AutoMap(typeof(KdyMenuNew))]
    public class GetVueMenuWithWorkVueDto
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [JsonIgnore]
        public long Id { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        [JsonIgnore]
        public long ParentMenuId { get; set; }

        /// <summary>
        /// 排序越大越靠前
        /// </summary>
        [JsonIgnore]
        public int OrderBy { get; set; }

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

        ///// <summary>
        ///// ??
        ///// </summary>
        //public string Badge { get; set; }

        /// <summary>
        /// 是否缓存
        /// </summary>
        [SourceMember(nameof(KdyMenuNew.IsCache))]
        public bool Cacheable { get; set; }

        /// <summary>
        /// 本地文件路径 不包含views
        /// </summary>
        public string LocalFilePath { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<GetVueMenuWithWorkVueDto> Children { get; set; }

        /// <summary>
        /// 生成菜单树并处理子节点的父路径
        /// </summary>
        /// <param name="menuList">所有菜单</param>
        /// <param name="rootMenuId">根节点Id</param>
        /// <param name="parentMenuUrl">父路径</param>
        /// <returns></returns>
        public static List<GetVueMenuWithWorkVueDto> GenerateMenuTreeAndParentHandler(IList<GetVueMenuWithWorkVueDto> menuList,
            long rootMenuId = 0, string parentMenuUrl = null)
        {
            var menuTree = menuList
                .Where(menu => menu.ParentMenuId == rootMenuId) // 筛选出对应级别的菜单
                .Select(menu =>
                {
                    // 如果传递了父菜单的MenuUrl，则将其设置为当前菜单的ParentPath
                    if (parentMenuUrl != null)
                    {
                        menu.ParentPath = parentMenuUrl;
                    }

                    // 递归处理子菜单，传递当前菜单的MenuUrl给子菜单
                    menu.Children = GenerateMenuTreeAndParentHandler(menuList, menu.Id, menu.MenuUrl);
                    return menu;
                })
                .OrderByDescending(a => a.OrderBy)
                .ToList();

            return menuTree.Any() ? menuTree : null; // 如果没有任何菜单，返回null
        }
    }
}
