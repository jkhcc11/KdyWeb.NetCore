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
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;

        public HomeController(ILogger<HomeController> logger, IKdyRequestCommon kdyRequestCommon, IMinIoFileService minIoFileService, IBackgroundJobClient backgroundJobClient, IKdyImgSaveService kdyImgSaveService, IKdyRedisCache redisCache, IKdyRequestClientCommon kdyRequestClientCommon)
        {
            _logger = logger;
            _kdyRequestCommon = kdyRequestCommon;
            _minIoFileService = minIoFileService;
            _backgroundJobClient = backgroundJobClient;
            _kdyImgSaveService = kdyImgSaveService;
            _redisCache = redisCache;
            _kdyRequestClientCommon = kdyRequestClientCommon;
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

        /// <summary>
        /// 测试Cookie
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> TestCookie()
        {
            //var reqInput=new KdyRequestCommonInput("https://pan.baidu.com/s/11ULLsXTPA4hG7H7_ykoGjA#list/path=%2F", HttpMethod.Get)
            //{
            //    Cookie = "BIDUPSID=0F26932E0EE9CBCB41A0F46179D0B2D9;PSTM=1589612958;BAIDUID=0F26932E0EE9CBCBDDBC688CA551CCB5:FG=1;PANWEB=1;csrfToken=kn627t6zIaJJ1fMbCsecb9on;pan_login_way=1;delPer=0;ZD_ENTRY=baidu;PSINO=6;Hm_lvt_7a3960b6f067eb0085b7f96ff5e660b0=1597498191,1598153994;recommendTime=mac2020-09-02%2015%3A38%3A00;BDUSS=oyaWIzaUZRem9WQXY1dGVROXBWTm05VkZreWhXeWhEaEx2dGN-VklzcWEzM2xmSVFBQUFBJCQAAAAAAAAAAAEAAACWkdLta2R5NjY2MjAxOQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJpSUl-aUlJfLX;BDUSS_BFESS=oyaWIzaUZRem9WQXY1dGVROXBWTm05VkZreWhXeWhEaEx2dGN-VklzcWEzM2xmSVFBQUFBJCQAAAAAAAAAAAEAAACWkdLta2R5NjY2MjAxOQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJpSUl-aUlJfLX; STOKEN=08184247673d3d11e25de140d6368aa325842b04f943bcb4bdce160463ba1e95;SCRC=89a00e3c5d311c002ed7dd91cfff6940;H_PS_PSSID=7541_32606_1439_32328_31660_32045_32679_32117_31321;BDCLND=PsvyJNzBn0KLG5GxIGRJOOG3dhbJ7G5FLOEkydU%2BTic%3D;Hm_lpvt_7a3960b6f067eb0085b7f96ff5e660b0=1599486499;PANPSC=2861820099159283262%3ACU2JWesajwDlmux2abJQFky7txDAjOFZT71QYf2NbKjPvHE4IT5LETgOvhx7XIhXomkDDRqX579TMx5%2FR2wsyFDrhfWzVbTlBldOXVdDlbBKvICFQelbNKL5jVfu52v0baC532AuNi1GiBt6rgD9UiZTnz2RR6t%2BG5E6QgrAKeR89Veg5I7xCzuoAeCl9TBG"
            //};
            var reqInput = new KdyRequestCommonInput("https://passport2.chaoxing.com/fanyalogin", HttpMethod.Post)
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36",
                Referer = "https://passport2.chaoxing.com/login?newversion=true&refer=http%3A%2F%2Fpan-yz.chaoxing.com%2F",
                ExtData = new KdyRequestCommonExtInput()
                {
                    IsAjax = true,
                    PostData = "fid=-1&uname=1549930804%40qq.com&password=Qk9IRUpJTkcxMjM%3D&refer=http%253A%252F%252Fpan-yz.chaoxing.com%252F&t=true"
                }
            };

            var result = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (result.IsSuccess == false)
            {
                return Content(result.ErrMsg);
            }

            return Json(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
