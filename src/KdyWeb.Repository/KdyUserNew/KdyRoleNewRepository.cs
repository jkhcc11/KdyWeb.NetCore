using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.KdyUserNew;
using KdyWeb.IRepository.KdyUserNew;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Repository.KdyUserNew
{
    /// <summary>
    /// 角色 仓储实现
    /// </summary>
    public class KdyRoleNewRepository : KdyRepository<KdyRoleNew, long>, IKdyRoleNewRepository
    {
        public KdyRoleNewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 角色标识是否存在
        /// </summary>
        /// <param name="roleFlag">角色标识</param>
        /// <returns></returns>
        public async Task<bool> RoleFlagExistAsync(string roleFlag)
        {
            return await DbSet.AnyAsync(a => a.RoleFlag == roleFlag);
        }

        /// <summary>
        /// 角色标识是否存在
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleFlag">角色标识</param>
        /// <returns></returns>
        public async Task<bool> RoleFlagExistAsync(long roleId, string roleFlag)
        {
            return await DbSet.AnyAsync(a => a.RoleFlag == roleFlag &&
                                             a.Id != roleId);
        }

        /// <summary>
        /// 根据角色标识查询角色信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<KdyRoleNew>> GetRoleListByRoleFlagAsync(string[] roleFlags)
        {
            return await DbSet
                .Where(a => roleFlags.Contains(a.RoleFlag))
                .ToListAsync();
        }
    }
}
