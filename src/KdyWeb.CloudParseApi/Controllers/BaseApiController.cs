using KdyWeb.BaseInterface;
using KdyWeb.Dto.CloudParse.CacheItem;
using KdyWeb.Entity.CloudParse.Enum;
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
        ///  1、子账号不是当前登录用户反馈异常
        ///  2、子账号不是指定类型 异常
        /// </remarks>
        /// <param name="userId">用户Id</param>
        /// <param name="subAccountType">子账号类型</param>
        /// <param name="subAccountCacheItem">子账号缓存</param>
        protected void CheckSubAccountAuth(long userId, CloudParseCookieType subAccountType,
            CloudParseUserChildrenCacheItem subAccountCacheItem)
        {
            if (subAccountCacheItem.CookieType != subAccountType)
            {
                throw new KdyCustomException("参数错误,无效请求01");
            }

            if (subAccountCacheItem.UserId != userId)
            {
                throw new KdyCustomException("参数错误,无效请求02");
            }
        }
    }
}
