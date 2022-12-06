using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.IService.GameDown;
using KdyWeb.IService.Job;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// Hangfire Job初始化
    /// </summary>
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
    public class JobInitController : BaseManagerController
    {
        private readonly IJobInitService _jobInitService;
        private readonly IGameDownWithByrutService _gameDownWithByrutService;
        private readonly IConfiguration _configuration;

        public JobInitController(IJobInitService jobInitService,
            IGameDownWithByrutService gameDownWithByrutService,
            IConfiguration configuration)
        {
            _jobInitService = jobInitService;
            _gameDownWithByrutService = gameDownWithByrutService;
            _configuration = configuration;
        }

        /// <summary>
        /// 初始化循环影片录入Job
        /// </summary>
        /// <returns></returns>
        [HttpGet("initVideoJob")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InitRecurringVideoJobAsync()
        {
            var result = await _jobInitService.InitRecurringVideoJobAsync();
            return Ok(result);
        }

        /// <summary>
        /// 初始化循环UrlJob
        /// </summary>
        /// <returns></returns>
        [HttpGet("initUrlJob")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InitRecurrentUrlJobAsync()
        {
            var result = await _jobInitService.InitRecurrentUrlJobAsync();
            return Ok(result);
        }

        /// <summary>
        /// 游戏下载分页初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet("init-game-down")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InitGameDownQueryPageInfoAsync(int page)
        {
            var ua = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.GameDownUaWithByrut);
            var cookie = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.GameDownCookieWithByrut);

            await _gameDownWithByrutService.QueryPageInfoAsync(page, ua, cookie);
            return Ok("操作成功");
        }
    }
}
