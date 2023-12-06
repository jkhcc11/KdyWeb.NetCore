using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KdyWeb.BaseInterface.Filter
{
    /// <summary>
    /// 校验请求头部
    /// </summary>
    /// <remarks>
    ///   检查请求头部是否包含Sec-Fetch-Dest和Sec-Fetch-Mode
    /// </remarks>
    public class ValidateFetchHeadersAttribute : ActionFilterAttribute
    {
        public const string FetchDestValue = "iframe";
        public const string FetchModeValue = "navigate";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var secFetchDest = context.HttpContext.Request.Headers["Sec-Fetch-Dest"].ToString();
            var secFetchMode = context.HttpContext.Request.Headers["Sec-Fetch-Mode"].ToString();

            if (secFetchDest != FetchDestValue || secFetchMode != FetchModeValue)
            {
                //403
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            base.OnActionExecuting(context);
        }
    }
}
