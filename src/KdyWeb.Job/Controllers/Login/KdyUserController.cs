using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.IService;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Login
{
    /// <summary>
    /// 用户
    /// </summary>
    public class KdyUserController : BaseLoginController
    {
        private readonly IKdyUserService _kdyUserService;
        private readonly ILoginUserInfo _loginUserInfo;

        public KdyUserController(IKdyUserService kdyUserService, ILoginUserInfo loginUserInfo)
        {
            _kdyUserService = kdyUserService;
            _loginUserInfo = loginUserInfo;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getInfo")]
        [ProducesResponseType(typeof(KdyResult<GetUserInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var result = new GetUserInfoDto()
            {
                Id = _loginUserInfo.GetUserId(),
                UserEmail = _loginUserInfo.UserEmail,
                UserName = _loginUserInfo.UserName,
                UserNick = _loginUserInfo.UserNick,
                RoleName = _loginUserInfo.RoleName
            };
            await Task.CompletedTask;
            return Ok(KdyResult.Success(result));
        }


        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <returns></returns>
        [HttpPost("modifyPwd")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ModifyUserPwdAsync(ModifyUserPwdInput input)
        {
            var result = await _kdyUserService.ModifyUserPwdAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <returns></returns>
        [HttpPost("modifyInfo")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ModifyUserInfoAsync(ModifyUserInfoInput input)
        {
            var result = await _kdyUserService.ModifyUserInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> LogoutAsync()
        {
            var result = await _kdyUserService.LogoutAsync();
            return Ok(result);
        }
    }
}
