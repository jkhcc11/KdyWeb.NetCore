using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.PageParse
{
    /// <summary>
    /// 站点页面解析 服务接口
    /// </summary>
    /// <typeparam name="TSearchInput">搜索入参泛型</typeparam>
    /// <typeparam name="TSearchOut">搜索出参泛型</typeparam>
    /// <typeparam name="TSearchItem">搜索出参Item泛型</typeparam>
    /// <typeparam name="TPageInput">页面解析入参</typeparam>
    /// <typeparam name="TPageOut">页面解析出参</typeparam>
    public interface IKdyWebPageParseService<TSearchInput, TSearchOut, TSearchItem, TPageInput, TPageOut>
        where TSearchInput : IKdyWebPageSearchInput
        where TSearchOut : IKdyWebPageSearchOut<TSearchItem>
        where TPageInput : IKdyWebPagePageInput
        where TPageOut : IKdyWebPagePageOut
        where TSearchItem : IKdyWebPageSearchOutItem
    {
        /// <summary>
        /// 获取搜索结果
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<TSearchOut>> GetSearchResultAsync(TSearchInput input);

        /// <summary>
        /// 获取页面解析结果
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<TPageOut>>> GetPageResultAsync(TSearchItem searchOut, TPageInput input);
    }
}
