using System.Threading.Tasks;
using KdyWeb.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.VideoPlay.Controllers
{
    /// <summary>
    /// 解析Api
    /// </summary>
    [AllowAnonymous]
    public class PlayApiController : BaseApiController
    {
        private readonly IVideoPlayService _videoPlayService;

        public PlayApiController(IVideoPlayService videoPlayService)
        {
            _videoPlayService = videoPlayService;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <returns></returns>
        [HttpPost("getResult/{epId}")]
        public async Task<IActionResult> Index(long epId)
        {
            var result = await _videoPlayService.GetVideoInfoByEpIdAsync(epId);
            return Ok(result);
        }
    }
}
