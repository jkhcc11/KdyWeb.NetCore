using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 基础control
    /// </summary>
    [Route("api/cloud-parse/v1/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
