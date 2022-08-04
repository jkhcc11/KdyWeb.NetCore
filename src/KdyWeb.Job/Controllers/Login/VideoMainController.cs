using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Login
{
    /// <summary>
    /// 影片相关
    /// </summary>
    public class VideoMainController : BaseLoginController
    {
        private readonly IVideoMainService _videoMainService;

        public VideoMainController(IVideoMainService videoMainService)
        {
            _videoMainService = videoMainService;
        }

        /// <summary>
        /// 分页查询影视库
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryVideoMainDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryVideoMainAsync([FromQuery] QueryVideoMainInput input)
        {
            var result = await _videoMainService.QueryVideoMainAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取影片统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getCountInfo")]
        [ProducesResponseType(typeof(KdyResult<List<GetCountInfoBySubtypeDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCountInfoBySubtypeAsync([FromQuery] GetCountInfoBySubtypeInput input)
        {
            var result = await _videoMainService.GetCountInfoBySubtypeAsync(input);
            return Ok(result);
        }
    }
}
