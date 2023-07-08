using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using IdentityModel;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Filter;
using KdyWeb.BaseInterface.KdyOptions;
using KdyWeb.Dto;
using KdyWeb.IService;
using KdyWeb.MiniProfiler;
using KdyWeb.Repository;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KdyWeb.HttpApi
{
    public static class KdyWebExtension
    {
        /// <summary>
        /// 添加默认扩展
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddKdyDefaultExt(this IServiceCollection services)
        {
            //清空微软默认的cliam type
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            //关闭ModelState自动校验
            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.SuppressModelStateInvalidFilter = true;
            });
            services.KdyRegister()
                .AddKdyAllRepository()
                .AddKdyMapper();

            services.AddControllers(opt =>
            {
                opt.Filters.Add<ModelStateValidFilter>();
            }).AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //注入HttpClient
            services.AddHttpClient(KdyBaseConst.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler(config => new HttpClientHandler
                {
                    //解析Gzip
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    //取消自动跳转 todo:parse cloud test 
                    AllowAutoRedirect = false
                }
                );

            #region ids
            services.Configure<KdyAuthServerOption>(configuration?.GetSection("AuthServer"));
            var authServer = configuration?.GetSection("AuthServer").Get<KdyAuthServerOption>();
            if (authServer != null)
            {
                //鉴权
                services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.Authority = authServer.AuthHost;
                        options.RequireHttpsMetadata = authServer.IsRequireHttps;
                        options.Audience = authServer.Scope;
                        options.Events = new JwtBearerEvents()
                        {
                            OnTokenValidated = CustomAuthValidated
                        };
                    });

                //策略授权
                services.AddAuthorization(options =>
                {
                    //options.AddPolicy(AuthorizationConst.CloudParseApiConst.NormalPolicy,
                    //    policy =>
                    //        policy.RequireAssertion(context => context.User.HasClaim(c =>
                    //                ((c.Type == JwtClaimTypes.Role && c.Value == adminApiConfiguration.AdministrationRole) ||
                    //                 (c.Type == $"client_{JwtClaimTypes.Role}" && c.Value == adminApiConfiguration.AdministrationRole))
                    //            ) && context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope && c.Value == adminApiConfiguration.OidcApiName)
                    //        ));

                    //登录普通
                    options.AddPolicy(AuthorizationConst.NormalPolicyName.NormalPolicy,
                        policy => policy.RequireAssertion(context =>
                            context.User.HasClaim(c => c.Type == JwtClaimTypes.Role &&
                                                       (c.Value == AuthorizationConst.NormalRoleName.Normal ||
                                                        c.Value == AuthorizationConst.NormalRoleName.SuperAdmin)) &&
                            context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope && c.Value == authServer.Scope)
                        ));

                    //超管
                    options.AddPolicy(AuthorizationConst.NormalPolicyName.SuperAdminPolicy,
                        policy => policy.RequireAssertion(context =>
                            context.User.HasClaim(c => c.Type == JwtClaimTypes.Role &&
                                                       c.Value == AuthorizationConst.NormalRoleName.SuperAdmin) &&
                            context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope && c.Value == authServer.Scope)
                        ));

                    //manager
                    options.AddPolicy(AuthorizationConst.NormalPolicyName.ManagerPolicy,
                        policy => policy.RequireAssertion(context =>
                            context.User.HasClaim(c => c.Type == JwtClaimTypes.Role &&
                                                       (c.Value == AuthorizationConst.NormalRoleName.VodAdmin ||
                                                        c.Value == AuthorizationConst.NormalRoleName.SuperAdmin)) &&
                            context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope && c.Value == authServer.Scope)
                        ));

                    ////登录普通
                    //options.AddPolicy(AuthorizationConst.NormalPolicyName.NormalPolicy,
                    //    policy => policy.RequireAssertion(context =>
                    //        {
                    //            return (context.User.IsInRole(AuthorizationConst.NormalRoleName.SuperAdmin) ||
                    //                    context.User.IsInRole(AuthorizationConst.NormalRoleName.Normal))
                    //                   && context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope &&
                    //                                                 c.Value == authServer.Scope);
                    //        }
                    //    ));

                    ////超管
                    //options.AddPolicy(AuthorizationConst.NormalPolicyName.SuperAdminPolicy,
                    //    policy => policy.RequireAssertion(context =>
                    //        {
                    //            return context.User.IsInRole(AuthorizationConst.NormalRoleName.SuperAdmin)
                    //                   && context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope &&
                    //                                                 c.Value == authServer.Scope);
                    //        }
                    //    ));

                    ////Manager
                    //options.AddPolicy(AuthorizationConst.NormalPolicyName.ManagerPolicy,
                    //    policy => policy.RequireAssertion(context =>
                    //        {
                    //            return (context.User.IsInRole(AuthorizationConst.NormalRoleName.SuperAdmin) ||
                    //                    context.User.IsInRole(AuthorizationConst.NormalRoleName.VodAdmin))
                    //                   && context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope &&
                    //                                                 c.Value == authServer.Scope);
                    //        }
                    //    ));

                    //未登录跨域
                    options.AddPolicy(AuthorizationConst.NormalPolicyName.CrossPolicy,
                        policy => policy.RequireAssertion(context =>
                            context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope &&
                                                       (c.Value == "kdy_cross_auth" || c.Value == authServer.Scope))
                        ));
                });
            }
            #endregion

            //初始化第三方组件
            services.InitIdGenerate(configuration)
                    .UseRedisCache(configuration)
                    .AddMemoryCache()
                    .AddMiniProfile();

            //自有host
            services.Configure<KdySelfHostOption>(configuration?.GetSection(KdyWebServiceConst.SelfHostKey));

            var allowHost = configuration.GetValue<string>("AllowedHosts");
            if (string.IsNullOrEmpty(allowHost))
            {
                allowHost = "https://*.wxkdy666.top,https://*.wxkdy666.com";
            }

            var hostArray = allowHost.Split(',').Select(a => a.TrimEnd('/')).ToArray();

            //跨域
            services.AddCors(option =>
                        option.AddPolicy("kdyCors", policy =>
                            policy.AllowAnyHeader()
                                .AllowAnyMethod()
                                .WithOrigins(hostArray)));

            AddRateLimit(services);
            return services;
        }

        /// <summary>
        /// 添加默认扩展
        /// </summary>
        /// <returns></returns>
        public static IApplicationBuilder AddKdyDefaultExt(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiniProfile();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIpRateLimiting();

            app.UseRouting();
            app.UseCors("kdyCors");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseKdyLog();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //全局DI容器
            KdyBaseServiceProvider.ServiceProvide = app.ApplicationServices;
            KdyBaseServiceProvider.HttpContextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>();
            return app;
        }

        /// <summary>
        /// 自定义Token校验
        /// </summary>
        /// <remarks>
        /// 如果是自带的授权 用户被删除后 Token一直有效 需要这里处理自定义校验
        /// </remarks>
        /// <returns></returns>
        private static async Task CustomAuthValidated(TokenValidatedContext tokenValidatedContext)
        {
            if (tokenValidatedContext.SecurityToken is JwtSecurityToken jwtToken)
            {
                // var userIdStr = jwtToken.Payload.Sub;
                var userService = tokenValidatedContext.HttpContext.RequestServices.GetRequiredService<IKdyUserTokenService>();
                var logout = await userService.UserTokenValidAsync(jwtToken.RawData);
                if (logout == false)
                {
                    //用户已注销
                    tokenValidatedContext.Fail("User token is logout");
                }

                //var isValid = await userService.UserIdIsValidAsync(userId);
                //if (isValid == false)
                //{
                //    //用户校验失败
                //    tokenValidatedContext.Fail($"User tot found");
                //    return;
                //}

                ////同账号只能在一台设备登录
                //var newToken = await userService.GetCurrentUserTokenAsync(userId);
                //if (string.IsNullOrEmpty(newToken) == false &&
                //    newToken != jwtToken.RawData)
                //{
                //    //用户已登录
                //    tokenValidatedContext.Fail($"User login another place");
                //}
            }
        }

        /// <summary>
        /// 添加限流
        /// </summary>
        private static void AddRateLimit(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            //todo:https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup
            // needed to load configuration from appsettings.json
            //services.AddOptions();
            // needed to store rate limit counters and ip rules
            // services.AddMemoryCache();

            //load general configuration from appsettings.json
            //EnableEndpointRateLimiting 为false时 全局过滤  为true时单个Api过滤
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            // services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            // services.AddRedisRateLimiting();
            services.AddDistributedRateLimiting<KdyCustomRedisProcessingStrategy>();

            //// Add framework services.
            //services.AddMvc();
            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // inject counter and rules distributed cache stores
            // services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
        }
    }
}
