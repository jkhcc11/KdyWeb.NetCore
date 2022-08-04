using KdyWeb.BaseInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.VideoPlay.Controllers.ApiController
{
    /// <summary>
    /// Api控制器 基类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.CrossPolicy)]
    public class BaseApiController : ControllerBase
    {

    }
}
