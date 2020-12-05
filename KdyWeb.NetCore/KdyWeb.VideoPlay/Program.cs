using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KdyWeb.VideoPlay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    //环境变量
                    var env = context.HostingEnvironment;
                    context.Configuration = config.Build();
                    string consulUrl = context.Configuration[ConsulConfigCenterExt.ConsulConfigUrl];
                    config.InitConfigCenter(context, consulUrl,
                        $"{env.ApplicationName}/appsettings.{env.EnvironmentName}.json");
                })
                //.ConfigureServices((context, service) =>
                //{
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).
                ConfigureLogging(config =>
                {
                    config.AddExceptionless();
                });
    }
}
