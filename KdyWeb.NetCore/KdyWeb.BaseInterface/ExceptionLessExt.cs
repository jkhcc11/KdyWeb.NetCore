using System;
using System.IO;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// ExceptionLess扩展
    /// </summary>
    public static class ExceptionLessExt
    {
        /// <summary>
        /// 初始化 ExceptionLess
        /// </summary>
        public static void InitExceptionLess(this IApplicationBuilder app)
        {
            ExceptionlessClient.Default.Configuration.UseInMemoryStorage();
            var lessConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            if (string.IsNullOrEmpty(lessConfig["ExceptionLess:ApiKey"]))
            {
                throw new Exception("启动ExceptionLess异常，未配置ExceptionLess节点");
            }

            app.UseExceptionless(lessConfig);
        }
    }
}
