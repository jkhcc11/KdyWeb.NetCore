using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 视频弹幕 服务接口
    /// </summary>
    public interface IVideoDanMuService : IKdyService
    {
        /// <summary>
        /// 创建弹幕
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateDanMuAsync(CreateDanMuInput input);

        /// <summary>
        /// 获取视频剧集弹幕
        /// </summary>
        /// <param name="epId">剧集Id</param>
        /// <returns></returns>
        Task<KdyResult<string>> GetVideoDanMuAsync(long epId);

        /// <summary>
        /// 搜索弹幕
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<SearchDanMuDto>>> SearchDanMuAsync(SearchDanMuInput input);

        /// <summary>
        /// 批量删除弹幕
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input);
    }
}
