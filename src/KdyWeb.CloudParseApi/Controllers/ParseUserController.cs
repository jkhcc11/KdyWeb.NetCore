using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.KdyUser;
using KdyWeb.Entity.CloudParse.Enum;
using KdyWeb.IService;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("user")]
    public class ParseUserController : BaseApiController
    {
        private readonly IKdyUserService _kdyUserService;
        private readonly ICloudParseUserService _cloudParseUserService;

        public ParseUserController(IKdyUserService kdyUserService, ICloudParseUserService cloudParseUserService)
        {
            _kdyUserService = kdyUserService;
            _cloudParseUserService = cloudParseUserService;
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
        /// 新增或修改子账号
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-and-update-sub-account")]
        [ProducesResponseType(typeof(KdyResult<GetLoginInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<KdyResult> CreateAndUpdateSubAccountAsync(CreateAndUpdateSubAccountInput input)
        {
            var result = await _cloudParseUserService.CreateAndUpdateSubAccountAsync(input);
            return result;
        }

        /// <summary>
        /// 根据类型获取子账号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-sub-account")]
        [ProducesResponseType(typeof(KdyResult<List<GetSubAccountByTypeDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubAccountByTypeAsync(CloudParseCookieType type)
        {
            var result = await _cloudParseUserService.GetSubAccountByTypeAsync(type);
            return Ok(result);
        }
    }
}
