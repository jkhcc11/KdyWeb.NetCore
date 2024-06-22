using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.Resource;
using KdyWeb.IService.Resource;
using KdyWeb.BaseInterface;
using Microsoft.AspNetCore.Authorization;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 资源接口
    /// </summary>
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
    public class ResourceController : BaseManagerController
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        /// <summary>
        /// 查询资源列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<PageList<QueryResourceDto>>> QueryResourceAsync([FromQuery] QueryResourceInput input)
        {
            return await _resourceService.QueryResourceAsync(input);
        }

        /// <summary>
        /// 创建|更新 资源
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-and-update")]
        public async Task<KdyResult> CreateAndUpdateResourceAsync(CreateAndUpdateResourceInput input)
        {
            return await _resourceService.CreateAndUpdateResourceAsync(input);
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <returns></returns>
        [HttpPatch("open/{id}")]
        public async Task<KdyResult> OpenResourceAsync(long id)
        {
            return await _resourceService.OpenResourceAsync(id);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        [HttpPatch("ban/{id}")]
        public async Task<KdyResult> BanResourceAsync(long id)
        {
            return await _resourceService.BanResourceAsync(id);
        }
    }
}
