using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IDouBanInfoService _douBanInfoService;

        public UnitTest1()
        {
            var webHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //环境变量
                    var env = hostingContext.HostingEnvironment;
                    hostingContext.Configuration = config.Build();
                    string consulUrl = hostingContext.Configuration[ConsulConfigCenterExt.ConsulConfigUrl];

                    config.InitConfigCenter(hostingContext, consulUrl,
                        $"KdyWeb.NetCore/appsettings.Development.json");
                }).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>();
                }).Build();
            //全局DI容器
            KdyBaseServiceProvider.ServiceProvide = webHost.Services;
            _douBanInfoService = webHost.Services.GetService<IDouBanInfoService>();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var result = await _douBanInfoService.QueryDouBanInfoAsync(new QueryDouBanInfoInput()
            {
                Key = "只有"
            });
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
