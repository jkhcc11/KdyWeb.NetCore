using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.IService.VideoConverts;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 转码订单
    /// </summary>
    public class ConvertOrderController : BaseManagerController
    {
        private readonly IConvertOrderService _convertOrderService;

        public ConvertOrderController(IConvertOrderService convertOrderService)
        {
            _convertOrderService = convertOrderService;
        }

        /// <summary>
        /// 审批订单
        /// </summary>
        /// <returns></returns>
        [HttpPost("audit")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AuditOrderAsync(AuditOrderInput input)
        {
            var result = await _convertOrderService.AuditOrderAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 驳回订单
        /// </summary>
        /// <returns></returns>
        [HttpPost("rejected")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RejectedOrderAsync(RejectedOrderInput input)
        {
            var result = await _convertOrderService.RejectedOrderAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 查询转码订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryOrderListWithAdminDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryOrderListWithAdminAsync([FromQuery] QueryOrderListWithAdminInput input)
        {
            var result = await _convertOrderService.QueryOrderListWithAdminAsync(input);
            return Ok(result);
        }
    }
}
