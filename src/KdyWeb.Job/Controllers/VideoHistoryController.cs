﻿using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 视频播放记录
    /// </summary>
    public class VideoHistoryController : OldBaseApiController
    {
        private readonly IVideoHistoryService _videoHistoryService;

        public VideoHistoryController(IVideoHistoryService videoHistoryService)
        {
            _videoHistoryService = videoHistoryService;
        }

        /// <summary>
        /// 创建播放记录
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateVideoHistoryAsync(CreateVideoHistoryInput input)
        {
            var result = await _videoHistoryService.CreateVideoHistoryAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 搜索播放记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryVideoHistoryDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryVideoHistoryAsync([FromQuery] QueryVideoHistoryInput input)
        {
            var result = await _videoHistoryService.QueryVideoHistoryAsync(input);
            return Ok(result);
        }
    }
}
