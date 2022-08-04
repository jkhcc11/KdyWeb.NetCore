using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.KdyImg;
using KdyWeb.IService.ImageSave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.NetCore.Controllers.ApiController
{
    /// <summary>
    /// 图片上传
    /// </summary>
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.NormalPolicy)]
    public class UploadController : BaseImgApiController
    {
        private readonly IKdyImgSaveService _kdyImgSaveService;

        public UploadController(IKdyImgSaveService kdyImgSaveService)
        {
            _kdyImgSaveService = kdyImgSaveService;
        }

        /// <summary>
        /// 通过Url上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("postFileByUrl")]
        [ProducesResponseType(typeof(KdyResult<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostFileByUrlAsync(PostFileByUrlInput input)
        {
            var result = await _kdyImgSaveService.PostFileByUrl(input);
            return Ok(result);
        }

        /// <summary>
        /// 通过Byte上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("postFileByByte")]
        [ProducesResponseType(typeof(KdyResult<string>), (int)HttpStatusCode.OK)]
        [DisableKdyLog]
        public async Task<IActionResult> PostFileByByteAsync([FromForm] PostFileByByteInput input)
        {
            var result = await _kdyImgSaveService.PostFileByByteAsync(input);
            return Ok(result);
        }

    }
}
