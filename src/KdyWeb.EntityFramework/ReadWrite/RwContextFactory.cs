using System;
using System.Linq;
using KdyWeb.BaseInterface.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.EntityFramework
{
    /// <summary>
    /// 多数据库配置工厂 实现
    /// </summary>
    public class RwContextFactory : IRwContextFactory
    {
        /// <summary>
        /// 写库连接字符串
        /// </summary>
        private readonly string _writeConnStr;
        /// <summary>
        /// 多从库连接字符串
        /// </summary>
        private readonly string[] _readConnStr;

        public RwContextFactory(IConfiguration configuration)
        {
            _writeConnStr = configuration.GetConnectionString("WeChatDb");
            _readConnStr = configuration.GetSection("ConnectionStrings:ReadConn")
                .Get<string[]>();
            if (_readConnStr == null || _readConnStr.Any() == false)
            {
                _readConnStr = new[] { _writeConnStr };
            }
        }

        public DbContextOptions<ReadWriteContext> GetDbContext(ReadWrite rw)
        {
            var optionBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
            if (rw == ReadWrite.Write)
            {
                optionBuilder.UseSqlServer(_writeConnStr);
                return optionBuilder.Options;
            }

            var randRead = _readConnStr[new Random().Next(0, _readConnStr.Length)];
            optionBuilder.UseSqlServer(randRead);
            return optionBuilder.Options;
        }
    }
}
