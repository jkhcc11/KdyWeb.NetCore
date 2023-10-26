using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse.SelfHost.Models;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParse.SelfHost.Controllers
{
    /// <summary>
    /// 统一播放器
    /// </summary>
    [Route("player-v2")]
    public class CloudParsePlayerController : BaseController
    {
        private readonly IDiskParseService _diskParseService;
        private readonly ISubAccountService _subAccountService;
        public CloudParsePlayerController(IDiskParseService diskParseService,
            ISubAccountService subAccountService)
        {
            _diskParseService = diskParseService;
            _subAccountService = subAccountService;
        }

        /// <summary>
        /// 通用
        /// </summary>
        /// <returns></returns>
        [HttpGet("common/{userInfo}/{fileInfo}")]
        public async Task<ActionResult> CommonCloudParseAsync(CloudParsePlayerInput input)
        {
            Response.Headers.Add("PowerBy", "VGdfemN5MjAyMw==");
            var referrer = HttpContext.Request.Headers["Referer"] + "";
            var newBusinessFlag = await _subAccountService.GetBusinessFlagByUserIdAsync(input.UserInfo
                , input.IsOldUserInfo == false);
            if (string.IsNullOrEmpty(newBusinessFlag))
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "未知用户信息");
                return View("Index");
            }

            var cachePrefix = $"{CacheKeyConst.BusinessFlagToDownCachePrefix(newBusinessFlag)}:";
            var isNeedSerCookie = CloudParseCookieType.IsNeedServerCookie(newBusinessFlag);
            KdyResult<CommonParseDto> parseResult;
            if (isNeedSerCookie)
            {
                parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
                    referrer, newBusinessFlag,
                    input.UserInfo, input.FileInfo, false,
                    input.ParseModel == ParseModel.Name);
            }
            else
            {
                parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
                    referrer, newBusinessFlag,
                    input.UserInfo, input.FileInfo, false,
                    input.ParseModel == ParseModel.Name);
            }

            return ToActionResult(parseResult);
        }

        #region remove
        ///// <summary>
        ///// 胜天
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("st/{userInfo}/{fileInfo}")]
        //public async Task<ActionResult> StCloudParseAsync(CloudParsePlayerInput input)
        //{
        //    var referrer = HttpContext.Request.Headers["Referer"] + "";
        //    var cachePrefix = $"{CacheKeyConst.StCacheKey.DownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
        //        referrer, CloudParseCookieType.BitQiu,
        //        input.UserInfo, input.FileInfo, false,
        //        input.ParseModel == ParseModel.Name);
        //    return ToActionResult(parseResult);
        //}

        ///// <summary>
        ///// 阿里
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("ali/{userInfo}/{fileInfo}")]
        //public async Task<ActionResult> AliCloudParseAsync(CloudParsePlayerInput input)
        //{
        //    var referrer = HttpContext.Request.Headers["Referer"] + "";
        //    var cachePrefix = $"{CacheKeyConst.AliYunCacheKey.DownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
        //        referrer, CloudParseCookieType.Ali,
        //        input.UserInfo, input.FileInfo, true,
        //        input.ParseModel == ParseModel.Name);
        //    return ToActionResult(parseResult);
        //}

        ///// <summary>
        ///// 天翼云 个人
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("ty-person/{userInfo}/{fileInfo}")]
        //public async Task<ActionResult> TyPersonCloudParseAsync(CloudParsePlayerInput input)
        //{
        //    var referrer = HttpContext.Request.Headers["Referer"] + "";
        //    var cachePrefix = $"{CacheKeyConst.TyCacheKey.DownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
        //        referrer, CloudParseCookieType.TyPerson,
        //        input.UserInfo, input.FileInfo, false,
        //        input.ParseModel == ParseModel.Name);
        //    return ToActionResult(parseResult);
        //}

        ///// <summary>
        ///// 天翼云 家庭
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("ty-family/{userInfo}/{fileInfo}")]
        //public async Task<ActionResult> TyFamilyCloudParseAsync(CloudParsePlayerInput input)
        //{
        //    var referrer = HttpContext.Request.Headers["Referer"] + "";
        //    var cachePrefix = $"{CacheKeyConst.TyCacheKey.FamilyDownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
        //        referrer, CloudParseCookieType.TyFamily,
        //        input.UserInfo, input.FileInfo, false,
        //        input.ParseModel == ParseModel.Name);
        //    return ToActionResult(parseResult);
        //}

        ///// <summary>
        ///// 天翼云 企业
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("ty-crop/{userInfo}/{fileInfo}")]
        //public async Task<ActionResult> TyCropCloudParseAsync(CloudParsePlayerInput input)
        //{
        //    var referrer = HttpContext.Request.Headers["Referer"] + "";
        //    var cachePrefix = $"{CacheKeyConst.TyCacheKey.CropDownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
        //        referrer, CloudParseCookieType.TyCrop,
        //        input.UserInfo, input.FileInfo, false,
        //        input.ParseModel == ParseModel.Name);
        //    return ToActionResult(parseResult);
        //}

        ///// <summary>
        ///// 盘139
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("hc/{userInfo}/{fileInfo}")]
        //public async Task<ActionResult> Pan139CloudParseAsync(CloudParsePlayerInput input)
        //{
        //    var referrer = HttpContext.Request.Headers["Referer"] + "";
        //    var cachePrefix = $"{CacheKeyConst.Pan139CacheKey.DownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
        //        referrer, CloudParseCookieType.Pan139,
        //        input.UserInfo, input.FileInfo, true,
        //        input.ParseModel == ParseModel.Name);
        //    return ToActionResult(parseResult);
        //} 
        #endregion

        /// <summary>
        /// 转为Action
        /// </summary>
        /// <returns></returns>
        private ActionResult ToActionResult(KdyResult<CommonParseDto> parseResult)
        {
            if (parseResult.IsSuccess == false)
            {
                ViewBag.ParseResult = KdyResult.Error(parseResult.Code, parseResult.Msg);
                return View("Index");
            }

            ViewBag.ParseResult = KdyResult.Success<string>(parseResult.Data.DownLink.ToStrConfuse(7));
            return View("Index");
        }
    }
}
