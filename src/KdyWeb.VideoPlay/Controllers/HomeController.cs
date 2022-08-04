using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KdyWeb.VideoPlay.Controllers
{

    public class HomeController : BaseController
    {
        private readonly ILoginUserInfo _loginUserInfo;

        public HomeController(ILoginUserInfo loginUserInfo)
        {
            _loginUserInfo = loginUserInfo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [Authorize]
        public IActionResult UserInfo()
        {
            return Json(_loginUserInfo);
        }

        [Authorize(AuthorizationConst.NormalPolicyName.NormalPolicy)]
        public IActionResult NormalInfo()
        {
            return Json(_loginUserInfo);
        }

        [Authorize(AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public IActionResult SupperInfo()
        {
            return Json(_loginUserInfo);
        }
    }
}
