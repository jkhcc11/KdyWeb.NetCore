using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 影片下载地址 服务接口
    /// </summary>
    public interface IVideoDownInfoService : IKdyService
    {
        /// <summary>
        /// 查询下载地址
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryVideoDownInfoDto>>> QueryVideoDownInfoAsync(QueryVideoDownInfoInput input);
    }
}
