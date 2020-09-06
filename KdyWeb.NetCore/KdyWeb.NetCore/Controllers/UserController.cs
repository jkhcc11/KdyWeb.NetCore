using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.KdyRedis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.NetCore.Controllers
{

    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly IKdyRedisCache _kdyRedisCache;

        public UserController(IKdyRedisCache kdyRedisCache)
        {
            _kdyRedisCache = kdyRedisCache;
        }

        public IActionResult Login()
        {
            var key = "fea7c9ccb6c24cf8ad3f8b1d224a99f9@185";
            _kdyRedisCache.GetDb(1).StringSet(key, DateTime.Now.ToString());

            Response.Cookies.Append(KdyBaseConst.CookieKey, key, new CookieOptions()
            {
               // MaxAge = TimeSpan.FromHours(1)
            });

            //await HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(claimsIdentity),
            //    authProperties);

            return Content("未登录");
        }
    }
}
