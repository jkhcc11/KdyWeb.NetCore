using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.DouBan;

namespace KdyWeb.IService.HttpApi
{
    /// <summary>
    /// 豆瓣 Api 
    /// </summary>
    public interface IDouBanHttpApi : IKdyService
    {
        /// <summary>
        /// 搜索提示
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        Task<KdyResult<List<SearchSuggestResponse>>> SearchSuggestAsync(string keyWord);

        /// <summary>
        /// 关键字搜索
        /// </summary>
        /// <remarks>
        /// 这个没有年份
        /// </remarks>
        /// <param name="keyWord">关键字</param>
        /// <param name="page">分页</param>
        /// <returns></returns>
        Task<KdyResult<List<SearchSuggestResponse>>> KeyWordSearchAsync(string keyWord, int page);
    }
}
