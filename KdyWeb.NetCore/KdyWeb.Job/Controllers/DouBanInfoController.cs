using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
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
        [ProducesResponseType(typeof(KdyResult<List<GetTop50DouBanInfoDto>>), (int)HttpStatusCode.OK)]
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
        [ProducesResponseType(typeof(KdyResult<PageList<QueryDouBanInfoDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryDouBanInfoAsync([FromQuery] QueryDouBanInfoInput input)
        {
            var result = await _douBanInfoService.QueryDouBanInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        [ProducesResponseType(typeof(KdyResult<GetDouBanInfoForIdDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDouBanInfoForIdAsync(int id)
        {
            var result = await _douBanInfoService.GetDouBanInfoForIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// 变更豆瓣信息状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("change")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeDouBanInfoAsync(ChangeDouBanInfoStatusInput input)
        {
            var result = await _douBanInfoService.ChangeDouBanInfoStatusAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 重试保存图片
        /// </summary>
        /// <returns></returns>
        [HttpPut("retrySaveImg/{id}")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RetrySaveImgAsync(int id)
        {
            var result = await _douBanInfoService.RetrySaveImgAsync(id);
            return Ok(result);
        }
    }
}
