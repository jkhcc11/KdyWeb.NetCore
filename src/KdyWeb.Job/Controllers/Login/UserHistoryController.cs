using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Login
{
    /// <summary>
    /// 用户历史记录
    /// </summary>
    public class UserHistoryController : BaseLoginController
    {
        private readonly IUserHistoryService _userHistoryService;

        public UserHistoryController(IUserHistoryService userHistoryService)
        {
            _userHistoryService = userHistoryService;
        }

        /// <summary>
        /// 创建用户播放记录
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateUserHistoryAsync(CreateUserHistoryInput input)
        {
            var result = await _userHistoryService.CreateUserHistoryAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 用户播放记录分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryUserHistoryDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryUserHistoryAsync([FromQuery] QueryUserHistoryInput input)
        {
            var result = await _userHistoryService.QueryUserHistoryAsync(input);
            return Ok(result);
        }
    }
}
