using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.KdyLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
        private readonly Stopwatch _stopwatch;
        public KdyLogMiddleware(RequestDelegate next)
        {
            _next = next;
            _stopwatch = new Stopwatch();

        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
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

            if (request.Path.Value.Contains("/api/") == false)
            {
                //非api不用记录
                await _next(context);
                return;
            }

            _stopwatch.Restart();

            var data = new ConcurrentDictionary<string, object>();
            data.TryAdd("request.url", request.Path.ToString());
            data.TryAdd("request.method", request.Method);
            data.TryAdd("request.executeStartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            if (request.Method.ToLower() == "get")
            {
                data.TryAdd("request.body", request.QueryString.Value);
            }
            else
            {
                request.EnableBuffering();
                var sr = new StreamReader(request.Body);
                var bodyStr = await sr.ReadToEndAsync();
                data.TryAdd("request.body", bodyStr);

                //重置
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            // 获取Response.Body内容
            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            //执行其他
            await _next(context);

            //重置
            responseBody.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(responseBody);
            var str = await reader.ReadToEndAsync();

            //todo:如果这里有请求慢的时候 两个请求会在一起 导致add字典异常
            data.TryAdd("response.body", str);
            data.TryAdd("response.executeEndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);

            _stopwatch.Stop();
            data.TryAdd("time", _stopwatch.ElapsedMilliseconds + "ms");
            //记录日志
            kdyLog.Info($"用户请求{request.Path.Value}结束", data.ToDictionary(a => a.Key, a => a.Value));

        }
    }
}
