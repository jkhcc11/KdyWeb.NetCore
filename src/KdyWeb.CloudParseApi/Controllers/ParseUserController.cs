using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.CloudParse.CacheItem;
using KdyWeb.Dto.KdyUser;
using KdyWeb.Dto.Selenium;
using KdyWeb.IService;
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.Selenium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [CustomRoute("parse-user")]
    public class ParseUserController : BaseApiController
    {
        private readonly IKdyUserService _kdyUserService;
        private readonly ICloudParseUserService _cloudParseUserService;
        private readonly ISubAccountService _subAccountService;
        private readonly ISeleniumLoginService _seleniumLoginService;

        public ParseUserController(IKdyUserService kdyUserService,
            ICloudParseUserService cloudParseUserService,
            ISubAccountService subAccountService, ISeleniumLoginService seleniumLoginService)
        {
            _kdyUserService = kdyUserService;
            _cloudParseUserService = cloudParseUserService;
            _subAccountService = subAccountService;
            _seleniumLoginService = seleniumLoginService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetLoginTokenAsync(GetLoginTokenInput input)
        {
            var result = await _kdyUserService.GetLoginTokenAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUserAsync(CreateUserInput input)
        {
            var result = await _kdyUserService.CreateUserAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet("exit")]
        [ProducesResponseType(typeof(KdyResult<bool>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUserExitAsync([FromQuery] CheckUserExitInput input)
        {
            var result = await _kdyUserService.CheckUserExitAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("find")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> FindUserPwdAsync(FindUserPwdInput input)
        {
            var result = await _kdyUserService.FindUserPwdAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 登录信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("login-info")]
        [ProducesResponseType(typeof(KdyResult<GetLoginInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLoginInfoAsync()
        {
            var result = await _kdyUserService.GetLoginInfoAsync();
            return Ok(result);
        }

        /// <summary>
        /// 查询子账号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-sub-account")]
        public async Task<KdyResult<PageList<QueryParseUserSubAccountDto>>> QueryParseUserSubAccountAsync([FromQuery] QueryParseUserSubAccountInput input)
        {
            var result = await _cloudParseUserService.QueryParseUserSubAccountAsync(input);
            return result;
        }

        /// <summary>
        /// 新增或修改子账号
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-and-update-sub-account")]
        public async Task<KdyResult> CreateAndUpdateSubAccountAsync(CreateAndUpdateSubAccountInput input)
        {
            var result = await _cloudParseUserService.CreateAndUpdateSubAccountAsync(input);
            return result;
        }

        /// <summary>
        /// 用户所有子账号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-sub-account")]
        public async Task<KdyResult<IList<QueryParseUserSubAccountDto>>> GetUserAllSubAccountAsync()
        {
            var result = await _cloudParseUserService.GetUserAllSubAccountAsync();
            return result;
        }

        /// <summary>
        /// 获取所有类型Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-cookie-type")]
        public async Task<KdyResult<List<CloudParseCookieTypeCacheItem>>> GetAllCookieTypeCacheAsync()
        {
            var result = await _subAccountService.GetAllCookieTypeCacheAsync();
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 根据刷新Token获取Token
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh/{refreshToken}")]
        [AllowAnonymous]
        public async Task<KdyResult<GetLoginTokenDto>> RefreshTokenAsync(string refreshToken)
        {
            var result = await _kdyUserService.RefreshTokenAsync(refreshToken);
            if (result.IsSuccess == false)
            {
                throw new AuthenticationException("用户信息失效,请刷新页面");
            }

            return result;
        }

        /// <summary>
        /// 通用Url解析
        /// </summary>
        /// <returns></returns>
        [HttpGet("selenium-parse")]
        [AllowAnonymous]
        public async Task<KdyResult<string>> ParseVideoByUrlAsync([FromQuery] ParseVideoByUrlInput input)
        {
            if (input.IsDelay)
            {
                Task.Run(() => _seleniumLoginService.ParseVideoByUrlAsync(input));
                return KdyResult.Success<string>("任务已提交");
            }

            var result = await _seleniumLoginService.ParseVideoByUrlAsync(input);
            return result;
        }
    }
}
