using System.Threading.Tasks;
using KdyWeb.IService;
using KdyWeb.VideoPlay.Models;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.VideoPlay.Controllers.ApiController
{
    /// <summary>
    /// 解析Api
    /// </summary>
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
        [HttpPost("result")]
        public async Task<IActionResult> Index(PlayApiInput input)
        {
            var result = await _videoPlayService.GetVideoInfoByEpIdAsync(input.EpId);
            return Ok(result);
        }
    }
}
