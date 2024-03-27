using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.KdyUserNew;

namespace KdyWeb.Dto.CloudParse.KdyUserNew
{
    /// <summary>
    /// 查询角色列表
    /// </summary>
    [AutoMap(typeof(KdyRoleNew))]
    public class QueryPageRoleDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色标识
        /// </summary>
        /// <remarks>
        /// 对应Ids那边的角色名 eg:kdyadmin
        /// </remarks>
        public string RoleFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string RoleRemark { get; set; }
    }
}
