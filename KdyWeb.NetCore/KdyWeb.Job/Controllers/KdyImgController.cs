using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.KdyFile;
using KdyWeb.Dto.KdyImg;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.KdyFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 图床
    /// </summary>
    public class KdyImgController : OldBaseApiController
    {
        private readonly IKdyImgSaveService _kdyImgSaveService;

        public KdyImgController(IKdyImgSaveService kdyImgSaveService)
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
        // [DisableKdyLog]
        public async Task<IActionResult> PostFileByByteAsync([FromForm] PostFileByByteInput input)
        {
            var result = await _kdyImgSaveService.PostFileByByteAsync(input);
            return Ok(result);
        }
    }
}
