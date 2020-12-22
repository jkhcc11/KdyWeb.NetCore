﻿using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 站点页面解析 相关
    /// </summary>
    public class KdyPageParseController : OldBaseApiController
    {
        private readonly INormalPageParseService _normalPageParseService;

        public KdyPageParseController(INormalPageParseService normalPageParseService)
        {
            _normalPageParseService = normalPageParseService;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<NormalPageParseOut>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetResultAsync([FromQuery] NormalPageParseInput input)
        {
            var result = await _normalPageParseService.GetResultAsync(input);
            return Ok(result);
        }
    }
}
