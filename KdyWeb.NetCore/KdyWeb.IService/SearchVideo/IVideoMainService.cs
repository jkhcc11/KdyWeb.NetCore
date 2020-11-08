using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 影片主表 服务接口
    /// </summary>
    public interface IVideoMainService : IKdyService
    {
        /// <summary>
        /// 创建影片信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateVideoInfoAsync();
    }
}
