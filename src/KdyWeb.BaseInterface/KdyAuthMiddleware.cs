using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.KdyRedis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Distributed;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 授权中间件配置
    /// </summary>
    public class KdyAuthMiddlewareOption
    {
        /// <summary>
        /// 登录页面
        /// </summary>
        public string LoginUrl { get; set; }
    }

    /// <summary>
    /// 授权中间件 扩展
    /// </summary>
    public static class KdyAuthMiddlewareExt
    {
        /// <summary>
        /// 使用Kdy授权中间件 使用Cookie验证
        /// </summary>
        /// <returns></returns>
        public static IApplicationBuilder UseKdyAuth(this IApplicationBuilder builder, KdyAuthMiddlewareOption option)
        {
            //todo:迁移完改成JWT统一
            builder.UseMiddleware<KdyAuthMiddleware>(option);
            return builder;
        }
    }

    /// <summary>
    /// 授权中间件
    /// </summary>
    public class KdyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly KdyAuthMiddlewareOption _option;
        public KdyAuthMiddleware(RequestDelegate next, KdyAuthMiddlewareOption option)
        {
            _next = next;
            _option = option;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            if (request == null)
            {
                return;
            }

            //通过EndPoint获取Controller特性
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            //跳过验证
            var anonymous = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>();
            if (anonymous != null)
            {
                await _next(context);
                return;
            }

            if (string.IsNullOrEmpty(_option.LoginUrl))
            {
                _option.LoginUrl = "/User/Login";
            }

            if (request.Cookies.ContainsKey(KdyBaseConst.CookieKey) == false)
            {
                //未登录
                context.Response.Redirect(_option.LoginUrl);
                return;
            }

            var cookie = request.Cookies[KdyBaseConst.CookieKey];
            if (string.IsNullOrEmpty(cookie))
            {
                context.Response.Redirect(_option.LoginUrl);
                return;
            }

            var kdyRedis = (IKdyRedisCache)context.RequestServices.GetService(typeof(IKdyRedisCache));
            if (kdyRedis == null)
            {
                throw new Exception($"{nameof(KdyAuthMiddleware)} IKdyRedisCache为空");
            }

            //todo:兼容旧版 
            var value = (string)await kdyRedis.GetDb(1).StringGetAsync(cookie);
            if (string.IsNullOrEmpty(value))
            {
                context.Response.Redirect(_option.LoginUrl);
                return;
            }

            await _next(context);
        }
    }
}
