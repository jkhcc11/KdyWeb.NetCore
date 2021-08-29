using Exceptionless;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KdyWeb.BaseInterface.Extensions
{
    /// <summary>
    /// ExceptionLess扩展
    /// </summary>
    public static class ExceptionLessExt
    {
        /// <summary>
        /// 配置ExceptionLess日志
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder ConfigureExceptionLessLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddLogging(builder =>
                {
                    var exceptionLessSection = context.Configuration.GetSection("Exceptionless");
                    if (exceptionLessSection == null)
                    {
                        throw new KdyCustomException($"启动ExceptionLess异常，未配置Exceptionless节点信息。In:{nameof(ConfigureExceptionLessLogging)}");
                    }

                    string apiKey = exceptionLessSection.GetValue<string>("ApiKey"),
                        serverUrl = exceptionLessSection.GetValue<string>("ServerUrl");
                    builder.AddExceptionless(apiKey, serverUrl);
                    //var client = new ExceptionlessClient(configure =>
                    //{
                    //    configure.ApiKey = apiKey;
                    //    configure.ServerUrl = serverUrl;
                    //});
                    // builder.AddExceptionless(client);
                });
            });

            return hostBuilder;
        }

        ///// <summary>
        ///// 初始化 ExceptionLess old
        ///// </summary>
        //public static IApplicationBuilder InitExceptionLess(this IApplicationBuilder app, IConfiguration configuration)
        //{
        //    //ExceptionlessClient.Default.Configuration.UseFolderStorage();
        //    //var lessConfig = new ConfigurationBuilder()
        //    //    .SetBasePath(Directory.GetCurrentDirectory())
        //    //    .AddJsonFile("appsettings.json", optional: true)
        //    //    .Build();
        //    var key = configuration.GetValue<string>("ExceptionLess:ApiKey");
        //    if (string.IsNullOrEmpty(key))
        //    {
        //        throw new Exception("启动ExceptionLess异常，未配置ExceptionLess节点");
        //    }
        //    app.UseExceptionless(configuration);
        //    return app;
        //}
    }
}
