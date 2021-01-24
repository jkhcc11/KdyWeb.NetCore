using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        protected readonly IKdyLog KdyLog;
        /// <summary>
        /// 统一配置
        /// </summary>
        protected readonly IConfiguration KdyConfiguration;

        protected readonly IHttpClientFactory HttpClientFactory;
        protected BaseKdyHttpClient(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
            KdyLog = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IKdyLog>();
            KdyConfiguration = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IConfiguration>();
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
            //KdyLog.Info("Http请求开始", new Dictionary<string, object>()
            //{
            //    {"HttpInput",input}
            //});

            //todo:待验证cookie问题 需要改造
            //这里的name必须和注入时保持一致时 注入的配置才生效
            var httpClient = HttpClientFactory.CreateClient(KdyBaseConst.HttpClientName);
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
                request.Headers.Add("User-Agent", input.UserAgent);
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
                    result.IsSuccess = false;
                    return result;
                }

                var resultBytes = await response.Content.ReadAsByteArrayAsync();
                var resultStr = CharsetHandler(input, resultBytes);

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
                ex.ToExceptionless().Submit();
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrMsg = $"Http程序异常：{ex.Message}";
                ex.ToExceptionless().Submit();
                return result;
            }
            finally
            {
                watch.Stop();
                KdyLog.Info($"Http请求结束,Url:{input.Url},耗时：{watch.ElapsedMilliseconds}ms", new Dictionary<string, object>()
                {
                    {"HttpResult",result},
                    {"HttpInput",input}
                });
            }
        }

        /// <summary>
        /// 网页编码处理
        /// </summary>
        protected string CharsetHandler(TInput input, byte[] repBytes)
        {
            string tempStr = Encoding.Default.GetString(repBytes);

            if (tempStr.StartsWith("{") || tempStr.StartsWith("["))
            {
                if (input.EnCoding == null)
                {
                    input.EnCoding = Encoding.UTF8;
                }

                //json格式 使用请求编码返回 默认utf8
                return input.EnCoding.GetString(repBytes);
            }

            //todo:无法识别 <meta charset=gb2312 /> xml格式
            var meta = Regex.Match(tempStr, "<meta[^<]*charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
            string charset = string.Empty;
            if (meta.Groups.Count > 0)
            {
                charset = meta.Groups[1].Value.ToLower().Trim();
            }
            if (charset.Length > 2)
            {
                try
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    //todo:System.Text.Encoding.CodePages 需要引入这个 否则无法解析gbk
                    input.EnCoding = Encoding.GetEncoding(charset.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                }
                catch
                {
                    //异常设置且没有设置
                    if (input.EnCoding == null)
                    {
                        input.EnCoding = Encoding.Default;
                    }
                }
            }
            else if (input.EnCoding == null)
            {
                input.EnCoding = Encoding.Default;
            }

            return input.EnCoding.GetString(repBytes);
        }

        public void Dispose()
        {
        }
    }
}
