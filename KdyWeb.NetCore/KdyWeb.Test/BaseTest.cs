using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        protected BaseTest()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                //环境变量
                var env = hostingContext.HostingEnvironment;
                    hostingContext.Configuration = config.Build();
                    string consulUrl = hostingContext.Configuration[ConsulConfigCenterExt.ConsulConfigUrl];

                    config.InitConfigCenter(hostingContext, consulUrl,
                        $"KdyWeb.NetCore/appsettings.Test.json");
                }).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>();
                }).Build();
            //全局DI容器
            KdyBaseServiceProvider.ServiceProvide = _host.Services;

            _service = _host.Services.GetService<TService>();
        }
    }
}
