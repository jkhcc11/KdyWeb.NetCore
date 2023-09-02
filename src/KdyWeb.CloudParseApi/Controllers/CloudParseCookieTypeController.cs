using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.CloudParse.CacheItem;
using KdyWeb.Dto.KdyUser;
using KdyWeb.IService;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// Cookie类型
    /// </summary>
    [CustomRoute("cookie-type")]
    public class CloudParseCookieTypeController : BaseApiController
    {
        private readonly ICloudParseCookieTypeService _cloudParseCookieTypeService;

        public CloudParseCookieTypeController(ICloudParseCookieTypeService cloudParseCookieTypeService)
        {
            _cloudParseCookieTypeService = cloudParseCookieTypeService;
        }

        /// <summary>
        /// 新增或修改Cookie类型
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-and-update")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<KdyResult> CreateAndUpdateCookieTypeAsync(CreateAndUpdateCookieTypeInput input)
        {
            var result = await _cloudParseCookieTypeService.CreateAndUpdateCookieTypeAsync(input);
            return result;
        }

        /// <summary>
        /// 查询Cookie类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryCookieTypeDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryCookieTypeAsync([FromQuery] QueryCookieTypeInput input)
        {
            var result = await _cloudParseCookieTypeService.QueryCookieTypeAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public async Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _cloudParseCookieTypeService.BatchDeleteAsync(input);
            return result;
        }
    }
}
