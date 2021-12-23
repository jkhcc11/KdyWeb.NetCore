using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 反馈信息
    /// </summary>
    public class FeedBackInfoController : BaseNormalController
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
        [HttpGet("getPageList")]
        [ProducesResponseType(typeof(KdyResult<PageList<GetFeedBackInfoDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPageFeedBackInfoAsync([FromQuery] GetFeedBackInfoInput input)
        {
            var result = await _feedBackInfoService.GetPageFeedBackInfoAsync(input);
            return Ok(result);
        }
    }
}
