using System.Net.Http;
using System.Text;

namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// Http请求参数 接口
    /// </summary>
    public interface IHttpRequestInput<TExt>
    {
        /// <summary>
        /// 请求Url
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        string Referer { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        string UserAgent { set; get; }

        /// <summary>
        /// Cookie
        /// </summary>
        string Cookie { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        HttpMethod Method { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        Encoding EnCoding { get; set; }

        /// <summary>
        /// 是否自动302跳转
        /// </summary>
        bool IsAutoRedirect { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        TExt ExtData { set; get; }

        /// <summary>
        /// 格式化入参
        /// </summary>
        /// <returns></returns>
        string GetString();
    }
}
