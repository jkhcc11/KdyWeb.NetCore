using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse.SelfHost.Models;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.Selenium;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.Selenium;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParse.SelfHost.Controllers
{
    /// <summary>
    /// ApiJson格式
    /// </summary>
    [Route("api-v2")]
    [ApiController]
    public class CloudParseJsonController : BaseController
    {
        private readonly IDiskParseService _diskParseService;
        private readonly ISubAccountService _subAccountService;
        private readonly ISeleniumLoginService _seleniumLoginService;
        public CloudParseJsonController(IDiskParseService diskParseService,
            ISubAccountService subAccountService, ISeleniumLoginService seleniumLoginService)
        {
            _diskParseService = diskParseService;
            _subAccountService = subAccountService;
            _seleniumLoginService = seleniumLoginService;
        }

        /// <summary>
        /// 通用
        /// </summary>
        /// <returns></returns>
        [HttpGet("common/{userInfo}/{fileInfo}")]
        public async Task<JsonParseDto> CommonCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        {
            Response.Headers.Add("PowerBy", "VGdfemN5MjAyMw==");
            var newBusinessFlag = await _subAccountService.GetBusinessFlagByUserIdAsync(input.UserInfo
                , input.IsOldUserInfo == false);
            if (string.IsNullOrEmpty(newBusinessFlag))
            {
                return JsonParseDto.SetFail("未知用户信息");
            }

            var cachePrefix = $"{CacheKeyConst.BusinessFlagToDownCachePrefix(newBusinessFlag)}:";
            var isNeedSerCookie = CloudParseCookieType.IsNeedServerCookie(newBusinessFlag);
            KdyResult<CommonParseDto> parseResult;
            if (isNeedSerCookie)
            {
                parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
                    input.Token, newBusinessFlag,
                    input.UserInfo, input.FileInfo, false,
                    input.ParseModel == ParseModel.Name);
            }
            else
            {
                parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
                   input.Token, newBusinessFlag,
                   input.UserInfo, input.FileInfo, false,
                   input.ParseModel == ParseModel.Name);
            }

            return ToJsonParseDto(parseResult);
        }

        /// <summary>
        /// 天翼个人
        /// </summary>
        /// <returns></returns>
        [HttpPost("login-with-ty-person")]
        public async Task<KdyResult<string>> LoginWithTyPersonAsync(LoginBySeleniumInput input)
        {
            return await _seleniumLoginService.LoginWithTyPersonAsync(input);
        }

        /// <summary>
        /// 天翼H5
        /// </summary>
        /// <returns></returns>
        [HttpPost("login-with-ty-h5")]
        public async Task<KdyResult<string>> LoginWithTyH5Async(LoginBySeleniumInput input)
        {
            return await _seleniumLoginService.LoginWithTyH5Async(input);
        }

        /// <summary>
        /// 301跳转直链
        /// </summary>
        /// <returns></returns>
        [HttpGet("jump/{userInfo}/{fileInfo}")]
        public async Task<IActionResult> CommonCloudParseWithJump301Async([FromRoute] CloudParsePlayerInput input)
        {
            Response.Headers.Add("PowerBy", "VGdfemN5MjAyMw==");
            var newBusinessFlag = await _subAccountService.GetBusinessFlagByUserIdAsync(input.UserInfo
                , input.IsOldUserInfo == false);
            if (string.IsNullOrEmpty(newBusinessFlag))
            {
                return Content("未知用户信息");
            }

            var cachePrefix = $"{CacheKeyConst.BusinessFlagToDownCachePrefix(newBusinessFlag)}:";
            var isNeedSerCookie = CloudParseCookieType.IsNeedServerCookie(newBusinessFlag);
            KdyResult<CommonParseDto> parseResult;
            if (isNeedSerCookie)
            {
                parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
                    input.Token, newBusinessFlag,
                    input.UserInfo, input.FileInfo, false,
                    input.ParseModel == ParseModel.Name);
            }
            else
            {
                parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
                    input.Token, newBusinessFlag,
                    input.UserInfo, input.FileInfo, false,
                    input.ParseModel == ParseModel.Name);
            }

            if (parseResult.IsSuccess == false)
            {
                return Content(parseResult.Msg);
            }

            return Redirect(parseResult.Data.DownLink);
        }

        private JsonParseDto ToJsonParseDto(KdyResult<CommonParseDto> parseResult)
        {
            if (parseResult.IsSuccess == false)
            {
                return JsonParseDto.SetFail(parseResult.Msg);
            }

            return JsonParseDto.SetSuccess(parseResult.Data.DownLink);
        }
    }
}
