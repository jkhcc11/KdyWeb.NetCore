using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.IService.VideoConverts;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 视频转码任务
    /// </summary>
    public class VideoConvertTaskController : BaseManagerController
    {
        private readonly IVideoConvertTaskService _videoConvertTaskService;

        public VideoConvertTaskController(IVideoConvertTaskService videoConvertTaskService)
        {
            _videoConvertTaskService = videoConvertTaskService;
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateTaskAsync(CreateTaskInput input)
        {
            var result = await _videoConvertTaskService.CreateTaskAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 查询任务列表(admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryConvertTaskWithAdminDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryConvertTaskWithAdminAsync([FromQuery] QueryConvertTaskWithAdminInput input)
        {
            var result = await _videoConvertTaskService.QueryConvertTaskWithAdminAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 删除Task
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{taskId}")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteTaskAsync(long taskId)
        {
            var result = await _videoConvertTaskService.DeleteTaskAsync(taskId);
            return Ok(result);
        }
    }
}
