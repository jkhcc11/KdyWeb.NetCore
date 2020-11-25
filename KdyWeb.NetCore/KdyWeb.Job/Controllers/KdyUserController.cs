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
    }
}
