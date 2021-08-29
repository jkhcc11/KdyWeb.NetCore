using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Exceptionless.Json;
using KdyWeb.BaseInterface.BaseModel;
using Newtonsoft.Json;

namespace KdyWeb.Dto.KdyHttp
{
    /// <summary>
    /// 通用Http请求 输入参数
    /// </summary>
    public class KdyRequestCommonInput : IHttpRequestInput<KdyRequestCommonExtInput>
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="method">请求方式</param>
        public KdyRequestCommonInput(string url, HttpMethod method)
        {
            Url = url;
            Method = method;
            EnCoding = Encoding.UTF8;
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.81 Safari/537.36 SE 2.X MetaSr 1.0";
        }

        public int TimeOut { get; set; } = 10;

        public string Url { get; set; }

        public string Referer { get; set; }

        public string UserAgent { get; set; }

        public string Cookie { get; set; }

        public HttpMethod Method { get; set; }

        [JsonIgnore]
        public Encoding EnCoding { get; set; }

        public bool IsAutoRedirect { get; set; }

        public KdyRequestCommonExtInput ExtData { get; set; }

        public string GetString()
        {
            var str = JsonConvert.SerializeObject(this);
            if (str.Length > 1024)
            {
                return str.Substring(0, 1024);
            }

            return str;
        }
    }

    /// <summary>
    /// 通用Http请求扩展 输入参数
    /// </summary>
    public class KdyRequestCommonExtInput
    {
        /// <summary>
        /// 请求内容类型 默认 application/x-www-form-urlencoded
        /// </summary>
        /// <remarks>
        ///  标准格式：application/x-www-form-urlencoded <br/>
        /// application/json
        /// </remarks>
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";

        /// <summary>
        /// 是否Ajax请求
        /// </summary>
        public bool IsAjax { get; set; }

        /// <summary>
        /// Post数据
        /// </summary>
        public string PostData { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Post文件数据
        /// </summary>
        [JsonIgnore]
        public byte[] FileBytes { get; set; }

        /// <summary>
        /// 文件标签Name
        /// </summary>
        public string NameField { get; set; }

        /// <summary>
        /// 表单其他字段
        /// </summary>
        public Dictionary<string, string> PostParDic { get; set; }
    }
}
