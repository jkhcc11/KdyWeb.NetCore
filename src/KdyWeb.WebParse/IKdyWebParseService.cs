using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.WebParse
{
    /// <summary>
    /// 自用站点解析 服务接口
    /// </summary>
    public interface IKdyWebParseService<TParseInput, TParseOut>
        where TParseInput : IKdyWebParseInput
        where TParseOut : IKdyWebParseOut
    {
        /// <summary>
        /// 获取解析结果
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<TParseOut>> GetResultAsync(TParseInput input);
    }
}
