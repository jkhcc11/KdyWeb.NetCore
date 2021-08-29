using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 站点页面解析 相关
    /// </summary>
    public class KdyPageParseController : OldBaseApiController
    {
        private readonly IPageSearchConfigService _pageSearchConfigService;

        public KdyPageParseController(IPageSearchConfigService pageSearchConfigService)
        {
            _pageSearchConfigService = pageSearchConfigService;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<NormalPageParseOut>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetResultAsync([FromQuery] NormalPageParseInput input)
        {
            var parseInput=new GetPageParseInstanceInput()
            {
                ConfigId = input.ConfigId,
              //  BaseHost = input.
            };

            var pageResult = await _pageSearchConfigService.GetPageParseInstanceAsync(parseInput);
            if (pageResult.IsSuccess == false)
            {
                return Ok(pageResult);
            }

            var result =await pageResult.Data.Instance.GetResultAsync(input);
            return Ok(result);
        }
    }
}
