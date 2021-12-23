using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Login
{
    /// <summary>
    /// 反馈信息
    /// </summary>
    public class FeedBackInfoController : BaseLoginController
    {
        private readonly IFeedBackInfoService _feedBackInfoService;

        public FeedBackInfoController(IFeedBackInfoService feedBackInfoService)
        {
            _feedBackInfoService = feedBackInfoService;
        }

        /// <summary>
        /// 创建反馈信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateFeedBackInfoAsync(CreateFeedBackInfoInput input)
        {
            var result = await _feedBackInfoService.CreateFeedBackInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 创建反馈信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("createByDouBan")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateFeedBackInfoByDouBanAsync(CreateFeedBackInfoInput input)
        {
            var result = await _feedBackInfoService.CreateFeedBackInfoAsync(input);
            return Ok(result);
        }
    }
}
