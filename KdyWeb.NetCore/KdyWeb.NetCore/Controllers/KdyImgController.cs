using System;
using System.IO;
using System.Threading.Tasks;
using KdyWeb.Dto.KdyFile;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.KdyFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.NetCore.Controllers
{
    /// <summary>
    /// 图床
    /// </summary>
    [Route("kdyImg")]
    [AllowAnonymous]
    public class KdyImgController : Controller
    {
        private readonly IKdyImgSaveService _kdyImgSaveService;
        private readonly IWeiBoFileService _weiBoFileService;
        private readonly IDouBanWebInfoService _douBanWebInfoService;

        public KdyImgController(IKdyImgSaveService kdyImgSaveService, IWeiBoFileService weiBoFileService, IDouBanWebInfoService douBanWebInfoService)
        {
            _kdyImgSaveService = kdyImgSaveService;
            _weiBoFileService = weiBoFileService;
            _douBanWebInfoService = douBanWebInfoService;
        }

        public async Task<IActionResult> Index(string url)
        {
            return Content("tttt");
            //if (string.IsNullOrEmpty(url))
            //{
            //    await _douBanWebInfoService.GetInfoBySubjectId("111");
            //    return Content("Url Is");
            //}

            //var result = await _kdyImgSaveService.PostFileByUrl(url);
            //return Json(result);
        }

        /// <summary>
        /// 获取微博cookie
        /// </summary>
        /// <returns></returns>
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
