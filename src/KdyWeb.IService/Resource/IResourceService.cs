using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using System.Threading.Tasks;
using KdyWeb.Dto.Resource;

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

        /// <summary>
        /// 创建|更新资源
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateResourceAsync(CreateAndUpdateResourceInput input);

        /// <summary>
        /// 查询资源列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryResourceDto>>> QueryResourceAsync(QueryResourceInput input);

        /// <summary>
        /// 启用资源
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> OpenResourceAsync(long configId);

        /// <summary>
        /// 禁用资源
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BanResourceAsync(long configId);

    }
}
