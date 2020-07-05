using System;
using System.IO;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// Hangfire扩展
    /// </summary>
    public static class HangFireExt
    {
        /// <summary>
        /// 初始化Hangfire服务端
        /// </summary>
        /// <param name="services"></param>
        public static void InitHangFireServer(this IServiceCollection services)
        {
            services.AddHangfireServer();
            services.InitHangFire();

        }

        /// <summary>
        /// 初始化Hangfire客户端
        /// </summary>
        /// <param name="services"></param>
        public static void InitHangFire(this IServiceCollection services)
        {
            var hangFireConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            var connectionStr = hangFireConfig.GetConnectionString("HangFireConnStr");
            if (string.IsNullOrEmpty(connectionStr))
            {
                throw new Exception("启动Hangfire异常，找不到连接字符串");
            }

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionStr, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

        }

        /// <summary>
        /// 初始化面板
        /// </summary>
        public static void InitDashboard(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/kdyHangFire");
        }
    }
}
