using KdyWeb.BaseInterface.Service;
using KdyWeb.HttpApi;
using KdyWeb.Service.ServiceExtension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.Test
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddKdyDefaultExt(Configuration);

            //注入自用站点解析
            services.AddKdyWebParse(Configuration);
            services.AddKdyPageParse(Configuration);

            services.AddTransient<ILoginUserInfo, LoginUserInfo>(a => new LoginUserInfo(null)
            {
                UserId = 1470430508717576192
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
