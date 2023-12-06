using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Filter;
using KdyWeb.CloudParse.SelfHost.Models;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Service.CloudParse;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParse.SelfHost.Controllers
{
    /// <summary>
    /// 统一播放器
    /// </summary>
    [Route("player-v2")]
    [ValidateFetchHeaders]
    [AddPowerByHeaderFilter]
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
            var referrer = HttpContext.Request.Headers["Referer"] + "";
            var newBusinessFlag = await _subAccountService.GetBusinessFlagByUserIdAsync(input.UserInfo
                , input.IsOldUserInfo == false);
            if (string.IsNullOrEmpty(newBusinessFlag))
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "未知用户信息");
                return View("Index");
            }

            var cachePrefix = $"{DiskCloudParseFactory.BusinessFlagToDownCachePrefix(newBusinessFlag)}:";
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
