using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.KdyUserNew;

namespace KdyWeb.IRepository.KdyUserNew
{
    /// <summary>
    /// 角色 仓储接口
    /// </summary>
    public interface IKdyRoleNewRepository : IKdyRepository<KdyRoleNew, long>
    {
        /// <summary>
        /// 角色标识是否存在
        /// </summary>
        /// <param name="roleFlag">角色标识</param>
        /// <returns></returns>
        Task<bool> RoleFlagExistAsync(string roleFlag);

        /// <summary>
        /// 角色标识是否存在
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleFlag">角色标识</param>
        /// <returns></returns>
        Task<bool> RoleFlagExistAsync(long roleId, string roleFlag);

        /// <summary>
        /// 根据角色标识查询角色信息
        /// </summary>
        /// <returns></returns>
        Task<List<KdyRoleNew>> GetRoleListByRoleFlagAsync(string[] roleFlags);
    }
}
