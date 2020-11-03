using System.Linq;
using System.Threading.Tasks;
using KdyWeb.EntityFramework;
using KdyWeb.EntityFramework.ReadWrite;
using KdyWeb.IRepository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Repository
{
    /// <summary>
    /// 工作单元 实现
    /// </summary>
    public class UnitOfWork : IRwUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly DbContext _readDbContext;

        public UnitOfWork(IRwContextFactory contextFactory)
        {
            _dbContext = new ReadWriteContext(contextFactory.GetDbContext(ReadWrite.Write));
            _readDbContext = new ReadWriteContext(contextFactory.GetDbContext(ReadWrite.Read));
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

        public DbContext GetCurrentDbContext(ReadWrite rw)
        {
            if (rw == ReadWrite.Write)
            {
                return _dbContext;
            }

            return _readDbContext;
        }
    }
}
