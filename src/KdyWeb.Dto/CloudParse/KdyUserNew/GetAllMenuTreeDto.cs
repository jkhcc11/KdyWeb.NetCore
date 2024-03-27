using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity;
using System.Collections.Generic;
using System.Linq;
using KdyWeb.Entity.KdyUserNew;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 获取所有菜单树
    /// </summary>
    [AutoMap(typeof(KdyMenuNew))]
    public class GetAllMenuTreeDto : BaseEntityDto<long>
    {
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
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }

        public List<GetAllMenuTreeDto> Children { get; set; }

        /// <summary>
        /// 生成菜单树
        /// </summary>
        /// <param name="menuList">所有菜单</param>
        /// <param name="rootMenuId">根节点Id</param>
        /// <returns></returns>
        public static List<GetAllMenuTreeDto> GenerateMenuTree(IList<GetAllMenuTreeDto> menuList,
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
