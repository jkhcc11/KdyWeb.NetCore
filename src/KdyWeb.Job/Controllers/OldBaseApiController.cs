using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 旧版本兼容Api 基础control
    /// </summary>
    [Route("api/old/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public abstract class OldBaseApiController: ControllerBase
    {
    }
}
