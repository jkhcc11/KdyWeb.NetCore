using System.Net.Http;
using AutoMapper;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Dto;
using KdyWeb.EntityFramework;
using KdyWeb.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KdyWeb.NetCore
{
    /// <summary>
    /// 初始化注入
    /// </summary>
    public static class KdyInit
    {
        /// <summary>
        /// 初始化注入
        /// </summary>
        public static void KdyRegisterInit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReadWriteContext>(options =>
            {
                var connectionStr = configuration.GetConnectionString("WeChatDb");
                options.UseSqlServer(connectionStr);
            });

            services.KdyRegister();

            //注入通用泛型仓储
            services.TryAdd(ServiceDescriptor.Scoped(typeof(IKdyRepository<>), typeof(CommonRepository<>)));
            services.TryAdd(ServiceDescriptor.Scoped(typeof(IKdyRepository<,>), typeof(CommonRepository<,>)));

            //AutoMapper注入
            //https://www.codementor.io/zedotech/how-to-using-automapper-on-asp-net-core-3-0-via-dependencyinjection-zq497lzsq
            //services.AddAutoMapper(typeof(KdyMapperInit));
            var dtoAssembly = typeof(KdyMapperInit).Assembly;
            var entityAssembly = typeof(BaseEntity<>).Assembly;
            services.AddAutoMapper(dtoAssembly, entityAssembly);

            //注入HttpClient
            services.AddHttpClient(KdyBaseConst.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler()
                {
                    //取消自动跳转
                    AllowAutoRedirect = false,
                    //不自动设置cookie
                    // UseCookies = false
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            //初始化第三方组件
            services.InitHangFire(configuration)
                .InitIdGenerate(configuration)
                .UseRedisCache(configuration)
                .AddMemoryCache();

            services.AddMiniProfile();
        }
    }
}
