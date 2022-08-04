using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.IService.VideoConverts;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Login
{
    /// <summary>
    /// 转码订单
    /// </summary>
    public class ConvertOrderController : BaseLoginController
    {
        private readonly IConvertOrderService _convertOrderService;

        public ConvertOrderController(IConvertOrderService convertOrderService)
        {
            _convertOrderService = convertOrderService;
        }

        /// <summary>
        /// 查询我的转码订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryMe")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryMeOrderListDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryMeOrderListAsync([FromQuery] QueryMeOrderListInput input)
        {
            var result = await _convertOrderService.QueryMeOrderListAsync(input);
            return Ok(result);
        }
    }
}
