using System.Threading.Tasks;
using KdyWeb.IService;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.VideoPlay.Controllers
{
    /// <summary>
    /// 视频播放
    /// </summary>
    public class VideoPlayController : BaseController
    {
        private readonly IVideoPlayService _videoPlayService;

        public VideoPlayController(IVideoPlayService videoPlayService)
        {
            _videoPlayService = videoPlayService;
        }

        /// <summary>
        /// 播放页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(long id)
        {
            ViewData["Title"] = "看电影";

            var result = await _videoPlayService.GetVideoInfoByEpIdAsync(id);
            ViewBag.OutModel = ToJsonStr(result);
            return View();
        }
    }
}
