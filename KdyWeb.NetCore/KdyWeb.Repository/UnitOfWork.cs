using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Repository
{
    /// <summary>
    /// 工作单元 实现
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void UnchangedAll()
        {
            //遍历当前跟踪的实体类 全部放弃更改
            _dbContext.ChangeTracker.Entries()
                .Where(e => e.Entity != null).ToList()
                .ForEach(e => e.State = EntityState.Detached);
        }
    }
}
