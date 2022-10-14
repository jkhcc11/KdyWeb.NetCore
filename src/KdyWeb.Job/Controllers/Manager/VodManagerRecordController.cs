using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.IService.VideoConverts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 影片管理者记录
    /// </summary>
    public class VodManagerRecordController : BaseManagerController
    {
        private readonly IVodManagerRecordService _vodManagerRecordService;

        public VodManagerRecordController(IVodManagerRecordService vodManagerRecordService)
        {
            _vodManagerRecordService = vodManagerRecordService;
        }

        /// <summary>
        /// 查询影片管理者记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryVodManagerRecordDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryVodManagerRecordAsync([FromQuery] QueryVodManagerRecordInput input)
        {
            var result = await _vodManagerRecordService.QueryVodManagerRecordAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 更新记录实际金额
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateActualAmount")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public async Task<IActionResult> UpdateRecordActualAmountAsync(UpdateRecordActualAmountInput input)
        {
            var result = await _vodManagerRecordService.UpdateRecordActualAmountAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 批量结算
        /// </summary>
        /// <returns></returns>
        [HttpPost("checkout")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public async Task<IActionResult> BatchCheckoutRecordAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _vodManagerRecordService.BatchCheckoutRecordAsync(input);
            return Ok(result);
        }

    }
}
