using System;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Winton.Extensions.Configuration.Consul;

namespace KdyWeb.NetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     //环境变量
                     var env = hostingContext.HostingEnvironment;
                     hostingContext.Configuration = config.Build();
                     string consulUrl = hostingContext.Configuration[ConsulConfigCenterExt.ConsulConfigUrl];

                     config.InitConfigCenter(hostingContext, consulUrl,
                         $"{env.ApplicationName}/appsettings.{env.EnvironmentName}.json");
                 })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });
        }

    }
}
