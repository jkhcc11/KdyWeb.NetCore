using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using System.Threading.Tasks;

namespace KdyWeb.IService.Resource
{
    /// <summary>
    /// 资源 服务接口
    /// </summary>
    public interface IResourceService: IKdyService
    {
        /// <summary>
        /// 获取全局资源
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetAllResourceDto>> GetAllResourceAsync();
    }
}
