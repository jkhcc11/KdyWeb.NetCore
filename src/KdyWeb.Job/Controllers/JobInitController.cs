using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.IService.Job;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// Hangfire Job初始化
    /// </summary>
    public class JobInitController : OldBaseApiController
    {
        private readonly IJobInitService _jobInitService;

        public JobInitController(IJobInitService jobInitService)
        {
            _jobInitService = jobInitService;
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
    }
}
