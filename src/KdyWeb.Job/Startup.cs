using System;
using System.IO;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.HttpApi;
using KdyWeb.Job.Extensions;
using KdyWeb.Service.ServiceExtension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

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
            services.AddKdyDefaultExt()
                .AddOldJob()
                .InitHangFireServer(Configuration);

            //注入自用站点解析
            services.AddKdyWebParse(Configuration);
            services.AddKdyPageParse(Configuration);

            //Swagger
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("normal", new OpenApiInfo
                {
                    Title = "公用Api",
                    Version = "v2"
                });

                option.SwaggerDoc("login", new OpenApiInfo
                {
                    Title = "登录Api",
                    Version = "v2"
                });

                option.SwaggerDoc("manager", new OpenApiInfo
                {
                    Title = "管理Api",
                    Version = "v2"
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.AddKdyDefaultExt();
            app.InitDashboard();

            if (env.IsDevelopment())
            {
                //swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/normal/swagger.json", "normal");
                    c.SwaggerEndpoint("/swagger/login/swagger.json", "login");
                    c.SwaggerEndpoint("/swagger/manager/swagger.json", "manager");
                    // c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
                });
            }
        }
    }
}
