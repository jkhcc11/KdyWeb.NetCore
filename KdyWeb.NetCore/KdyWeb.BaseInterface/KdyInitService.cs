using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KdyWeb.BaseInterface.InterfaceFlag;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.BaseInterface.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 初始化Service和仓储
    /// </summary>
    public static class KdyInitService
    {
        /// <summary>
        /// 注入所有Service和Repository
        /// </summary>
        public static void KdyRegister(this IServiceCollection services)
        {
            //加载当前项目程序集
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "KdyWeb.*.dll").Select(Assembly.LoadFrom).ToArray();

            //所有程序集类型声明
            var allTypes = new List<System.Type>();
            foreach (var itemAssemblies in assemblies)
            {
                allTypes.AddRange(itemAssemblies.GetTypes());
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
    }
}
