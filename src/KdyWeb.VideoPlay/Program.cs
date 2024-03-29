using KdyWeb.BaseInterface.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
                    //��������
                    var env = context.HostingEnvironment;
                    context.Configuration = config.Build();
                    var consulUrl = context.Configuration[ConsulConfigCenterExt.ConsulConfigUrl];
                    config.InitConfigCenter(context, consulUrl,
                        $"{env.ApplicationName}/appsettings.{env.EnvironmentName}.json");
                })
                //.ConfigureServices((context, service) =>
                //{
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureExceptionLessLogging();
    }
}
