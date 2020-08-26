using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.BaseInterface.HttpBase
{
    /// <summary>
    ///  Http请求 接口
    /// </summary>
    /// <typeparam name="TResult">Http返回泛型 必需继承IHttpResult TData</typeparam>
    /// <typeparam name="TData">具体结果泛型</typeparam>
    /// <typeparam name="TExt">Http请求泛型</typeparam>
    /// <typeparam name="TExtData">请求扩展</typeparam>
    public interface IKdyHttp<TResult, TData, TExt, TExtData>
        where TResult : class, IHttpOut<TData>, new()
        where TExt : class, IHttpRequestInput<TExtData>
    {
        /// <summary>
        /// 统一请求
        /// </summary>
        /// <returns></returns>
        Task<TResult> SendAsync(TExt input);

        /// <summary>
        /// 设置请求参数
        /// </summary>
        /// <returns></returns>
        HttpRequestMessage RequestPar(TExt input);
    }
}
