using System.IO;
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

        public ReadWriteContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connStr = configuration.GetConnectionString("WeChatDb");

            var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
            optionsBuilder.UseSqlServer(connStr);

            return new ReadWriteContext(optionsBuilder.Options);
        }
    }
}
