using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.KdyImg;
using KdyWeb.IService.FileStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.NetCore.Controllers.ApiController
{
    /// <summary>
    /// 图片管理
    /// </summary>
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
    public class KdyImgMgrController : BaseImgApiController
    {
        private readonly IKdyImgSaveService _kdyImgSaveService;
        public KdyImgMgrController(IKdyImgSaveService kdyImgSaveService)
        {
            _kdyImgSaveService = kdyImgSaveService;
        }

        /// <summary>
        /// 分页查询图床
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryKdyImg")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryKdyImgDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryKdyImgAsync([FromQuery] QueryKdyImgInput input)
        {
            var result = await _kdyImgSaveService.QueryKdyImgAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 更新字段值
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateValueByField")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostFileByUrlAsync(UpdateValueByFieldInput input)
        {
            var result = await _kdyImgSaveService.UpdateValueByFieldAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 批量删除图床
        /// </summary>
        /// <returns></returns>
        [HttpDelete("batchDelete")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _kdyImgSaveService.DeleteAsync(input);
            return Ok(result);
        }

    }
}
