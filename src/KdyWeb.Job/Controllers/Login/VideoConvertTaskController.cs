using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.IService.VideoConverts;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Login
{
    /// <summary>
    /// 视频转码任务
    /// </summary>
    public class VideoConvertTaskController : BaseLoginController
    {
        private readonly IVideoConvertTaskService _videoConvertTaskService;

        public VideoConvertTaskController(IVideoConvertTaskService videoConvertTaskService)
        {
            _videoConvertTaskService = videoConvertTaskService;
        }

        /// <summary>
        /// 接任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("take")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TakeTaskAsync(List<long> ids)
        {
            var result = await _videoConvertTaskService.TakeTaskAsync(ids);
            return Ok(result);
        }

        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryConvertTaskWithNormalDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryConvertTaskWithNormalAsync([FromQuery] QueryConvertTaskWithNormalInput input)
        {
            var result = await _videoConvertTaskService.QueryConvertTaskWithNormalAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 查询我的任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryMe")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryMeTaskListDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryMeTaskListAsync([FromQuery] QueryMeTaskListInput input)
        {
            var result = await _videoConvertTaskService.QueryMeTaskListAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("submit")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SubmitTaskAsync(SubmitTaskInput input)
        {
            var result = await _videoConvertTaskService.SubmitTaskAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("cancel")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CancelTaskAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _videoConvertTaskService.CancelTaskAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 提交资源
        /// </summary>
        /// <returns></returns>
        [HttpPost("publish-vod")]
        public async Task<KdyResult> PublishVodAsync(PublishVodInput input)
        {
            var result = await _videoConvertTaskService.PublishVodAsync(input);
            return result;
        }
    }
}
