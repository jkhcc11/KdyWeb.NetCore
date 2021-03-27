using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 用户订阅
    /// </summary>
    public class UserSubscribeController : OldBaseApiController
    {
        private readonly IUserSubscribeService _userSubscribeService;

        public UserSubscribeController(IUserSubscribeService userSubscribeService)
        {
            _userSubscribeService = userSubscribeService;
        }

        /// <summary>
        /// 用户收藏查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryUserSubscribeDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryUserSubscribeAsync([FromQuery] QueryUserSubscribeInput input)
        {
            var result = await _userSubscribeService.QueryUserSubscribeAsync(input);
            return Ok(result);
        }
    }
}
