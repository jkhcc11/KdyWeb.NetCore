using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 反馈信息
    /// </summary>
    public class FeedBackInfoController : OldBaseApiController
    {
        private readonly IFeedBackInfoService _feedBackInfoService;

        public FeedBackInfoController(IFeedBackInfoService feedBackInfoService)
        {
            _feedBackInfoService = feedBackInfoService;
        }

        /// <summary>
        /// 分页获取反馈信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getPageList")]
        public async Task<IActionResult> GetPageFeedBackInfoAsync([FromQuery] GetFeedBackInfoInput input)
        {
            var result = await _feedBackInfoService.GetPageFeedBackInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 创建反馈信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateFeedBackInfoAsync(CreateFeedBackInfoInput input)
        {
            var result = await _feedBackInfoService.CreateFeedBackInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 变更反馈状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("change")]
        public async Task<IActionResult> ChangeFeedBackInfoAsync(ChangeFeedBackInfoInput input)
        {
            var result = await _feedBackInfoService.ChangeFeedBackInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取反馈信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetFeedBackInfoAsync(int id)
        {
            var result = await _feedBackInfoService.GetFeedBackInfoAsync(id);
            return Ok(result);
        }

    }
}
