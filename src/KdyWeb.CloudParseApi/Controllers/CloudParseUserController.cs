using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.CloudParse;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 解析用户
    /// </summary>
    //[Authorize(Policy = AuthorizationConst.NormalPolicyName.NormalPolicy)]
    [CustomRoute("user-info")]
    public class CloudParseUserController : BaseApiController
    {
        private readonly ICloudParseUserService _cloudParseUserService;

        public CloudParseUserController(ICloudParseUserService cloudParseUserService)
        {
            _cloudParseUserService = cloudParseUserService;
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

        /// <summary>
        /// 创建解析用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("create")]
        public async Task<KdyResult> CreateParesUserAsync()
        {
            var result = await _cloudParseUserService.CreateParesUserAsync();
            return result;
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <returns></returns>
        [HttpPost("audit/{id}")]
        [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public async Task<KdyResult> AuditAsync(long id)
        {
            var result = await _cloudParseUserService.AuditAsync(id);
            return result;
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<PageList<QueryParseUserDto>>> QueryParseUserAsync([FromQuery] QueryParseUserInput input)
        {
            var result = await _cloudParseUserService.QueryParseUserAsync(input);
            return result;
        }

        /// <summary>
        /// 延期
        /// </summary>
        /// <returns></returns>
        [HttpPost("delay-data/{id}")]
        [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public async Task<KdyResult> DelayDateAsync(long id)
        {
            var result = await _cloudParseUserService.DelayDateAsync(id);
            return result;
        }
    }
}
