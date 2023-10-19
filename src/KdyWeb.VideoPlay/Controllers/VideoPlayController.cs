using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.VideoPlay.Controllers
{
    /// <summary>
    /// 视频播放
    /// </summary>
    public class VideoPlayController : BaseController
    {
        /// <summary>
        /// 播放页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(long id)
        {
            ViewData["Title"] = "看电影";

            ViewBag.EpId = id;

            Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self' http://*.kdy666.pro");
            return View();
        }
    }
}
