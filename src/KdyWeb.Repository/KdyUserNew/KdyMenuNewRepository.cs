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
    /// 菜单 仓储实现
    /// </summary>
    public class KdyMenuNewRepository : KdyRepository<KdyMenuNew, long>, IKdyMenuNewRepository
    {
        public KdyMenuNewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 路由名是否存在
        /// </summary>
        /// <param name="routeName">路由名</param>
        /// <returns></returns>
        public async Task<bool> RouteNameExistAsync(string routeName)
        {
            return await DbSet.AnyAsync(a => a.RouteName == routeName);
        }

        /// <summary>
        /// 路由名是否存在
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <param name="routeName">路由名</param>
        /// <returns></returns>
        public async Task<bool> RouteNameExistAsync(long menuId, string routeName)
        {
            return await DbSet.AnyAsync(a => a.RouteName == routeName &&
                                             a.Id != menuId);
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<KdyMenuNew>> GetAllMenuAsync()
        {
            return await DbSet
                .OrderByDescending(a => a.OrderBy)
                .ToListAsync();
        }
    }
}
