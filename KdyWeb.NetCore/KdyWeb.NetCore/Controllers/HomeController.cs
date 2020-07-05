using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.Dto.Job;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KdyWeb.NetCore.Models;
using KdyWeb.Service.Job;

namespace KdyWeb.NetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HomeController(ILogger<HomeController> logger, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
        }

        public IActionResult Index()
        {
            var input = new SendEmailInput()
            {
                Email = "154@qq.com",
                Content = "jfiejfieji"
            };
            _backgroundJobClient.Enqueue<SendEmailQueue>(a => a.Execute(input));

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
