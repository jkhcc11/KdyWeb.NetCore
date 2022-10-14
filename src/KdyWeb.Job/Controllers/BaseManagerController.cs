using KdyWeb.BaseInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 管理员Controller基类
    /// </summary>
    [Route("api/kdy-manager/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "manager")]
    //[Authorize(Policy = AuthorizationConst.NormalPolicyName.NormalPolicy)]
    [Authorize(Roles = AuthorizationConst.NormalRoleName.VodAdmin + "," + AuthorizationConst.NormalRoleName.SuperAdmin)]
    public abstract class BaseManagerController : Controller
    {

    }
}
