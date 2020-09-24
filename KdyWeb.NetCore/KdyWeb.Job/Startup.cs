using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Kdy.StandardJob;
using Kdy.StandardJob.JobService;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.InterfaceFlag;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Service.Job;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Job
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //加载当前项目程序集
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Kdy*.dll").Select(Assembly.LoadFrom).ToArray();

            //所有程序集类型声明
            var allTypes = new List<System.Type>();
            foreach (var itemAssemblies in assemblies)
            {
                allTypes.AddRange(itemAssemblies.GetTypes());
            }

            #region 自动注入Scoped
            //公用的接口
            var baseType = typeof(IKdyJobFlag);
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

            //注入ExceptionLess日志
            services.AddHttpContextAccessor()
                .AddSingleton<IKdyLog, KdyLogForExceptionLess>();
            services.AddControllers();
            services.InitHangFireServer(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.InitDashboard();
            app.InitExceptionLess(Configuration);

        }
    }
}
