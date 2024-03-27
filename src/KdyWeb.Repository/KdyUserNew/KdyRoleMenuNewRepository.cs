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
    /// 角色菜单 仓储实现
    /// </summary>
    public class KdyRoleMenuNewRepository : KdyRepository<KdyRoleMenuNew, long>, IKdyRoleMenuNewRepository
    {
        public KdyRoleMenuNewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        /// <summary>
        /// 根据角色获取角色激活菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<KdyMenuNew>> GetActivatedMenuByRoleNameAsync(string roleName)
        {
            var dbList = await DbSet
                .Include(a => a.KdyMenuNew)
                .Where(a => a.RoleName == roleName &&
                            a.IsActivate)
                .Select(a => a.KdyMenuNew)
                .OrderByDescending(a => a.OrderBy)
                .ToListAsync();

            return dbList;
        }

        /// <summary>
        /// 根据角色获取角色激活菜单(相同的去重)
        /// </summary>
        /// <returns></returns>
        public async Task<List<KdyMenuNew>> GetActivatedMenuByRoleNameAsync(string[] roleNames)
        {
            var dbList = await DbSet
                .Include(a => a.KdyMenuNew)
                .Where(a => roleNames.Contains(a.RoleName) &&
                            a.IsActivate)
                .Select(a => a.KdyMenuNew)
                .OrderByDescending(a => a.OrderBy)
                .Distinct()
                .ToListAsync();

            return dbList;
        }

        /// <summary>
        /// 根据角色获取所有菜单配置
        /// </summary>
        /// <returns></returns>
        public async Task<List<KdyRoleMenuNew>> GetAllMenuByRoleNameAsync(string roleName)
        {
            return await DbSet.Where(a => a.RoleName == roleName).ToListAsync();
        }
    }
}
