using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse.KdyUserNew;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// 角色 服务接口
    /// </summary>
    public interface IKdyRoleNewService : IKdyService
    {
        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryPageRoleDto>>> QueryPageRoleAsync(QueryPageRoleInput input);

        /// <summary>
        /// 创建和更新角色
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateRoleAsync(CreateAndUpdateRoleInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(long roleId);
    }
}
