using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.VideoPlay.Controllers
{
    /// <summary>
    /// Api控制器 基类
    /// </summary>
    [Route("api/parse/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
