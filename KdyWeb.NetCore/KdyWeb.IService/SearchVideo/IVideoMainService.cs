using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 影片主表 服务接口
    /// </summary>
    public interface IVideoMainService : IKdyService
    {
        /// <summary>
        /// 通过豆瓣信息创建影片信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateForDouBanInfoAsync(CreateForDouBanInfoInput input);

        /// <summary>
        /// 获取影片信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetVideoDetailDto>> GetVideoDetailAsync(long keyId);
    }
}
