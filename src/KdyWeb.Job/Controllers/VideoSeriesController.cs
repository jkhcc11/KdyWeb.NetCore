﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 影片系列相关
    /// </summary>
    public class VideoSeriesController : OldBaseApiController
    {
        private readonly IVideoSeriesService _videoSeriesService;

        public VideoSeriesController(IVideoSeriesService videoSeriesService)
        {
            _videoSeriesService = videoSeriesService;
        }

        /// <summary>
        /// 查询影片系列
        /// </summary>
        /// <returns></returns>
        [HttpGet("querySeries")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryVideoSeriesDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryVideoSeriesAsync([FromQuery] QueryVideoSeriesInput input)
        {
            var result = await _videoSeriesService.QueryVideoSeriesAsync(input);
            return Ok(result);
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
        /// 查询影片系列列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("querySeriesList")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryVideoSeriesListDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryVideoSeriesListAsync([FromQuery] QueryVideoSeriesListInput input)
        {
            var result = await _videoSeriesService.QueryVideoSeriesListAsync(input);
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
        /// 获取影片系列
        /// </summary>
        /// <returns></returns>
        [HttpGet("getSeriesList")]
        [ProducesResponseType(typeof(List<SelectedItemOut>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVideoSeriesListAsync()
        {
            var result = await _videoSeriesService.GetVideoSeriesListAsync();
            return Ok(result);
        }

        /// <summary>
        /// 获取影片系列详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("getDetail/{seriesId}")]
        [ProducesResponseType(typeof(KdyResult<QueryVideoSeriesDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVideoSeriesDetailAsync(long seriesId)
        {
            var result = await _videoSeriesService.GetVideoSeriesDetailAsync(seriesId);
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
