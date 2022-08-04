using KdyWeb.BaseInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 登录Controller基类
    /// </summary>
    [Route("api/kdy-login/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "login")]
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.NormalPolicy)]
    public abstract class BaseLoginController : Controller
    {

    }
}
