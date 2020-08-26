using System.Net.Http;
using System.Text;
using KdyWeb.BaseInterface.BaseModel;

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

        public string Url { get; set; }

        public string Referer { get; set; }
        public string UserAgent { get; set; }

        public string Cookie { get; set; }

        public HttpMethod Method { get; set; }

        public Encoding EnCoding { get; set; }

        public KdyRequestCommonExtInput ExtData { get; set; }
    }

    /// <summary>
    /// 通用Http请求扩展 输入参数
    /// </summary>
    public class KdyRequestCommonExtInput
    {
        /// <summary>
        /// 请求内容类型 默认 application/x-www-form-urlencoded
        /// </summary>
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";

        /// <summary>
        /// Post数据
        /// </summary>
        public string PostData { get; set; }

        /// <summary>
        /// 是否Ajax请求
        /// </summary>
        public bool IsAjax { get; set; }
    }
}
