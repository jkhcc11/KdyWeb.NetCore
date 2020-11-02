using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 豆瓣信息
    /// </summary>
    public class DouBanInfoController : OldBaseApiController
    {
        private readonly IDouBanInfoService _douBanInfoService;

        public DouBanInfoController(IDouBanInfoService douBanInfoService)
        {
            _douBanInfoService = douBanInfoService;
        }

        /// <summary>
        /// 创建并获取豆瓣信息
        /// </summary>
        /// <param name="subject">豆瓣Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("createGet/{subject}")]
        public async Task<IActionResult> CreateForSubjectIdAsync(string subject)
        {
            var result = await _douBanInfoService.CreateForSubjectIdAsync(subject);
            return Ok(result);
        }

        /// <summary>
        /// 获取最新Top信息
        /// </summary>
        /// <param name="top">最新几条</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getTop/{top}")]
        public async Task<IActionResult> GetTopDouBanInfoAsync(int top)
        {
            var result = await _douBanInfoService.GetTopDouBanInfoAsync(top);
            return Ok(result);
        }

        /// <summary>
        /// 查询豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("query")]
        public async Task<IActionResult> QueryDouBanInfoAsync([FromQuery] QueryDouBanInfoInput input)
        {
            var result = await _douBanInfoService.QueryDouBanInfoAsync(input);
            return Ok(result);
        }
    }
}
