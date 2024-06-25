using KdyWeb.BaseInterface.Extensions;
using KdyWeb.HttpApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace KdyWeb.NetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            KdyWebExtension.GeneralHostBuilderByConfig(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureExceptionLessLogging();

    }
}
