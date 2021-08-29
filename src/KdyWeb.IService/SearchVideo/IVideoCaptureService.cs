using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 影片采集 服务接口
    /// </summary>
    public interface IVideoCaptureService : IKdyService
    {
        /// <summary>
        /// 根据影片源详情创建影片 
        /// </summary>
        /// <remarks>
        ///  1、根据源详情获取影片信息 <br/>
        ///  2、根据名称如果存在则跳过
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> CreateVideoInfoByDetailAsync(CreateVideoInfoByDetailInput input);

        /// <summary>
        /// 根据影片源详情创建下载地址 
        /// </summary>
        /// <remarks>
        ///  1、根据源详情获取影片信息 <br/>
        ///  2、根据名称如果存在则跳过
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> CreateVideoDownByDetailAsync(CreateVideoInfoByDetailInput input);
    }
}
