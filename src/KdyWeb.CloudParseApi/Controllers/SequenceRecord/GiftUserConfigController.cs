using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SequenceRecord;
using KdyWeb.IService.SequenceRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 奖励用户配置
    /// </summary>
    [CustomRoute("gift-user-config")]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "ppv1")]
    public class GiftUserConfigController : BaseApiController
    {
        private readonly IGiftUserConfigService _giftUserConfigService;

        public GiftUserConfigController(IGiftUserConfigService giftUserConfigService)
        {
            _giftUserConfigService = giftUserConfigService;
        }

        /// <summary>
        /// 分页查询奖励用户配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-page")]
        public async Task<KdyResult<PageList<QueryPageGiftUserConfigDto>>> QueryPageGiftUserConfigAsync([FromQuery] QueryPageGiftUserConfigInput input)
        {
            var result = await _giftUserConfigService.QueryPageGiftUserConfigAsync(input);
            return result;
        }

        /// <summary>
        /// 创建奖励用户配置
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult> CreateGiftUserConfigAsync(CreateGiftUserConfigInput input)
        {
            var result = await _giftUserConfigService.CreateGiftUserConfigAsync(input);
            return result;
        }

        /// <summary>
        /// 修改奖励用户配置
        /// </summary>
        /// <returns></returns>
        [HttpPost("modify")]
        public async Task<KdyResult> ModifyGiftUserConfigAsync(ModifyGiftUserConfigInput input)
        {
            var result = await _giftUserConfigService.ModifyGiftUserConfigAsync(input);
            return result;
        }


        /// <summary>
        /// 禁用奖励用户配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("ban/{configId}")]
        public async Task<KdyResult> BanGiftUserConfigAsync(long configId)
        {
            var result = await _giftUserConfigService.BanGiftUserConfigAsync(configId);
            return result;
        }
    }
}
