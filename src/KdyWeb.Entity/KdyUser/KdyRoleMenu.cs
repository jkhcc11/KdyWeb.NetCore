using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class KdyRoleMenu : BaseEntity<int>,IsActivate
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivate { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public KdyMenu KdyMenu { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public KdyRole KdyRole { get; set; }
    }
}
