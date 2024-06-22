using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.Resource;
using KdyWeb.IService.Resource;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 资源接口
    /// </summary>
    public class ResourceController : BaseNormalController
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        /// <summary>
        /// 默认全局资源
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-default")]
        public async Task<KdyResult<GetAllResourceDto>> GetAllResourceAsync()
        {
            return await _resourceService.GetAllResourceAsync();
        }
    }
}
