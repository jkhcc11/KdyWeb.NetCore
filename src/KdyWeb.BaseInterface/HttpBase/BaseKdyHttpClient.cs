using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KdyWeb.BaseInterface.HttpBase
{
    /// <summary>
    /// 基于HttpClient Http请求 抽象类
    /// todo:获取跨域cookie麻烦
    /// </summary>
    public abstract class BaseKdyHttpClient<TResult, TData, TInput, TExtData> : IKdyService
        where TResult : class, IHttpOut<TData>, new()
        where TData : class
        where TInput : class, IHttpRequestInput<TExtData>
    {
        /// <summary>
        /// 统一日志
        /// </summary>
        protected readonly ILogger KdyLog;
        /// <summary>
        /// 统一配置
        /// </summary>
        protected readonly IConfiguration KdyConfiguration;

        protected readonly IHttpClientFactory HttpClientFactory;

        private readonly IHttpContextAccessor _httpContextAccessor;
        protected BaseKdyHttpClient(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
            KdyLog = KdyBaseServiceProvider.ServiceProvide.GetService<ILoggerFactory>().CreateLogger(GetType());
            KdyConfiguration = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IConfiguration>();
            _httpContextAccessor = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IHttpContextAccessor>();
        }

        /// <summary>
        /// 设置请求参数
        /// </summary>
        /// <returns></returns>
        public abstract HttpRequestMessage RequestPar(TInput input);

        /// <summary>
        /// 异步请求
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TResult> SendAsync(TInput input)
        {
            //这里的name必须和注入时保持一致时 注入的配置才生效
            var httpClient = HttpClientFactory.CreateClient(KdyBaseConst.HttpClientName);
            if (string.IsNullOrEmpty(input.BaseHost) == false)
            {
                if (input.Url.StartsWith(input.BaseHost))
                {
                    throw new KdyCustomException($"{nameof(input.BaseHost)}有值时，{nameof(input.Url)}不能是觉得路径");
                }

                httpClient.BaseAddress = new Uri(input.BaseHost);
            }

            httpClient.Timeout = TimeSpan.FromSeconds(input.TimeOut);

            var request = RequestPar(input);
            if (string.IsNullOrEmpty(input.Cookie) == false)
            {
                request.Headers.Add("Cookie", input.Cookie);
            }

            if (string.IsNullOrEmpty(input.Referer) == false)
            {
                request.Headers.Add("Referer", input.Referer);
            }

            if (string.IsNullOrEmpty(input.UserAgent) == false)
            {
                //不用校验UserAgent,有些特殊的  可能无法识别
                request.Headers.TryAddWithoutValidation("User-Agent", input.UserAgent);
                //request.Headers.Add("User-Agent", input.UserAgent);
            }

            var result = new TResult() { IsSuccess = true };
            await GetResult(httpClient, request, result, input);
            return result;
        }

        /// <summary>
        /// Http结果格式化
        /// </summary>
        /// <param name="httpClient">HttpClient 实例</param>
        /// <param name="request">HttpRequestMessage</param>
        /// <param name="result">结果实例</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        protected virtual async Task<TResult> GetResult(HttpClient httpClient, HttpRequestMessage request, TResult result, TInput input)
        {
            var currentFlag = _httpContextAccessor.HttpContext?.TraceIdentifier;
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                var response = await httpClient.SendAsync(request);

                if (response.Headers.Contains("location"))
                {
                    result.LocationUrl = response.Headers.GetValues("location").First();
                }

                if (response.Headers.Contains("Set-Cookie"))
                {
                    #region cookie处理

                    var sb = new StringBuilder();
                    var tempArray = response.Headers.GetValues("Set-Cookie").ToList();
                    if (tempArray.Any())
                    {
                        foreach (var item in tempArray)
                        {
                            //用;分隔取第一个
                            var temp = item.Split(';').First();
                            sb.Append($"{temp.Trim()};");
                            if (temp.Contains("=") == false)
                            {
                                continue;
                            }

                            var tempA = temp.Split('=');
                            string key = tempA[0].Trim(),
                                value = tempA[1].Trim();
                            if (result.CookieDic.ContainsKey(key))
                            {
                                //有些站点还有重复得。。。
                                continue;
                            }

                            result.CookieDic.Add(key, value);

                        }
                    }

                    result.Cookie = sb.ToString();

                    #endregion
                }

                result.HttpCode = response.StatusCode;
                if (response.IsSuccessStatusCode == false)
                {
                    KdyLog.LogWarning("Http请求异常,{flag},原始返回：{msg}",
                        currentFlag,
                        await response.Content.ReadAsStringAsync());
                    result.IsSuccess = false;
                    return result;
                }

                string resultStr;
                if (input.EnCoding != null)
                {
                    var resultBytes = await response.Content.ReadAsByteArrayAsync();
                    resultStr = input.EnCoding.GetString(resultBytes);
                }
                else
                {
                    resultStr = await response.Content.ReadAsStringAsync();
                }

                if (typeof(TData) == typeof(string))
                {
                    //String类型
                    result.Data = resultStr as TData;
                }
                else
                {
                    //其他类型
                    result.Data = JsonConvert.DeserializeObject<TData>(resultStr);
                }

                return result;
            }
            catch (WebException ex)
            {
                result.IsSuccess = false;
                result.ErrMsg = $"Http网站异常：{ex.Message}";
                //ex.ToExceptionless().Submit();
                KdyLog.LogError(ex, "Http网站异常.Flag:{flag}.信息:{msg}", currentFlag, ex.Message);
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrMsg = $"Http程序异常：{ex.Message}";
                // ex.ToExceptionless().Submit();
                KdyLog.LogError(ex, "Http程序异常.Flag:{flag}.信息:{msg}",
                    currentFlag, ex.Message);
                return result;
            }
            finally
            {
                watch.Stop();
                KdyLog.LogTrace("Http请求结束01.耗时:{time}ms,Flag:{flag},入参:{input}",
                    watch.ElapsedMilliseconds
                    , currentFlag
                    , JsonConvert.SerializeObject(input));
                KdyLog.LogTrace("Http请求结束02.Flag:{flag},返回:{result}",
                   currentFlag,
                   JsonConvert.SerializeObject(result));

                //KdyLog.Info($"Http请求结束,Url:{input.Url},耗时：{watch.ElapsedMilliseconds}ms", new Dictionary<string, object>()
                //{
                //    {"HttpResult",result},
                //    {"HttpInput",input}
                //});
            }
        }

        public void Dispose()
        {
        }
    }
}
