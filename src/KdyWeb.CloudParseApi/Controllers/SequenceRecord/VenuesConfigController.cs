using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SequenceRecord;
using KdyWeb.IService.SequenceRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 场馆配置
    /// </summary>
    [CustomRoute("venues-config")]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "ppv1")]
    public class VenuesConfigController : BaseApiController
    {
        private readonly IVenuesConfigService _venuesConfigService;

        public VenuesConfigController(IVenuesConfigService venuesConfigService)
        {
            _venuesConfigService = venuesConfigService;
        }

        /// <summary>
        /// 分页查询场馆配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-page")]
        public async Task<KdyResult<PageList<QueryPageVenuesConfigDto>>> QueryPageVenuesConfigAsync([FromQuery] QueryPageVenuesConfigInput input)
        {
            var result = await _venuesConfigService.QueryPageVenuesConfigAsync(input);
            return result;
        }

        /// <summary>
        /// 创建场馆配置
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult> CreateVenuesConfigAsync(CreateVenuesConfigInput input)
        {
            var result = await _venuesConfigService.CreateVenuesConfigAsync(input);
            return result;
        }

        /// <summary>
        /// 禁用场馆奖励计算
        /// </summary>
        /// <returns></returns>
        [HttpGet("ban/{venuesConfigId}")]
        public async Task<KdyResult> BanVenuesGiftAsync(long venuesConfigId)
        {
            var result = await _venuesConfigService.BanVenuesGiftAsync(venuesConfigId);
            return result;
        }
    }
}
