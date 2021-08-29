using Exceptionless;
using Exceptionless.Logging;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Test
{
    /// <summary>
    /// 测试抽象基类
    /// </summary>
    /// <typeparam name="TService">待测试服务接口 <br/>  eg: IFeedBackInfoService</typeparam>
    public abstract class BaseTest<TService>
    {
        protected IHost _host;
        /// <summary>
        /// 待测试的服务
        /// </summary>
        protected readonly TService _service;

        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogger _logger;

        protected BaseTest()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    hostingContext.Configuration = config.Build();
                    var consulUrl = hostingContext.Configuration[ConsulConfigCenterExt.ConsulConfigUrl];

                    config.InitConfigCenter(hostingContext, consulUrl,
                        $"KdyWeb.NetCore/appsettings.Test.json");
                }).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>();
                })
                .ConfigureExceptionLessLogging()
                .Build();
            //全局DI容器
            KdyBaseServiceProvider.ServiceProvide = _host.Services;
            KdyBaseServiceProvider.HttpContextAccessor = _host.Services.GetService<IHttpContextAccessor>();

            _service = _host.Services.GetService<TService>();
            _logger = _host.Services.GetService<ILoggerFactory>().CreateLogger(GetType());
        }
    }
}
