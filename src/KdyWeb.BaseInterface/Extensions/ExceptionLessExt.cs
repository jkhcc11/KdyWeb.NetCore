using Exceptionless;
using Exceptionless.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KdyWeb.BaseInterface.Extensions
{
    /// <summary>
    /// ExceptionLess扩展
    /// </summary>
    public static class ExceptionLessExt
    {
        public const string ExceptionLessConfigApiKey = "Exceptionless:ApiKey";
        public const string ExceptionLessConfigApiUrl = "Exceptionless:ServerUrl";

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
                    if (context.HostingEnvironment.IsProduction())
                    {
                        builder.ClearProviders();
                    }

                    string? apiKey = context.Configuration.GetValue<string>(ExceptionLessConfigApiKey),
                        serverUrl = context.Configuration.GetValue<string>(ExceptionLessConfigApiUrl);
                    if (string.IsNullOrEmpty(apiKey) ||
                        string.IsNullOrEmpty(serverUrl))
                    {
                        throw new KdyCustomException($"启动ExceptionLess异常，未配置Exceptionless节点信息。In:{nameof(ConfigureExceptionLessLogging)}");
                    }
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
