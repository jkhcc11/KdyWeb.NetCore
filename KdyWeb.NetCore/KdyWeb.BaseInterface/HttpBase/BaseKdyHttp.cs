using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using Newtonsoft.Json;

namespace KdyWeb.BaseInterface.HttpBase
{
    /// <summary>
    /// Http请求 抽象类
    /// </summary>
    public abstract class BaseKdyHttp<TResult, TData, TInput, TExtData> : BaseKdyService
        where TResult : class, IHttpOut<TData>, new()
        where TData : class
        where TInput : class, IHttpRequestInput<TExtData>
    {
        protected readonly IHttpClientFactory HttpClientFactory;
        protected BaseKdyHttp(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
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
            var httpClient = HttpClientFactory.CreateClient();
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

        ///// <summary>
        ///// 异步Post
        ///// </summary>
        ///// <returns></returns>
        //protected virtual async Task<TResult> PostAsync(string url, string postData)
        //{
        //    var httpClient = HttpClientFactory.CreateClient("DemoHttp");

        //    //组装post内容
        //    var postContent = new StringContent(postData);
        //    postContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        //    //头部信息
        //    var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = postContent };
        //    request.Headers.Add("Referer", url);
        //    var result = new TResult() { IsSuccess = true };

        //    await GetResult(httpClient, request, result);
        //    return result;
        //}

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
            try
            {
                var response = await httpClient.SendAsync(request);

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

    }
}
