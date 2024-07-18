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
        private readonly IVideoMainMatchInfoService _videoMainMatchInfoService;

        public VideoMainController(IVideoMainService videoMainService,
            IVideoMainMatchInfoService videoMainMatchInfoService)
        {
            _videoMainService = videoMainService;
            _videoMainMatchInfoService = videoMainMatchInfoService;
        }

        /// <summary>
        /// 获取影片信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-home")]
        public async Task<KdyResult<List<HomeDataItem>>> GetHomeDataAsync()
        {
            return await _videoMainService.GetHomeDataAsync();
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

        /// <summary>
        /// 匹配豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("match-vod-info/{keyId}")]
        public async Task<KdyResult> MatchDouBanInfoAsync(long keyId)
        {
            var result = await _videoMainMatchInfoService.MatchDouBanInfoAsync(keyId);
            return result;
        }

        /// <summary>
        /// 匹配资源
        /// </summary>
        /// <returns></returns>
        [HttpGet("match-vod-zy/{keyId}")]
        public async Task<KdyResult> MatchZyAsync(long keyId)
        {
            var result = await _videoMainMatchInfoService.MatchZyAsync(keyId);
            return result;
        }

        /// <summary>
        /// 自动匹配并保存剧集
        /// </summary>
        /// <returns></returns>
        [HttpPost("auto-match-save")]
        public async Task<KdyResult<string>> AutoMatchSaveEpAsync(AutoMatchSaveEpInput input)
        {
            var result = await _videoMainMatchInfoService.AutoMatchSaveEpAsync(input);
            return result;
        }
    }
}
