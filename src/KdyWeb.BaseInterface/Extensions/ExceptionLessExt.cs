using Exceptionless;
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
        /// <remarks>
        /// 未配置使用默认
        /// </remarks>
        /// <returns></returns>
        public static IHostBuilder ConfigureExceptionLessLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddLogging(builder =>
                {
                    string? apiKey = context.Configuration.GetValue<string>(ExceptionLessConfigApiKey),
                        serverUrl = context.Configuration.GetValue<string>(ExceptionLessConfigApiUrl);
                    if (string.IsNullOrEmpty(apiKey) == false &&
                        string.IsNullOrEmpty(serverUrl) == false)
                    {
                        if (context.HostingEnvironment.IsProduction())
                        {
                            builder.ClearProviders();
                        }

                        builder.AddExceptionless(apiKey, serverUrl);
                    }
                });
            });

            return hostBuilder;
        }
    }
}
