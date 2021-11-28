using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using KdyWeb.BaseInterface.Extensions;

namespace KdyWeb.CloudParseApi
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
                    string consulUrl = context.Configuration[ConsulConfigCenterExt.ConsulConfigUrl],
                        clientName = context.Configuration[ConsulConfigCenterExt.ConfigClientName];
                    if (string.IsNullOrEmpty(clientName) == false)
                    {
                        clientName = "." + clientName;
                    }

                    config.InitConfigCenter(context, consulUrl,
                        $"{env.ApplicationName}/appsettings.{env.EnvironmentName}{clientName}.json");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureExceptionLessLogging();
    }
}
