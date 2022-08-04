using System.Collections.Generic;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KdyWeb.Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IBackgroundJobClient backgroundJobClient, ILogger<HomeController> logger)
        {
            _backgroundJobClient = backgroundJobClient;
            _logger = logger;
        }

        public string Test()
        {
            return "测试";
        }
    }
}
