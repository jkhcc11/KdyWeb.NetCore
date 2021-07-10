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

        // GET: api/<HomeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {

            var test = new
            {
                Name = "1",
                Age = 10
            };
            _logger.LogInformation("User {test} ffff",test);

            // _backgroundJobClient.Enqueue(() => Test());
            return new string[] { "value1", "value2" };
        }

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HomeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public string Test()
        {
            return "测试";
        }
    }
}
