using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// 菜单 服务接口
    /// </summary>
    public interface IKdyMenuNewService : IKdyService
    {
        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryPageMenuDto>>> QueryPageMenuAsync(QueryPageMenuInput input);

        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetAllMenuTreeDto>>> GetAllMenuTreeAsync();

        /// <summary>
        /// 创建和更新菜单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateMenuAsync(CreateAndUpdateMenuInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchDeleteForLongKeyInput input);

        /// <summary>
        /// 获取所有一级菜单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetAllOneMenuAsync();
    }
}
