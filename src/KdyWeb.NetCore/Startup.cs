using System;
using System.IO;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Filter;
using KdyWeb.HttpApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

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

            services.AddKdyDefaultExt(Configuration);

            //初始化第三方组件
            services.InitHangFire(Configuration);

            //Swagger
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "图床Api",
                    Version = "v1"
                });

                //  option.ExampleFilters();
                option.OperationFilter<AddResponseHeadersFilter>();

                var xmlPath = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Directory.GetFiles(xmlPath, "KdyWeb.*.xml");
                foreach (var item in filePath)
                {
                    option.IncludeXmlComments(item, true);
                }

                option.OperationFilter<AddResponseHeadersFilter>();

                //授权
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Authorization format : Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                //在Header中添加Token
                option.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

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
