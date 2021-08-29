using AutoMapper;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [AutoMap(typeof(KdyRole))]
    public class KdyRoleDto
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        public KdyRoleType KdyRoleType { get; set; }

        /// <summary>
        /// 角色类型Str
        /// </summary>
        public string KdyRoleTypeStr => KdyRoleType.GetDisplayName();

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivate { get; set; }

    }
}
