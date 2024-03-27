using System.Collections.Generic;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KdyWeb.Dto.CloudParse.KdyUserNew;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    [CustomRoute("role-menu")]
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
    public class RoleMenuController : BaseApiController
    {
        private readonly IKdyMenuNewService _kdyMenuNewService;
        private readonly IKdyRoleMenuNewService _kdyRoleMenuNewService;
        private readonly IKdyRoleNewService _kdyRoleNewService;

        public RoleMenuController(IKdyMenuNewService kdyMenuNewService,
            IKdyRoleMenuNewService kdyRoleMenuNewService, IKdyRoleNewService kdyRoleNewService)
        {
            _kdyMenuNewService = kdyMenuNewService;
            _kdyRoleMenuNewService = kdyRoleMenuNewService;
            _kdyRoleNewService = kdyRoleNewService;
        }

        #region 菜单
        /// <summary>
        /// 创建和更新菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("menu/create-and-update")]
        public async Task<KdyResult> CreateAndUpdateMenuAsync(CreateAndUpdateMenuInput input)
        {
            var result = await _kdyMenuNewService.CreateAndUpdateMenuAsync(input);
            return result;
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("menu/query")]
        public async Task<KdyResult<PageList<QueryPageMenuDto>>> QueryPageRoleMenuAsync([FromQuery] QueryPageMenuInput input)
        {
            var result = await _kdyMenuNewService.QueryPageMenuAsync(input);
            return result;
        }

        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet("menu/get-all-menu-tree")]
        public async Task<KdyResult<List<GetAllMenuTreeDto>>> GetAllMenuTreeAsync()
        {
            var result = await _kdyMenuNewService.GetAllMenuTreeAsync();
            return result;
        }

        /// <summary>
        /// 获取所有一级菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("menu/get-all-one-menu")]
        public async Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetAllOneMenuAsync()
        {
            var result = await _kdyMenuNewService.GetAllOneMenuAsync();
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("menu/delete")]
        public async Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _kdyMenuNewService.BatchDeleteAsync(input);
            return result;
        }
        #endregion

        #region 角色权限
        /// <summary>
        /// 创建和更新角色菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("role-auth/create-and-update")]
        public async Task<KdyResult> CreateAndUpdateRoleMenuAsync(CreateAndUpdateRoleMenuInput input)
        {
            var result = await _kdyRoleMenuNewService.CreateAndUpdateRoleMenuAsync(input);
            return result;
        }

        /// <summary>
        /// 获取角色已激活菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("role-auth/{roleName}")]
        public async Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetRoleActivateMenuAsync(string roleName)
        {
            var result = await _kdyRoleMenuNewService.GetRoleActivateMenuAsync(roleName);
            return result;
        }
        #endregion

        #region 角色
        /// <summary>
        /// 创建和更新菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("role/create-and-update")]
        public async Task<KdyResult> CreateAndUpdateRoleAsync(CreateAndUpdateRoleInput input)
        {
            var result = await _kdyRoleNewService.CreateAndUpdateRoleAsync(input);
            return result;
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("role/query")]
        public async Task<KdyResult<PageList<QueryPageRoleDto>>> QueryPageRoleAsync([FromQuery] QueryPageRoleInput input)
        {
            var result = await _kdyRoleNewService.QueryPageRoleAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("role/delete/{roleId}")]
        public async Task<KdyResult> DeleteRoleAsync(long roleId)
        {
            var result = await _kdyRoleNewService.DeleteAsync(roleId);
            return result;
        }
        #endregion
    }
}
