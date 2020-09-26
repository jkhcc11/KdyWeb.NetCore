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
    [Route("kdyImg")]
    public class KdyImgController : Controller
    {
        private readonly IKdyImgSaveService _kdyImgSaveService;
        private readonly IWeiBoFileService _weiBoFileService;

        public KdyImgController(IKdyImgSaveService kdyImgSaveService, IWeiBoFileService weiBoFileService)
        {
            _kdyImgSaveService = kdyImgSaveService;
            _weiBoFileService = weiBoFileService;
        }

        public async Task<IActionResult> Index(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return Content("Url Is");
            }

            var result = await _kdyImgSaveService.PostFileByUrl(url);
            return Json(result);
        }

        [Route("init")]
        public async Task<IActionResult> InitWebBo()
        {
            var result = await _weiBoFileService.GetLoginCookie();
            return Json(result);
        }

        /// <summary>
        /// 获取图床图片
        /// </summary>
        /// <returns></returns>
        [Route("path/{id}")]
        public async Task<IActionResult> GetImgAsync(long id)
        {
            var url = await _kdyImgSaveService.GetImageByImgId(id);
            return Redirect(url);
        }
    }
}
