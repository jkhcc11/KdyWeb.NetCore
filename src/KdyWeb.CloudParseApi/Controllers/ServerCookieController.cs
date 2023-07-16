using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 服务器Cooke
    /// </summary>
    [CustomRoute("server-cookie")]
    public class ServerCookie : BaseApiController
    {
        private readonly IServerCookieService _serverCookieService;

        public ServerCookie(IServerCookieService serverCookieService)
        {
            _serverCookieService = serverCookieService;
        }

        /// <summary>
        /// 创建和更新服务器Cookie
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-and-update")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<KdyResult> CreateAndUpdateServerCookieAsync(CreateAndUpdateServerCookieInput input)
        {
            var result = await _serverCookieService.CreateAndUpdateServerCookieAsync(input);
            return result;
        }

        /// <summary>
        /// 查询服务器Cookie列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<PageList<QueryServerCookieDto>>> QueryServerCookieAsync([FromQuery] QueryServerCookieInput input)
        {
            var result = await _serverCookieService.QueryServerCookieAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public async Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _serverCookieService.BatchDeleteAsync(input);
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
            var result = await _serverCookieService.AuditAsync(id);
            return result;
        }
    }
}
