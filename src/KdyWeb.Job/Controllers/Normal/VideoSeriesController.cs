using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 影片系列相关
    /// </summary>
    public class VideoSeriesController : BaseNormalController
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
        /// 获取影片系列
        /// </summary>
        /// <returns></returns>
        [HttpGet("getSeriesList")]
        public async Task<KdyResult<List<SelectedItemOut>>> GetVideoSeriesListAsync()
        {
            var result = await _videoSeriesService.GetVideoSeriesListAsync();
            return result;
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
    }
}
