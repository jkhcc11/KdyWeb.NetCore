using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 豆瓣信息
    /// </summary>
    public class DouBanInfoController : BaseNormalController
    {
        private readonly IDouBanInfoService _douBanInfoService;

        public DouBanInfoController(IDouBanInfoService douBanInfoService)
        {
            _douBanInfoService = douBanInfoService;
        }

        /// <summary>
        /// 获取最新Top信息
        /// </summary>
        /// <param name="top">最新几条</param>
        /// <returns></returns>
        [HttpGet("getTop/{top}")]
        [ProducesResponseType(typeof(KdyResult<List<GetTop50DouBanInfoDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTopDouBanInfoAsync(int top)
        {
            var result = await _douBanInfoService.GetTopDouBanInfoAsync(top);
            return Ok(result);
        }
    }
}
