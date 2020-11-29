using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.IService;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class KdyUserController : OldBaseApiController
    {
        private readonly IKdyUserService _kdyUserService;

        public KdyUserController(IKdyUserService kdyUserService)
        {
            _kdyUserService = kdyUserService;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getInfo")]
        [ProducesResponseType(typeof(KdyResult<GetUserInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserInfoAsync([FromQuery] GetUserInfoInput input)
        {
            var result = await _kdyUserService.GetUserInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 创建用户
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
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckUserExitAsync([FromQuery] CheckUserExitInput input)
        {
            var result = await _kdyUserService.CheckUserExitAsync(input);
            return Ok(result);
        }
    }
}
