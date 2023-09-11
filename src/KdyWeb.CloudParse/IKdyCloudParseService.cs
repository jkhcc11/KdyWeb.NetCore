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
    public interface IKdyCloudParseService
    {
        /// <summary>
        /// 根据关键字获取文件信息
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <returns>
        ///  返回全字匹配结果  eg: 张三.ab.mp4  必须是 张三.ab.mp4
        /// </returns>
        Task<KdyResult<BaseResultOut>> GetFileInfoByKeyWordAsync(string keyWord);

        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <remarks>
        /// 根据各网盘缓存情况删除
        /// </remarks>
        /// <returns></returns>
        Task<bool> ClearCacheAsync();

        /// <summary>
        /// 获取下载地址
        /// </summary>
        /// <param name="input">下载参数</param>
        Task<KdyResult<string>> GetDownUrlAsync<TDownEntity>(BaseDownInput<TDownEntity> input);
        
        /// <summary>
        /// 获取所有文件映射关系
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, bool>> GetAllFileInfoMapAsync();
    }

    /// <summary>
    /// 网盘解析接口
    /// </summary>
    /// <typeparam name="TOut">搜索结果返回</typeparam>
    /// <typeparam name="TInput">搜索输入</typeparam>
    public interface IKdyCloudParseService<TInput, TOut> : IKdyCloudParseService
        where TOut : IBaseResultOut
    {
        /// <summary>
        /// 查询接口
        /// </summary>
        Task<KdyResult<List<TOut>>> QueryFileAsync(BaseQueryInput<TInput> input);
    }
}
