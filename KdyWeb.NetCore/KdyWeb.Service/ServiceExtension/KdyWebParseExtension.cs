using KdyWeb.Dto.HttpCapture;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.Service.ServiceExtension
{
    /// <summary>
    /// 自用站点解析 扩展
    /// </summary>
    public static class KdyWebParseExtension
    {
        /// <summary>
        /// 注入自用站点解析
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddKdyWebParse(this IServiceCollection services, IConfiguration configuration)
        {
            var cctv = configuration.GetSection(KdyWebServiceConst.KdyWebParseConfig.CctvConfig);
            if (cctv != null)
            {
                services.Configure<CctvParseConfig>(cctv);
            }

            var normal = configuration.GetSection(KdyWebServiceConst.KdyWebParseConfig.NormalConfig);
            if (normal != null)
            {
                services.Configure<NormalParseConfig>(normal);
            }

            return services;
        }
    }
}
