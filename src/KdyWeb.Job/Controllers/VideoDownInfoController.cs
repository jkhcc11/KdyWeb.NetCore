using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 影片下载地址
    /// </summary>
    public class VideoDownInfoController : OldBaseApiController
    {
        private readonly IVideoDownInfoService _videoDownInfoService;

        public VideoDownInfoController(IVideoDownInfoService videoDownInfoService)
        {
            _videoDownInfoService = videoDownInfoService;
        }

        /// <summary>
        /// 查询下载地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryVideoDownInfoDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryVideoHistoryAsync([FromQuery] QueryVideoDownInfoInput input)
        {
            var result = await _videoDownInfoService.QueryVideoDownInfoAsync(input);
            return Ok(result);
        }
    }
}
