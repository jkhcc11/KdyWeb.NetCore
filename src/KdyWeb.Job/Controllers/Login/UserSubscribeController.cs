using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Login
{
    /// <summary>
    /// 用户订阅
    /// </summary>
    public class UserSubscribeController : BaseLoginController
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

        /// <summary>
        /// 创建用户收藏
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateUserSubscribeAsync(CreateUserSubscribeInput input)
        {
            var result = await _userSubscribeService.CreateUserSubscribeAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 取消用户收藏
        /// </summary>
        /// <returns></returns>
        [HttpPatch("cancel/{subId}")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CancelUserSubscribeAsync(long subId)
        {
            var result = await _userSubscribeService.CancelUserSubscribeAsync(subId);
            return Ok(result);
        }
    }
}
