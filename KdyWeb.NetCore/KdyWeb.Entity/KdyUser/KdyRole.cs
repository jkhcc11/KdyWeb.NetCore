using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity
{
    /// <summary>
    /// 角色
    /// </summary>
    public class KdyRole : BaseEntity<int>, IsActivate
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        [Required]
        public KdyRoleType KdyRoleType { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivate { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public ICollection<KdyUser> KdyUsers { get; set; }

        /// <summary>
        /// 角色菜单
        /// </summary>
        public ICollection<KdyRoleMenu> KdyRoleMenus { get; set; }
    }
}
