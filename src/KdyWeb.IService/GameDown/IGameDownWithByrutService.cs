using System.Threading.Tasks;

namespace KdyWeb.IService.GameDown
{
    /// <summary>
    /// Byrut下载相关
    /// </summary>
    public interface IGameDownWithByrutService
    {
        /// <summary>
        /// 根据详情创建下载信息
        /// </summary>
        /// <returns></returns>
        Task CreateDownInfoByDetailUrlAsync(string detailUrl, string userAgent, string cookie);

        /// <summary>
        /// 查询分页信息
        /// </summary>
        /// <remarks>
        /// 根据分页获取详情Url
        /// </remarks>
        /// <returns></returns>
        Task QueryPageInfoAsync(int page, string userAgent, string cookie);

        /// <summary>
        /// 根据最大分页查询所有
        /// </summary>
        /// <remarks>
        /// 初始化所有分页从1开始，到达最大页数每个创建一个任务
        /// </remarks>
        /// <returns></returns>
        Task QueryAllInfoAsync(int maxPage, string userAgent, string cookie);
    }
}
