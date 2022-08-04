using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 站点页面搜索配置 相关
    /// </summary>
    public class PageSearchConfigController : BaseManagerController
    {
        private readonly IPageSearchConfigService _pageSearchConfigService;

        public PageSearchConfigController(IPageSearchConfigService pageSearchConfigService)
        {
            _pageSearchConfigService = pageSearchConfigService;
        }

        /// <summary>
        /// 创建搜索配置
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSearchConfigAsync(CreateSearchConfigInput input)
        {
            var result = await _pageSearchConfigService.CreateSearchConfigAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 修改搜索配置
        /// </summary>
        /// <returns></returns>
        [HttpPost("modify")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ModifySearchConfigAsync(ModifySearchConfigInput input)
        {
            var result = await _pageSearchConfigService.ModifySearchConfigAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 搜索配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<PageList<GetDetailConfigDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchConfigAsync([FromQuery] SearchConfigInput input)
        {
            var result = await _pageSearchConfigService.SearchConfigAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取详情配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{configId}")]
        [ProducesResponseType(typeof(KdyResult<GetDetailConfigDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetailConfigAsync(long configId)
        {
            var result = await _pageSearchConfigService.GetDetailConfigAsync(configId);
            return Ok(result);
        }

        /// <summary>
        /// 一键复制站点配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("copy/{configId}")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> OneCopyAsync(long configId)
        {
            var result = await _pageSearchConfigService.OneCopyAsync(configId);
            return Ok(result);
        }
    }
}
