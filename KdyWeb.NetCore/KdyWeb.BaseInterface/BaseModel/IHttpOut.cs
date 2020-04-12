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
        bool IsSuccess { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        string Cookie { get; set; }

        /// <summary>
        /// Http状态码
        /// </summary>
        HttpStatusCode HttpCode { set; get; }

        /// <summary>
        /// Http请求结果
        /// </summary>
        TData Data { get; set; }
    }
}
