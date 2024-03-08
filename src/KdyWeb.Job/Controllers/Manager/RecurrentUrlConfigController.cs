using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 循环Url配置 相关
    /// </summary>
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
    public class RecurrentUrlConfigController : BaseManagerController
    {
        private readonly IRecurrentUrlConfigService _recurrentUrlConfigService;

        public RecurrentUrlConfigController(IRecurrentUrlConfigService recurrentUrlConfigService)
        {
            _recurrentUrlConfigService = recurrentUrlConfigService;
        }

        /// <summary>
        /// 查询循环Job
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<PageList<QueryRecurrentUrlConfigDto>>> QueryRecurrentUrlConfigAsync([FromQuery] QueryRecurrentUrlConfigInput input)
        {
            var result = await _recurrentUrlConfigService.QueryRecurrentUrlConfigAsync(input);
            return result;
        }

        /// <summary>
        /// 创建循环Url配置
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult> CreateRecurrentUrlConfigAsync(CreateRecurrentUrlConfigInput input)
        {
            var result = await _recurrentUrlConfigService.CreateRecurrentUrlConfigAsync(input);
            return result;
        }

        /// <summary>
        /// 修改循环Url配置
        /// </summary>
        /// <returns></returns>
        [HttpPost("modify")]
        public async Task<KdyResult> ModifyRecurrentUrlConfigAsync(ModifyRecurrentUrlConfigInput input)
        {
            var result = await _recurrentUrlConfigService.ModifyRecurrentUrlConfigAsync(input);
            return result;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("batchDelete")]
        public async Task<KdyResult> BatchDeleteAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _recurrentUrlConfigService.JobBatchDelAsync(input);
            return result;
        }
    }
}
