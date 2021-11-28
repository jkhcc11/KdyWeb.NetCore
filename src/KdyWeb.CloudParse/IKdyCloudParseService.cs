using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.Out;

namespace KdyWeb.CloudParse
{
    /// <summary>
    /// 网盘解析接口
    /// </summary>
    /// <typeparam name="TOut">搜索结果返回</typeparam>
    /// <typeparam name="TInput">搜索输入</typeparam>
    /// <typeparam name="TDownEntity">下载参数</typeparam>
    public interface IKdyCloudParseService<TInput, TOut, TDownEntity>
        where TOut : IBaseResultOut
    {
        /// <summary>
        /// 查询接口
        /// </summary>
        Task<KdyResult<List<TOut>>> QueryFileAsync(BaseQueryInput<TInput> input);

        /// <summary>
        /// 获取下载地址
        /// </summary>
        /// <param name="input">下载参数</param>
        Task<KdyResult<string>> GetDownUrlAsync(BaseDownInput<TDownEntity> input);
    }
}
