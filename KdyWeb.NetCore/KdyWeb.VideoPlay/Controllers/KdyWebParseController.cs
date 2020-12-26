using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.VideoPlay.Controllers
{
    /// <summary>
    /// 解析相关
    /// </summary>
    public class KdyWebParseController : BaseApiController
    {
        private readonly ICctvWebParseService _cctvWebParseService;

        public KdyWebParseController(ICctvWebParseService cctvWebParseService)
        {
            _cctvWebParseService = cctvWebParseService;
        }

        /// <summary>
        /// cctv解析
        /// </summary>
        /// <returns></returns>
        [HttpGet("cctv")]
        [ProducesResponseType(typeof(KdyResult<KdyWebParseOut>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryVideoSeriesAsync([FromQuery] KdyWebParseInput input)
        {
            var result = await _cctvWebParseService.GetResultAsync(input);
            return Ok(result);
        }
    }
}
