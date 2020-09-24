using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using AutoMapper;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.InterfaceFlag;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto;
using KdyWeb.EntityFramework;
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
        /// 注入所有Service和Repository
        /// </summary>
        public static void KdyRegister(IServiceCollection services)
        {
            //加载当前项目程序集
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "KdyWeb.*.dll").Select(Assembly.LoadFrom).ToArray();

            //所有程序集类型声明
            var allTypes = new List<System.Type>();
            foreach (var itemAssemblies in assemblies)
            {
                allTypes.AddRange(itemAssemblies.GetTypes());
                // allTypes.AddRange(itemAssemblies.Assembly.GetTypes());
            }

            #region 同一个请求获取多次会得到相同的实例
            //公用的接口
            var baseType = typeof(IKdyScoped);
            //过滤需要用到的服务声明接口
            var useType = allTypes.Where(a => baseType.IsAssignableFrom(a) && a.IsAbstract == false).ToList();
            foreach (var item in useType)
            {
                //该服务所属接口
                var currentInterface = item.GetInterfaces().FirstOrDefault(a => a.Name.EndsWith(item.Name));
                if (currentInterface == null)
                {
                    continue;
                }

                //每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例
                services.AddScoped(currentInterface, item);
            }
            #endregion

            #region 每次请求都是不同的实例
            //公用的接口
            baseType = typeof(IKdyTransient);
            //过滤需要用到的服务声明接口
            useType = allTypes.Where(a => baseType.IsAssignableFrom(a) && a.IsAbstract == false).ToList();
            foreach (var item in useType)
            {
                //该服务所属接口
                var currentInterface = item.GetInterfaces().FirstOrDefault(a => a.Name.EndsWith(item.Name));
                if (currentInterface == null)
                {
                    continue;
                }

                //每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例
                services.AddTransient(currentInterface, item);
            }
            #endregion

            #region 单例注入
            //公用的接口
            baseType = typeof(IKdySingleton);
            //过滤需要用到的服务声明接口
            useType = allTypes.Where(a => baseType.IsAssignableFrom(a) && a.IsAbstract == false).ToList();
            foreach (var item in useType)
            {
                //该服务所属接口
                var currentInterface = item.GetInterfaces().FirstOrDefault(a => a.Name.EndsWith(item.Name));
                if (currentInterface == null)
                {
                    continue;
                }

                //单例注入
                services.AddSingleton(currentInterface, item);
            }
            #endregion

            //为了后面获取HttpContext
            services.AddHttpContextAccessor()
                .AddSingleton<IKdyLog, KdyLogForExceptionLess>();
        }

        /// <summary>
        /// 初始化注入
        /// </summary>
        public static void KdyRegisterInit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<KdyContext>(options =>
            {
                var connectionStr = configuration.GetConnectionString("WeChatDb");
                options.UseSqlServer(connectionStr);
            });
            //todo: 必需注入此关系 后面仓储DbContext才可以使用
            services.AddScoped<DbContext, KdyContext>();

            KdyRegister(services);

            //AutoMapper注入
            //https://www.codementor.io/zedotech/how-to-using-automapper-on-asp-net-core-3-0-via-dependencyinjection-zq497lzsq
            services.AddAutoMapper(typeof(KdyMapperInit));

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
        }
    }
}
