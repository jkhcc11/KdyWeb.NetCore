using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.NetCore.Controllers.ApiController
{
    /// <summary>
    /// Api基类
    /// </summary>
    [Route("api/kdy-img/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public abstract class BaseImgApiController : Controller
    {

    }
}
