using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.KdyFile;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.KdyFile;
using KdyWeb.IService.KdyHttp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KdyWeb.NetCore.Models;
using KdyWeb.Service.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace KdyWeb.NetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
         private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IKdyRequestCommon _kdyRequestCommon;
        private readonly IMinIoFileService _minIoFileService;
        private readonly IKdyImgSaveService _kdyImgSaveService;
        private readonly IKdyRedisCache _redisCache;

        public HomeController(ILogger<HomeController> logger, IKdyRequestCommon kdyRequestCommon, IMinIoFileService minIoFileService, IBackgroundJobClient backgroundJobClient, IKdyImgSaveService kdyImgSaveService, IKdyRedisCache redisCache)
        {
            _logger = logger;
            _kdyRequestCommon = kdyRequestCommon;
            _minIoFileService = minIoFileService;
            _backgroundJobClient = backgroundJobClient;
            _kdyImgSaveService = kdyImgSaveService;
            _redisCache = redisCache;
        }

        public async Task<IActionResult> Index(string url)
        {
           // await _redisCache.GetCache().SetStringAsync("Index", url);

           // await _kdyImgSaveService.Test();
            var emailInput = new SendEmailInput()
            {
                Email = "154@qq.com",
                Content = "jfiejfieji"
            };
            _backgroundJobClient.Enqueue<SendEmailQueue>(a => a.Execute(emailInput));
            if (string.IsNullOrEmpty(url))
            {
                return View();
            }

            var name = $"{DateTime.Now.Ticks:x}.jpg";
            var input = new MinIoFileInput("kdyimg", name, url);

            var result = await _minIoFileService.PostFile(input);
            return Json(result);

            //if (string.IsNullOrEmpty(url))
            //{
            //    url = "https://www.baidu.com";
            //}
            //var reqInput = new KdyRequestCommonInput(url, HttpMethod.Get);
            //var result = await _kdyRequestCommon.SendAsync(reqInput);
            //if (result.IsSuccess == false)
            //{
            //    return Content(result.ErrMsg);
            //}

            //return Content(result.Data);

            return View();
        }

        public async Task<IActionResult> IndexRef()
        {

            var reqInput = new KdyRequestCommonInput("https://zx.itkdd.com/Cloud/Down/IndexV3/zxzj_232/1DE59A1DDCEAC51A64CFCB7BCCC34BB78D497E2E181AE03A515A48F6C851743F/2", HttpMethod.Get)
            {
                Referer = "https://www.zxzj.me/video/2946-1-1.html"
            };

            var result = await _kdyRequestCommon.SendAsync(reqInput);
            if (result.IsSuccess == false)
            {
                return Content(result.ErrMsg);
            }

            return Content(result.Data);
        }

        [AllowAnonymous]
        public async Task<IActionResult> IndexPost()
        {

            var reqInput = new KdyRequestCommonInput("https://api.hcc11.com/Index/Parse?jsonpCallback=success_ck", HttpMethod.Post)
            {
                ExtData = new KdyRequestCommonExtInput()
                {
                    PostData = "EncodeUrl=BE54D56871F7DB29CE0CAE7B88A6C950921399AA14727EE03F0BAEEFF0A7F6659BAA06A1B798B0AD82E0071A11F1FC4E8AC3D15FB2164AD6784EC1B71FDD853C47E2EEB2565A0881BFF816C54A5C9030194A3EA79C72E71B2C12BD6736256E12D9D5D6495C2CE3A9AAB5735F6B05DAFAA0EDAF2CA9BA0604B1184EDB9C2C0F82"
                }
            };

            var result = await _kdyRequestCommon.SendAsync(reqInput);
            if (result.IsSuccess == false)
            {
                return Content(result.ErrMsg);
            }

            return Content(result.Data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
