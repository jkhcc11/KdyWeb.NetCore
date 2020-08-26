using System.Net;

namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// Http请求结果
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IHttpOut<TData>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns>
        ///   一个指示 HTTP 响应是否成功的值。
        ///    如果 <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> 在 200-299 范围内，则为 <see langword="true" />；否则为 <see langword="false" />。
        /// </returns>
        bool IsSuccess { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        string Cookie { get; set; }

        /// <summary>
        /// Http状态码
        /// </summary>
        HttpStatusCode? HttpCode { set; get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        string ErrMsg { get; set; }

        /// <summary>
        /// Http请求结果
        /// </summary>
        TData Data { get; set; }
    }
}
