using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity
{
    /// <summary>
    /// 菜单信息
    /// </summary>
    public class KdyMenu : BaseEntity<int>, IsActivate
    {
        #region 常量
        /// <summary>
        /// Controller长度
        /// </summary>
        public const int ControllerNameLength = 100;
        /// <summary>
        /// Action长度
        /// </summary>
        public const int ActionNameLength = 100;
        /// <summary>
        /// 菜单名长度
        /// </summary>
        public const int MenuNameLength = 100;

        /// <summary>
        /// 导航Icon长度
        /// </summary>
        public const int NavIconLength = 50;
        #endregion

        /// <summary>
        /// 父节点Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Controller名称
        /// </summary>
        [StringLength(ControllerNameLength)]
        public string ControllerName { get; set; }

        /// <summary>
        /// Action名称
        /// </summary>
        [StringLength(ActionNameLength)]
        public string ActionName { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        [StringLength(MenuNameLength)]
        public string MenuName { get; set; }

        /// <summary>
        /// 排序 
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 是否导航
        /// </summary>
        public bool IsNav { get; set; }

        /// <summary>
        /// 导航图标
        /// </summary>
        [StringLength(NavIconLength)]
        public string NavIcon { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivate { get; set; }

        /// <summary>
        /// 角色菜单
        /// </summary>
        public ICollection<KdyRoleMenu> KdyRoleMenus { get; set; }
    }
}
