using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 视频播放记录 服务接口
    /// </summary>
    public interface IVideoHistoryService : IKdyService
    {
        /// <summary>
        /// 创建视频播放记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateVideoHistoryAsync(CreateVideoHistoryInput input);

        /// <summary>
        /// 查询视频播放记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryVideoHistoryDto>>> QueryVideoHistoryAsync(QueryVideoHistoryInput input);
    }
}
