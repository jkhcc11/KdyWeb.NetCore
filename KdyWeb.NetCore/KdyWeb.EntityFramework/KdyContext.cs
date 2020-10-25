using System.Reflection;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.EntityFramework
{

    /// <summary>
    /// 公众号数据库上下文
    /// </summary>
    public class KdyContext : DbContext
    {
        /// <summary>
        /// 注入使用的构造方法
        /// </summary>
        /// <param name="options"></param>
        public KdyContext(DbContextOptions<KdyContext> options) : base(options)
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
