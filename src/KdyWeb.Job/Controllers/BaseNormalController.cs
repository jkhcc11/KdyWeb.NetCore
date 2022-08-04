using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 默认Controller基类
    /// </summary>
    [Route("api/kdy-normal/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "normal")]
    public abstract class BaseNormalController : Controller
    {

    }
}
