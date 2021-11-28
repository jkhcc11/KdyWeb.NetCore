using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.CloudParse;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 解析用户
    /// </summary>
    public class CloudParseUserController : BaseApiController
    {
        private readonly ICloudParseUserService _cloudParseUserService;

        public CloudParseUserController(ICloudParseUserService cloudParseUserService)
        {
            _cloudParseUserService = cloudParseUserService;
        }

        #region 主账号
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<KdyResult<GetParseUserInfoDto>> LoginWithParseUserAsync(LoginWithParseUserInput input)
        {
            var result = await _cloudParseUserService.LoginWithParseUserAsync(input);
            return result;
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("saveUserInfo")]
        public async Task<KdyResult> SaveParseUserInfoAsync(SaveParseUserInfoInput input)
        {
            var result = await _cloudParseUserService.SaveParseUserInfoAsync(input);
            return result;
        }

        /// <summary>
        /// 获取解析用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLoginUserInfo")]
        public async Task<KdyResult<GetParseUserInfoDto>> CreateUserAsync()
        {
            var result = await _cloudParseUserService.GetParseUserInfoAsync();
            return result;
        }
        #endregion

        #region 子账号
        /// <summary>
        /// 获取子账号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getChildrenList")]
        public async Task<KdyResult<PageList<GetParseUserInfoChildrenDto>>> GetParseUserInfoChildrenAsync([FromQuery] GetParseUserInfoChildrenInput input)
        {
            var result = await _cloudParseUserService.GetParseUserInfoChildrenAsync(input);
            return result;
        }

        /// <summary>
        /// 新增子账号
        /// </summary>
        /// <returns></returns>
        [HttpPut("createChildren")]
        public async Task<KdyResult> SaveParseUserInfoChildrenAsync(SaveParseUserInfoChildrenInput input)
        {
            var result = await _cloudParseUserService.SaveParseUserInfoChildrenAsync(input);
            return result;
        }

        /// <summary>
        /// 获取子账号信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getChildren/{id}")]
        public async Task<KdyResult<GetParseUserInfoChildrenDto>> CreateUserAsync(int id)
        {
            var result = await _cloudParseUserService.GetParseUserInfoChildrenAsync(id);
            return result;
        }
        #endregion
    }
}
