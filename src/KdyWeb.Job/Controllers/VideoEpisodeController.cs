using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 剧集相关
    /// </summary>
    public class VideoEpisodeController : OldBaseApiController
    {
        private readonly IVideoEpisodeService _videoEpisodeService;

        public VideoEpisodeController(IVideoEpisodeService videoEpisodeService)
        {
            _videoEpisodeService = videoEpisodeService;
        }

        /// <summary>
        /// 根据剧集Id获取影片数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("epDetail/{epId}")]
        [ProducesResponseType(typeof(KdyResult<GetEpisodeInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEpisodeInfoAsync(long epId)
        {
            var result = await _videoEpisodeService.GetEpisodeInfoAsync(epId);
            return Ok(result);
        }

        /// <summary>
        /// 更新剧集
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateEpisode")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateEpisodeAsync(List<UpdateEpisodeInput> input)
        {
            var result = await _videoEpisodeService.UpdateEpisodeAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 创建剧集
        /// </summary>
        /// <returns></returns>
        [HttpPost("createEpisode")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateEpisodeAsync(CreateEpisodeInput input)
        {
            var result = await _videoEpisodeService.CreateEpisodeAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 删除剧集
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteEpisodeAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _videoEpisodeService.DeleteEpisodeAsync(input);
            return Ok(result);
        }
    }
}
