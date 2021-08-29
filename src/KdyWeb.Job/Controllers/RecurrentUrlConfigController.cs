using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 循环Url配置 相关
    /// </summary>
    public class RecurrentUrlConfigController : OldBaseApiController
    {
        private readonly IRecurrentUrlConfigService _recurrentUrlConfigService;

        public RecurrentUrlConfigController(IRecurrentUrlConfigService recurrentUrlConfigService)
        {
            _recurrentUrlConfigService = recurrentUrlConfigService;
        }

        /// <summary>
        /// 创建循环Url配置
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateRecurrentUrlConfigAsync(CreateRecurrentUrlConfigInput input)
        {
            var result = await _recurrentUrlConfigService.CreateRecurrentUrlConfigAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 修改循环Url配置
        /// </summary>
        /// <returns></returns>
        [HttpPost("modify")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ModifyRecurrentUrlConfigAsync(ModifyRecurrentUrlConfigInput input)
        {
            var result = await _recurrentUrlConfigService.ModifyRecurrentUrlConfigAsync(input);
            return Ok(result);
        }
    }
}
