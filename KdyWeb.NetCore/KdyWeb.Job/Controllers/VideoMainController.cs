using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 影片相关
    /// </summary>
    public class VideoMainController : OldBaseApiController
    {
        private readonly IVideoMainService _videoMainService;

        public VideoMainController(IVideoMainService videoMainService)
        {
            _videoMainService = videoMainService;
        }

        /// <summary>
        /// 创建并获取豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateForDouBanInfoAsync(CreateForDouBanInfoInput input)
        {
            var result = await _videoMainService.CreateForDouBanInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取影片信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail/{keyId}")]
        public async Task<IActionResult> GetVideoDetailAsync(long keyId)
        {
            var result = await _videoMainService.GetVideoDetailAsync(keyId);
            return Ok(result);
        }
    }
}
