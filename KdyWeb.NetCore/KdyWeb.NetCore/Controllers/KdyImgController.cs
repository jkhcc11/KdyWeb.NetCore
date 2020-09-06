﻿using System.Threading.Tasks;
using KdyWeb.IService.ImageSave;
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

        public KdyImgController(IKdyImgSaveService kdyImgSaveService)
        {
            _kdyImgSaveService = kdyImgSaveService;
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
