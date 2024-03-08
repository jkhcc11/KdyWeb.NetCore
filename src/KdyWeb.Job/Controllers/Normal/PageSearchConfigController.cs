using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 站点页面搜索配置 相关
    /// </summary>
    public class PageSearchConfigController : BaseNormalController
    {
        private readonly IPageSearchConfigService _pageSearchConfigService;

        public PageSearchConfigController(IPageSearchConfigService pageSearchConfigService)
        {
            _pageSearchConfigService = pageSearchConfigService;
        }

        /// <summary>
        /// 搜索配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-show-config")]
        public async Task<KdyResult<List<QueryShowPageConfigDto>>> QueryShowPageConfigAsync([FromQuery] SearchConfigInput input)
        {
            var result = await _pageSearchConfigService.QueryShowPageConfigAsync(input);
            return result;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<NormalPageParseOut>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetResultAsync([FromQuery] NormalPageParseInput input)
        {
            var parseInput = new GetPageParseInstanceInput()
            {
                ConfigId = input.ConfigId
            };

            var pageResult = await _pageSearchConfigService.GetPageParseInstanceAsync(parseInput);
            if (pageResult.IsSuccess == false)
            {
                return Ok(pageResult);
            }

            var result = await pageResult.Data.Instance.GetResultAsync(input);
            return Ok(result);
        }
    }
}
