using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.KdyUser;
using KdyWeb.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 用户
    /// </summary>
    public class KdyUserController : BaseNormalController
    {
        private readonly IKdyUserService _kdyUserService;
        public KdyUserController(IKdyUserService kdyUserService)
        {
            _kdyUserService = kdyUserService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
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
        public async Task<IActionResult> FindUserPwdAsync(FindUserPwdInput input)
        {
            var result = await _kdyUserService.FindUserPwdAsync(input);
            return Ok(result);
        }
    }
}
