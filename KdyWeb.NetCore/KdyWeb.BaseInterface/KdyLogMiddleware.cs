﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.BaseInterface.KdyRedis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 日志中间件 扩展
    /// </summary>
    public static class KdyLogMiddlewareExt
    {
        /// <summary>
        /// 使用日志中间件
        /// </summary>
        /// <returns></returns>
        public static void UseKdyLog(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<KdyLogMiddleware>();
        }
    }

    /// <summary>
    /// 日志中间件
    /// </summary>
    public class KdyLogMiddleware
    {
        private readonly RequestDelegate _next;
        public KdyLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            if (request == null)
            {
                return;
            }

            var kdyLog = (IKdyLog)context.RequestServices.GetService(typeof(IKdyLog));
            if (kdyLog == null)
            {
                await _next(context);
                return;
            }

            //通过EndPoint获取Controller特性
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var source = endpoint?.DisplayName;
            if (string.IsNullOrEmpty(source))
            {
                source = request.Path;
            }

            try
            {
                //原始数据流
                var oldBody = context.Response.Body;
                //清空流
                var ms = new MemoryStream();
                context.Response.Body = ms;

                //执行其他
                await _next(context);

                //重置
                ms.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(ms);
                var str = reader.ReadToEnd();

                //记录日志
                kdyLog.Info(source, $"{str}", context, request.Path);

                ms.Seek(0, SeekOrigin.Begin);
                // 写入到原有的流中
                await ms.CopyToAsync(oldBody);
            }
            catch
            {
                throw;
            }

        }

    }
}
