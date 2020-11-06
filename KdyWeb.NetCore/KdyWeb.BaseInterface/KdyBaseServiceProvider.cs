using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 用于获取当前DI容器反转创建对象
    /// </summary>
    public class KdyBaseServiceProvider
    {
        /// <summary>
        /// 服务提供
        /// </summary>
        /// <remarks>
        ///  注：必须是AddSingleton和AddTransient注入才可以用此方法获取实例
        ///  详见：https://www.cnblogs.com/xishuai/p/asp-net-core-ioc-di-get-service.html
        /// </remarks>
        public static IServiceProvider ServiceProvide { get; set; }

        /// <summary>
        /// 用于获取HttpContent
        /// </summary>
        public static IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 用于获取Scoped 实例
        /// </summary>
        public static IServiceProvider HttpContextServiceProvide
        {
            get
            {
                var contextServiceProvide = HttpContextAccessor?.HttpContext?.RequestServices;
                if (contextServiceProvide != null)
                {
                    return contextServiceProvide;
                }

                return ServiceProvide;
                //无HttpContext时 创建一个Scope
                // var scope = ServiceProvide.GetService<IServiceScopeFactory>();
                // return scope.CreateScope().ServiceProvider;
            }
        }
    }
}
