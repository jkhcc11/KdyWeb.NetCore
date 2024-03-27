using KdyWeb.Entity.KdyUserNew;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.CloudParse.KdyUserNew
{
    /// <summary>
    /// 创建和更新角色
    /// </summary>
    public class CreateAndUpdateRoleInput
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [StringLength(KdyMenuNew.RoleMenuCommonLength)]
        [Required(ErrorMessage = "角色名必填")]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色标识
        /// </summary>
        /// <remarks>
        /// 对应Ids那边的角色名 eg:kdyadmin
        /// </remarks>
        [StringLength(KdyMenuNew.RoleMenuCommonLength)]
        [Required(ErrorMessage = "角色标识必填")]
        public string RoleFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(KdyMenuNew.RoleMenuCommonLength)]
        public string RoleRemark { get; set; }
    }
}
