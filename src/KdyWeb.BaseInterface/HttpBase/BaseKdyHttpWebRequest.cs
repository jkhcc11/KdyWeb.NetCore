using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using Newtonsoft.Json;

namespace KdyWeb.BaseInterface.HttpBase
{
    /// <summary>
    /// 基于WebRequest Http请求 抽象类
    /// todo:获取跨域cookie方便
    /// </summary>
    public abstract class BaseKdyHttpWebRequest<TResult, TData, TInput, TExtData> : BaseKdyService
        where TResult : class, IHttpOut<TData>, new()
        where TData : class
        where TInput : class, IHttpRequestInput<TExtData>
    {
        protected BaseKdyHttpWebRequest(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 设置请求内容
        /// </summary>
        protected abstract void SetRequest(HttpWebRequest request, TInput input);

        /// <summary>
        /// 异步请求
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TResult> SendAsync(TInput input)
        {
            KdyLog.Info("Http请求开始", new Dictionary<string, object>()
            {
                {"HttpInput",input}
            });

            var request = (HttpWebRequest)WebRequest.Create(input.Url);
            SetRequest(request, input);
            if (string.IsNullOrEmpty(input.Cookie) == false)
            {
                request.Headers.Add("Cookie", input.Cookie);
            }

            if (string.IsNullOrEmpty(input.Referer) == false)
            {
                request.Referer = input.Referer;
            }

            if (string.IsNullOrEmpty(input.UserAgent) == false)
            {
                request.UserAgent = input.UserAgent;
            }

            request.AllowAutoRedirect = input.IsAutoRedirect;

            var result = new TResult()
            {
                IsSuccess = true
            };
            await GetResult(request, result, input);

            KdyLog.Info("Http请求结束", new Dictionary<string, object>()
            {
                {"HttpResult",result}
            });
            return result;
        }

        /// <summary>
        /// Http结果格式化
        /// </summary>
        /// <param name="request"></param>
        /// <param name="result">结果实例</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        protected virtual async Task<TResult> GetResult(HttpWebRequest request, TResult result, TInput input)
        {
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException ex)
            {
                //处理302 400 403 等非正常
                response = (HttpWebResponse)ex.Response;
            }

            try
            {

                result.Cookie = response.Headers["Set-Cookie"];
                result.LocationUrl = response.Headers["location"];
                CookieHandler(result);

                result.HttpCode = response.StatusCode;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.IsSuccess = false;
                    return result;
                }

                byte[] repByte;
                using (var stream = new MemoryStream())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream == null)
                        {
                            result.IsSuccess = false;
                            return result;
                        }

                        //GZIIP处理  
                        if (string.IsNullOrEmpty(response.ContentEncoding) == false &&
                            response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //开始读取流并设置编码方式  
                            new GZipStream(responseStream, CompressionMode.Decompress).CopyTo(stream);
                        }
                        else
                        {
                            //开始读取流并设置编码方式  
                            responseStream.CopyTo(stream);
                        }
                    }

                    repByte = stream.ToArray();
                }

                var resultStr = CharsetHandler(input, repByte);

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
                request.Abort();
                response?.Close();
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

        /// <summary>
        /// Cookie处理
        /// </summary>
        protected void CookieHandler(TResult input)
        {
            if (string.IsNullOrEmpty(input.Cookie))
            {
                return;
            }

            var sb = new StringBuilder();
            var tempArray = input.Cookie.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in tempArray)
            {
                if (item.Contains("=") == false)
                {
                    continue;
                }

                var eqIndex = item.IndexOf("=", StringComparison.Ordinal);
                if (eqIndex < 1)
                {
                    continue;
                }

                //排除不要的
                string name = item.Substring(0, eqIndex).Trim(),
                    value = item.Substring(eqIndex + 1).Trim();
                switch (name.ToLower())
                {
                    case "domain":
                    case "path":
                    case "max-age":
                    case "expires":
                        {
                            break;
                        }
                    default:
                        {
                            sb.Append($"{name}={value};");
                            if (input.CookieDic.ContainsKey(name) == false)
                            {
                                input.CookieDic.Add(name, value);
                            }
                            break;
                        }
                }
            }

            input.Cookie = sb.ToString();
        }


    }
}
