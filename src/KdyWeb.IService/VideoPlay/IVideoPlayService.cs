using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;

namespace KdyWeb.IService
{
    /// <summary>
    /// 视频播放 服务接口
    /// </summary>
    public interface IVideoPlayService : IKdyService
    {
        /// <summary>
        /// 根据剧集Id获取视频信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetVideoInfoByEpIdDto>> GetVideoInfoByEpIdAsync(long epId);
    }
}
