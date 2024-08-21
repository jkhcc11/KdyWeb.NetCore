using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Test Debug");
        }
    }
}
