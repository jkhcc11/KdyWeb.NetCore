using System;
using System.IO;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.RecurringJobExtensions;
using Hangfire.SqlServer;
using KdyWeb.BaseInterface.HangFire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KdyWeb.BaseInterface.Extensions
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
        /// <param name="configuration"></param>
        public static IServiceCollection InitHangFireServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KdyHangFireAuthOption>(configuration.GetSection("HangFireAuthConfig"));

            services.AddHangfireServer(config =>
            {
                config.Queues = new[]
                {
                    "default",
                    HangFireQueue.Email,
                    HangFireQueue.Capture,
                    HangFireQueue.DouBan,
                    HangFireQueue.GameCheck
                };
            });
            services.InitHangFire(configuration);
            return services;
        }

        /// <summary>
        /// 初始化Hangfire客户端
        /// </summary>
        public static IServiceCollection InitHangFire(this IServiceCollection services, IConfiguration configuration)
        {
            //var hangFireConfig = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json", optional: true)
            //    .Build();
            //var connectionStr = hangFireConfig.GetConnectionString("HangFireConnStr");

            var connectionStr = configuration.GetConnectionString("HangFireConnStr");
            if (string.IsNullOrEmpty(connectionStr))
            {
                throw new Exception("启动Hangfire异常，找不到连接字符串");
            }

            var recurringJobFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recurringjob.json");
            return services.AddHangfire(hgConfig =>
            {
                hgConfig
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
                    });

                if (File.Exists(recurringJobFile))
                {
                    hgConfig.UseRecurringJob(recurringJobFile);
                }
            });

        }

        /// <summary>
        /// 初始化面板
        /// </summary>
        public static IApplicationBuilder InitDashboard(this IApplicationBuilder app)
        {
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute()
            {
                Attempts = 5,
                DelaysInSeconds = new[] { 10, 15, 20 }
            });

            var hangFireOption = app.ApplicationServices.GetService<IOptions<KdyHangFireAuthOption>>()?.Value;
            if (hangFireOption == null)
            {
                return app;
            }

            //启用base认证
            var dashboardOpt = new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = hangFireOption.RequireSsl,
                        SslRedirect = hangFireOption.SslRedirect,
                        LoginCaseSensitive = hangFireOption.LoginCaseSensitive,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = hangFireOption.UserName,
                                PasswordClear = hangFireOption.Pwd
                            }
                        }
                    })
                }
            };

            app.UseHangfireDashboard(hangFireOption.BasePath, dashboardOpt);
            return app;
        }
    }
}
