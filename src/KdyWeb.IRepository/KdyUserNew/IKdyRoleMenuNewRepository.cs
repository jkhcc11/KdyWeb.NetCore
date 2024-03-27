using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.KdyUserNew;

namespace KdyWeb.IRepository.KdyUserNew
{
    /// <summary>
    /// 角色菜单 仓储接口
    /// </summary>
    public interface IKdyRoleMenuNewRepository : IKdyRepository<KdyRoleMenuNew, long>
    {
        /// <summary>
        /// 根据角色获取角色激活菜单
        /// </summary>
        /// <returns></returns>
        Task<List<KdyMenuNew>> GetActivatedMenuByRoleNameAsync(string roleName);

        /// <summary>
        /// 根据角色获取角色激活菜单(相同的去重)
        /// </summary>
        /// <returns></returns>
        Task<List<KdyMenuNew>> GetActivatedMenuByRoleNameAsync(string[] roleNames);

        /// <summary>
        /// 根据角色获取所有菜单配置
        /// </summary>
        /// <returns></returns>
        Task<List<KdyRoleMenuNew>> GetAllMenuByRoleNameAsync(string roleName);
    }
}
