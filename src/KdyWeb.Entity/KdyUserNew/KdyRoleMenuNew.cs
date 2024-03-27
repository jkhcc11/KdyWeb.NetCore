using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.KdyUserNew
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class KdyRoleMenuNew : BaseEntity<long>, IIsActivate
    {
        /// <summary>
        /// 角色菜单
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <param name="roleName">角色名</param>
        /// <param name="isActivate">是否激活</param>
        public KdyRoleMenuNew(long menuId, string roleName, bool isActivate)
        {
            MenuId = menuId;
            RoleName = roleName;
            IsActivate = isActivate;
        }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; protected set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; protected set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivate { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public virtual KdyMenuNew? KdyMenuNew { get; set; }

        public void NotActivated()
        {
            IsActivate = false;
        }

        public void Activated()
        {
            IsActivate = true;
        }
    }
}
