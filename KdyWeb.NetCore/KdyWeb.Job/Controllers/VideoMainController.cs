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
    /// 影片相关
    /// </summary>
    public class VideoMainController : OldBaseApiController
    {
        private readonly IVideoMainService _videoMainService;

        public VideoMainController(IVideoMainService videoMainService)
        {
            _videoMainService = videoMainService;
        }

        /// <summary>
        /// 通过豆瓣信息创建影片信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateForDouBanInfoAsync(CreateForDouBanInfoInput input)
        {
            var result = await _videoMainService.CreateForDouBanInfoAsync(input);
            return Ok(result);
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
        /// 更新字段值
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateField")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateValueByFieldAsync(UpdateValueByFieldInput input)
        {
            var result = await _videoMainService.UpdateValueByFieldAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 删除影片
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _videoMainService.DeleteAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 匹配豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("matchDouBanInfo")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> MatchDouBanInfoAsync(MatchDouBanInfoInput input)
        {
            var result = await _videoMainService.MatchDouBanInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 修改影片详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("modify")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ModifyVideoMainAsync(ModifyVideoMainInput input)
        {
            var result = await _videoMainService.ModifyVideoMainAsync(input);
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
