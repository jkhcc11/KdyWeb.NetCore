using KdyWeb.BaseInterface.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.KdyUserNew
{
    /// <summary>
    /// 角色（新版）
    /// </summary>
    public class KdyRoleNew : BaseEntity<long>
    {
        /// <summary>
        /// 角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="roleFlag">角色标识</param>
        public KdyRoleNew(string roleName, string roleFlag)
        {
            RoleName = roleName;
            RoleFlag = roleFlag;
        }

        /// <summary>
        /// 角色名
        /// </summary>
        [StringLength(KdyMenuNew.RoleMenuCommonLength)]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色标识
        /// </summary>
        /// <remarks>
        /// 对应Ids那边的角色名 eg:kdyadmin
        /// </remarks>
        [StringLength(KdyMenuNew.RoleMenuCommonLength)]
        public string RoleFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(KdyMenuNew.RoleMenuCommonLength)]
        public string? RoleRemark { get; set; }
    }
}
