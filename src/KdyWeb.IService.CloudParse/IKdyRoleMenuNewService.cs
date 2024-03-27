using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// 角色菜单 服务接口
    /// </summary>
    public interface IKdyRoleMenuNewService : IKdyService
    {
        /// <summary>
        /// 获取角色已激活菜单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetRoleActivateMenuAsync(string roleName);

        /// <summary>
        /// 创建和更新角色菜单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateRoleMenuAsync(CreateAndUpdateRoleMenuInput input);

        /// <summary>
        /// 获取角色已激活菜单树
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetVueMenuWithWorkVueDto>>> GetRoleActivateMenuTreeAsync(string[] roleNames);
    }
}
