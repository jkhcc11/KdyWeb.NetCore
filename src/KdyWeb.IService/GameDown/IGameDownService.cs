using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.GameDown;
using KdyWeb.Dto.HttpApi.Steam;

namespace KdyWeb.IService.GameDown
{
    /// <summary>
    /// 游戏下载资源 服务接口
    /// </summary>
    public interface IGameDownService : IKdyService
    {
        /// <summary>
        /// 查询游戏下载列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryGameDownListWithAdminDto>>> QueryGameDownListWithAdminAsync(QueryGameDownListWithAdminInput input);

        /// <summary>
        /// 根据下载Id获取磁力下载
        /// </summary>
        /// <param name="downId">下载Id</param>
        /// <returns></returns>
        Task<KdyResult<GetDownMagnetByDownIdDto>> GetDownMagnetByDownIdAsync(long downId);

        /// <summary>
        /// 根据Id和磁力更新下载信息
        /// </summary>
        /// <param name="downId">下载Id</param>
        /// <param name="magnetLink">磁力链接</param>
        /// <returns></returns>
        Task<KdyResult> SaveMagnetByTorrentUrlAsync(long downId, string magnetLink);

        /// <summary>
        /// 根据downId更新steam信息
        /// </summary>
        /// <param name="downId">下载Id</param>
        /// <param name="steamResponse">steam返回信息</param>
        /// <returns></returns>
        Task<KdyResult> SaveSteamInfoByDownIdAsync(long downId, GetGameInfoByStoreUrlResponse steamResponse);
    }
}
