using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.Repository
{
    /// <summary>
    /// 工作单元 实现
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly DbContext _readDbContext;
        /// <summary>
        /// 用户登录信息
        /// </summary>
        protected ILoginUserInfo LoginUserInfo;

        public UnitOfWork(IRwContextFactory contextFactory)
        {
            _dbContext = new ReadWriteContext(contextFactory.GetDbContext(ReadWrite.Write));
            _readDbContext = new ReadWriteContext(contextFactory.GetDbContext(ReadWrite.Read));
            LoginUserInfo = KdyBaseServiceProvider.ServiceProvide.GetService<ILoginUserInfo>();
        }

        public int SaveChanges()
        {
            InitEntityDefaultValue();
            var changes = _dbContext.SaveChanges();
            return changes;
        }

        public async Task<int> SaveChangesAsync()
        {
            InitEntityDefaultValue();
            var changes = await _dbContext.SaveChangesAsync();
            if (changes > 0)
            {
                //保存成功后 重置所有读库上下文跟踪状态为 未更改 todo:saveChange如果不重置 先保存后更改会报错 
                foreach (var item in _dbContext.ChangeTracker.Entries()
                    .Where(a => a.Entity != null))
                {
                    _readDbContext.Attach(item.Entity).State = EntityState.Unchanged;
                }
            }

            return changes;
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

        /// <summary>
        /// 初始化实体类默认值
        /// </summary>
        private void InitEntityDefaultValue()
        {
            var entries = _dbContext.ChangeTracker.Entries()
                .Where(a => (a.Entity is IBaseTimeKey))
                .ToList();

            foreach (var entry in entries)
            {
                if ((entry.Entity is IBaseTimeKey) == false)
                {
                    continue;
                }

                var entity = (IBaseTimeKey)entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            entity.CreatedUserId ??= LoginUserInfo.UserId;
                            if (entity.CreatedTime.Year == 1)
                            {
                                //无默认值时
                                entity.CreatedTime = DateTime.Now;
                            }
                            break;
                        }
                    case EntityState.Modified:
                        {
                            entity.ModifyUserId = LoginUserInfo.UserId;
                            entity.ModifyTime = DateTime.Now;
                            break;
                        }
                }
            }
        }

        public virtual void Dispose()
        {
            _dbContext?.Dispose();
            _readDbContext?.Dispose();
        }
    }
}
