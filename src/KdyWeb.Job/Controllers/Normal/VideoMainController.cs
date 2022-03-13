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
    /// 影片相关
    /// </summary>
    public class VideoMainController : BaseNormalController
    {
        private readonly IVideoMainService _videoMainService;

        public VideoMainController(IVideoMainService videoMainService)
        {
            _videoMainService = videoMainService;
        }

        /// <summary>
        /// 获取影片信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail/{keyId}")]
        [ProducesResponseType(typeof(KdyResult<GetVideoDetailDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVideoDetailAsync(long keyId)
        {
            var result = await _videoMainService.GetVideoDetailAsync(keyId);
            return Ok(result);
        }

        /// <summary>
        /// 查询同演员影片列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("querySameVideoByActor")]
        [ProducesResponseType(typeof(KdyResult<List<QuerySameVideoByActorDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QuerySameVideoByActorAsync([FromQuery] QuerySameVideoByActorInput input)
        {
            var result = await _videoMainService.QuerySameVideoByActorAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 分页查询影视库
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryVideoMainDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryVideoByNormalAsync([FromQuery] QueryVideoByNormalInput input)
        {
            var result = await _videoMainService.QueryVideoByNormalAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 随机影片(普通查询)
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryRand/{count}")]
        [ProducesResponseType(typeof(KdyResult<IList<QueryVideoMainDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RandVideoByNormalAsync(int count)
        {
            var result = await _videoMainService.RandVideoByNormalAsync(count);
            return Ok(result);
        }
    }
}
