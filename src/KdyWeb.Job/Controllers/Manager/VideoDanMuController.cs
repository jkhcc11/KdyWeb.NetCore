using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 视频弹幕
    /// </summary>
    public class VideoDanMuController : BaseManagerController
    {
        private readonly IVideoDanMuService _videoDanMuService;

        public VideoDanMuController(IVideoDanMuService videoDanMuService)
        {
            _videoDanMuService = videoDanMuService;
        }

        /// <summary>
        /// 批量删除弹幕
        /// </summary>
        /// <returns></returns>
        [HttpDelete("batch")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _videoDanMuService.DeleteAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 搜索弹幕
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<PageList<SearchDanMuDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchDanMuAsync([FromQuery] SearchDanMuInput input)
        {
            var result = await _videoDanMuService.SearchDanMuAsync(input);
            return Ok(result);
        }
    }
}
