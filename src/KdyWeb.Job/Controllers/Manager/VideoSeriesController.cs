﻿using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 影片系列相关
    /// </summary>
    public class VideoSeriesController : BaseManagerController
    {
        private readonly IVideoSeriesService _videoSeriesService;

        public VideoSeriesController(IVideoSeriesService videoSeriesService)
        {
            _videoSeriesService = videoSeriesService;
        }

        /// <summary>
        /// 创建系列
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateVideoSeriesAsync([FromBody] CreateVideoSeriesInput input)
        {
            var result = await _videoSeriesService.CreateVideoSeriesAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 修改影片系列
        /// </summary>
        /// <returns></returns>
        [HttpPatch("modify")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ModifyVideoSeriesAsync([FromBody] ModifyVideoSeriesInput input)
        {
            var result = await _videoSeriesService.ModifyVideoSeriesAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 创建影片系列列表
        /// </summary>
        /// <returns></returns>
        [HttpPut("createSeriesList")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateVideoSeriesListAsync(CreateVideoSeriesListInput input)
        {
            var result = await _videoSeriesService.CreateVideoSeriesListAsync(input);
            return Ok(result);
        }
    }
}