using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 视频播放记录
    /// </summary>
    public class VideoHistoryController : BaseNormalController
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
        public async Task<KdyResult<PageList<QueryVideoHistoryDto>>> QueryVideoHistoryAsync(
            [FromQuery] QueryVideoHistoryInput input)
        {
            if ((input.EndTime - input.StartTime).TotalDays > 7)
            {
                return KdyResult.Success(new PageList<QueryVideoHistoryDto>(1, 15));
            }

            var result = await _videoHistoryService.QueryVideoHistoryAsync(input);
            return result;
        }
    }
}
