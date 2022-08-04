using KdyWeb.BaseInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 旧版本兼容Api 基础control
    /// </summary>
    /// <remarks>
    /// 默认超管权限
    /// </remarks>
    [Route("api/old/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
    public abstract class OldBaseApiController: ControllerBase
    {
    }
}
