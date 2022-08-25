using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// Setting
    /// </summary>
    [Obsolete("临时使用")]
    public class SettingController : BaseManagerController
    {
        private readonly IKdyRedisCache _redisCache;

        public SettingController(IKdyRedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        /// <summary>
        /// 添加游戏检查cookie
        /// </summary>
        /// <returns></returns>
        [HttpPost("game-check-cookie")]
        [AllowAnonymous]
        public async Task<IActionResult> AddGameCheckCookieAsync(AddGameCheckCookieInput input)
        {
            var db = _redisCache.GetDb(2);
            var result = await db.HashSetAsync(KdyServiceCacheKey.GameCheckKey, input.Uid, input.Cookie);
            return Ok(result);
        }
    }

    public class AddGameCheckCookieInput
    {
        public string Uid { get; set; }


        public string Cookie { get; set; }
    }
}
