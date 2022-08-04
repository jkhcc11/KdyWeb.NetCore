using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 反馈信息
    /// </summary>
    public class FeedBackInfoController : BaseManagerController
    {
        private readonly IFeedBackInfoService _feedBackInfoService;

        public FeedBackInfoController(IFeedBackInfoService feedBackInfoService)
        {
            _feedBackInfoService = feedBackInfoService;
        }

        /// <summary>
        /// 变更反馈状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("change")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeFeedBackInfoAsync(ChangeFeedBackInfoInput input)
        {
            var result = await _feedBackInfoService.ChangeFeedBackInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取反馈信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(KdyResult<GetFeedBackInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFeedBackInfoAsync(int id)
        {
            var result = await _feedBackInfoService.GetFeedBackInfoAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// 批量删除反馈信息
        /// </summary>
        /// <returns></returns>
        [HttpDelete("batchDelete")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> BatchDeleteAsync(BatchDeleteForIntKeyInput input)
        {
            var result = await _feedBackInfoService.BatchDeleteAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取反馈统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getCountInfo")]
        [ProducesResponseType(typeof(KdyResult<List<GetCountInfoDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCountInfoAsync([FromQuery] GetCountInfoInput input)
        {
            var result = await _feedBackInfoService.GetCountInfoAsync(input);
            return Ok(result);
        }
    }
}
