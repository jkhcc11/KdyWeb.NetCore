using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.CloudParse;
using KdyWeb.IService;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 解析用户
    /// </summary>
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.NormalPolicy)]
    public class CloudParseUserController : BaseApiController
    {
        private readonly ICloudParseUserService _cloudParseUserService;

        public CloudParseUserController(ICloudParseUserService cloudParseUserService)
        {
            _cloudParseUserService = cloudParseUserService;
        }

        #region 主账号
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
    }
}
