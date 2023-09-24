using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        //private readonly IHostingEnvironment _environment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<KdyLogMiddleware> _logger;

        public KdyLogMiddleware(RequestDelegate next, ILogger<KdyLogMiddleware> logger, 
            IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _stopwatch = new Stopwatch();

        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            // var response = context.Response;
            if (request == null)
            {
                return;
            }

            //var kdyLog = (IKdyLog)context.RequestServices.GetService(typeof(IKdyLog));
            //if (kdyLog == null)
            //{
            //    await _next(context);
            //    return;
            //}
            var includeApiPrefix = new[] { "/api/", "/api-v2/", "/self-api-v2/" };

            if (includeApiPrefix.Any(request.Path.Value.StartsWith) == false)
            {
                //非api不用记录
                await _next(context);
                return;
            }

            //跳过记录 通过EndPoint获取Controller特性
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var anonymous = endpoint?.Metadata?.GetMetadata<DisableKdyLogAttribute>();
            if (anonymous != null)
            {
                await _next(context);
                return;
            }

            _stopwatch.Restart();
            var currentFlag = context.TraceIdentifier;

            var data = new ConcurrentDictionary<string, object>();
            data.TryAdd("request.url", request.Path.ToString());
            data.TryAdd("request.method", request.Method);
            data.TryAdd("request.executeStartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if (request.Headers.ContainsKey("Authorization"))
            {
                data.TryAdd("request.token", request.Headers["Authorization"]);
            }

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

            try
            {
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
                if (request.Method.ToLower() != "get")
                {
                    _logger.LogInformation("用户请求{url}结束.Flag:{flag},时间：{time}ms,扩展:{exData}",
                        request.Path.Value,
                        currentFlag,
                        _stopwatch.ElapsedMilliseconds,
                        JsonConvert.SerializeObject(data));
                    _logger.LogInformation("Flag:{flag},扩展:{exData}",
                        currentFlag,
                        JsonConvert.SerializeObject(data));
                }
                else
                {
                    //记录日志
                    _logger.LogTrace("用户请求{url}结束.Flag:{flag},时间：{time}ms,扩展:{exData}",
                        request.Path.Value,
                        currentFlag,
                        _stopwatch.ElapsedMilliseconds,
                        JsonConvert.SerializeObject(data));
                }
            }
            catch (Exception ex) when (ex is KdyCustomException customException)
            {
                var errResult = KdyResult.Error(KdyResultCode.Error, customException.Message);
                var str = JsonConvert.SerializeObject(errResult, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var bytes = Encoding.UTF8.GetBytes(str);
                var newStream = new MemoryStream(bytes);

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/json;charset=utf-8;";
                await newStream.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                var errResult = KdyResult.Error(KdyResultCode.SystemError, "系统错误，请稍后再试");
                if (_webHostEnvironment.IsDevelopment())
                {
                    errResult.Msg = ex.Message;
                }

                _logger.LogError(ex, "系统异常：{ex.Message}", ex.Message);
                var str = JsonConvert.SerializeObject(errResult, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var bytes = Encoding.UTF8.GetBytes(str);
                var newStream = new MemoryStream(bytes);

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/json;charset=utf-8;";
                await newStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}
