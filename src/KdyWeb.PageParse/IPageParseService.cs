using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.PageParse
{
    /// <summary>
    /// 页面解析 服务接口
    /// </summary>
    /// <typeparam name="TPageParseOut">页面解析出参 泛型</typeparam>
    /// <typeparam name="TPageParseInput">页面解析入参 泛型</typeparam>
    public interface IPageParseService<TPageParseOut, TPageParseInput>
        where TPageParseOut : IPageParseOut
        where TPageParseInput : IPageParseInput
    {
        /// <summary>
        /// 获取网页解析结果
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<TPageParseOut>> GetResultAsync(TPageParseInput input);
    }
}
