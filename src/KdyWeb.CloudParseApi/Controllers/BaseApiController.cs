using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse.CacheItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 基础control
    /// </summary>
    [Route("api/cloud-parse/v1/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.NormalPolicy)]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// 检查子账号权限
        /// </summary>
        /// <remarks>
        ///  1、子账号不是当前登录用户 异常 <br/>
        ///  2、没cookie 异常
        /// </remarks>
        /// <param name="loginUserInfo">登录信息</param>
        /// <param name="subAccountCacheItem">子账号缓存</param>
        protected void CheckSubAccountAuth(ILoginUserInfo loginUserInfo, CloudParseUserChildrenCacheItem subAccountCacheItem)
        {
            if (loginUserInfo.IsSuperAdmin == false &&
                subAccountCacheItem.UserId != loginUserInfo.GetUserId())
            {
                var userIdStr = loginUserInfo.GetUserId().ToString();
                if (string.IsNullOrEmpty(subAccountCacheItem.RelationalUserIds) == false &&
                    subAccountCacheItem.RelationalUserIds.Contains(userIdStr))
                {
                    //该用户关联了子账号
                    return;
                }

                throw new KdyCustomException("参数错误,无效请求02");
            }

            if (string.IsNullOrEmpty(subAccountCacheItem.CookieInfo))
            {
                throw new KdyCustomException("参数错误,无效请求01");
            }
        }
    }
}
