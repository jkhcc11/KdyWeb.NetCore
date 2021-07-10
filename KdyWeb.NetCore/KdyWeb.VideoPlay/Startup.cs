using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.MiniProfiler;
using KdyWeb.Service.ServiceExtension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KdyWeb.VideoPlay
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
            //关闭ModelState自动校验
            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.SuppressModelStateInvalidFilter = true;
            });

            //自定义防伪选项
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "play.antiforgery";
                //todo:非同源使用iframe引用时 不能使用Lax 否则cookie不生效,得使用None
                options.Cookie.SameSite = SameSiteMode.None;
                //options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                //优先header 然后form
                options.HeaderName = "_antiforgery";
                //todo:前端必须是application/x-www-form-urlencoded controller用fromform
                options.FormFieldName = "_antiforgery";

                //如果不为true 则其他站无法使用iframe引用
                //https://stackoverflow.com/questions/40523565/asp-net-core-x-frame-options-strange-behavior
                options.SuppressXFrameOptionsHeader = true;
            });

            //添加自动防伪标记
            services.AddControllersWithViews(options =>
                    {
                        //会跳get-HEAD-OPTIONS-TRACE 请求
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                        options.Filters.Add<ModelStateValidFilter>();
                    })
                .AddNewtonsoftJson(option =>
                {
                    option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });

            services.KdyRegisterInit(Configuration);

            services.AddKdyWebParse(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiniProfile();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseKdyAuth(new KdyAuthMiddlewareOption()
            {
                LoginUrl = "/User/Login"
            }).UseKdyLog();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //全局DI容器
            KdyBaseServiceProvider.ServiceProvide = app.ApplicationServices;
            KdyBaseServiceProvider.HttpContextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>();
           // app.InitExceptionLess(Configuration);
        }
    }
}
