using System.Reflection;
using KdyWeb.BaseInterface.BaseModel;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.EntityFramework
{
    /// <summary>
    /// 读写上写文
    /// </summary>
    public class ReadWriteContext : DbContext
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ReadWriteContext(DbContextOptions<ReadWriteContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //保证每个实体都是使用单独的Fluent-Api
            //加载当前 所有继承了IEntityTypeConfiguration的配置类
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            //获取所有数据实体类
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //实现了软删除接口的实体类
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }
    }
}
