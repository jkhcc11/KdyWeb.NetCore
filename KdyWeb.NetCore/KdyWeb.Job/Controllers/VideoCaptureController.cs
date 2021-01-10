using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 影片采集
    /// </summary>
    public class VideoCaptureController : OldBaseApiController
    {
        private readonly IVideoCaptureService _videoCaptureService;

        public VideoCaptureController(IVideoCaptureService videoCaptureService)
        {
            _videoCaptureService = videoCaptureService;
        }

        /// <summary>
        /// 根据影片源详情创建影片
        /// </summary>
        /// <returns></returns>
        [HttpPost("createForDetail")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryUserSubscribeAsync(CreateVideoInfoByDetailInput input)
        {
            var result = await _videoCaptureService.CreateVideoInfoByDetailAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 初始化 定时影片录入Job
        /// </summary>
        /// <returns></returns>
        [HttpGet("initJob")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateRecurringVideoJobAsync()
        {
            var result = await _videoCaptureService.CreateRecurringVideoJobAsync();
            return Ok(result);
        }
    }
}
