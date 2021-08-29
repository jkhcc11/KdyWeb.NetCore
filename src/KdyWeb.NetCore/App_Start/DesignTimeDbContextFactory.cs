using KdyWeb.BaseInterface;
using KdyWeb.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.NetCore
{
    /// <summary>
    /// DesignTime工厂
    /// </summary>
    /// <remarks>
    /// 数据迁移时使用
    /// </remarks>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ReadWriteContext>
    {
        /// <summary>
        /// 写库连接字符串
        /// </summary>
        private readonly string _writeConnStr;
        public DesignTimeDbContextFactory()
        {
            var configuration = KdyBaseServiceProvider.ServiceProvide.GetService<IConfiguration>();
            _writeConnStr = configuration.GetConnectionString("WeChatDb");
        }

        public ReadWriteContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
            optionsBuilder.UseSqlServer(_writeConnStr);

            return new ReadWriteContext(optionsBuilder.Options);
        }
    }
}
