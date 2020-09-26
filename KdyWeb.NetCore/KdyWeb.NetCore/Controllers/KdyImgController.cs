using System;
using System.IO;
using System.Threading.Tasks;
using KdyWeb.Dto.KdyFile;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.KdyFile;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.NetCore.Controllers
{
    /// <summary>
    /// 图床
    /// </summary>
    [Route("KdyImg")]
    public class KdyImgController : Controller
    {
        private readonly IKdyImgSaveService _kdyImgSaveService;
        private readonly IWeiBoFileService _weiBoFileService;
        private readonly INormalFileService _normalFileService;

        public KdyImgController(IKdyImgSaveService kdyImgSaveService, IWeiBoFileService weiBoFileService, INormalFileService normalFileService)
        {
            _kdyImgSaveService = kdyImgSaveService;
            _weiBoFileService = weiBoFileService;
            _normalFileService = normalFileService;
        }

        public async Task<IActionResult> Index(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return Content("Url Is");
            }

            var ext = Path.GetExtension(url);

            var input = new NormalFileInput("https://niupic.com/index/upload/process", "image_field",
                "data", $"{DateTime.Now.Ticks:x}{ext}", url);
            var result = await _normalFileService.PostFile(input);


            // var input = new BaseKdyFileInput(url);
            //var result = await _weiBoFileService.PostFile(input);
            return Json(result);
        }

        /// <summary>
        /// 获取图床图片
        /// </summary>
        /// <returns></returns>
        [Route("Path/{id}")]
        public async Task<IActionResult> GetImgAsync(long id)
        {
            var url = await _kdyImgSaveService.GetImageByImgId(id);
            return Redirect(url);
        }
    }
}
