using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.VideoPlay.Controllers
{
    /// <summary>
    /// 视频弹幕
    /// </summary>
    [AllowAnonymous]
    public class VideoDanMuController : BaseApiController
    {
        private readonly IVideoDanMuService _videoDanMuService;

        public VideoDanMuController(IVideoDanMuService videoDanMuService)
        {
            _videoDanMuService = videoDanMuService;
        }

        /// <summary>
        /// 获取弹幕
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{epId}")]
        public async Task<IActionResult> GetVideoDanMuAsync(long epId)
        {
            var result = await _videoDanMuService.GetVideoDanMuAsync(epId);
            return Content(result.Data, "text/xml");
            // return Ok(result.Data);
        }

        /// <summary>
        /// 弹幕
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateDanMuAsync(CreateDanMuInput input)
        {
            await _videoDanMuService.CreateDanMuAsync(input);
            return Ok();
        }
    }
}
