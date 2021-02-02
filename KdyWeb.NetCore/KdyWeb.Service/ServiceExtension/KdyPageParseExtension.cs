using System.Net.Http;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.PageParse;
using KdyWeb.Service.HttpCapture;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.Service.ServiceExtension
{
    /// <summary>
    /// 站点页面解析 扩展
    /// </summary>
    public static class KdyPageParseExtension
    {
        /// <summary>
        /// 注入站点页面解析
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddKdyPageParse(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPageParseService<NormalPageParseOut, NormalPageParseInput>, NormalPageParseService>();
            services.AddScoped<IPageParseService<NormalPageParseOut, NormalPageParseInput>, ZyPageParseService>();
            services.AddScoped<IPageParseService<NormalPageParseOut, NormalPageParseInput>, DownPageParseService>();
            return services;
        }
    }
}
