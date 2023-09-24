using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 剧集相关
    /// </summary>
    public class VideoEpisodeController : BaseNormalController
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
        /// 批量接收播放地址入库
        /// </summary>
        /// <returns></returns>
        [HttpPost("batch-receive-vod-url")]
        public async Task<KdyResult> BatchReceiveVodUrlAsync(BatchReceiveVodUrlInput input)
        {
            var result = await _videoEpisodeService.BatchReceiveVodUrlAsync(input);
            return result;
        }
    }
}
