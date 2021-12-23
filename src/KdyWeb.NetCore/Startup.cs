using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Filter;
using KdyWeb.HttpApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.NetCore
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
            //自定义模型校验
            services.AddControllersWithViews(options => { options.Filters.Add<ModelStateValidFilter>(); });

            services.AddKdyDefaultExt();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.HttpOnly = HttpOnlyPolicy.Always;
            //    options.MinimumSameSitePolicy = SameSiteMode.Lax;
            //});

            //初始化第三方组件
            services.InitHangFire(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.AddKdyDefaultExt();
            //app.UseKdyAuth(new KdyAuthMiddlewareOption()
            //{
            //    LoginUrl = "/User/Login"
            //}).UseKdyLog();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
