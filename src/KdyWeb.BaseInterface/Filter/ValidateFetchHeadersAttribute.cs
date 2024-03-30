using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            var kdyLog = context.HttpContext.RequestServices.GetService<ILogger>();

            // 这个正则表达式可能需要根据实际情况进行调整
            // 获取User-Agent头部
            var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            var isAppleBrowser = Regex.IsMatch(userAgent, @"\(iP(hone|ad|od).+Version\/[\d\.]+ Mobile\/\S+ Safari\/[\d\.]+$");

            // 如果不是苹果浏览器，则检查Sec-Fetch-Dest和Sec-Fetch-Mode头部
            if (!isAppleBrowser)
            {
                var secFetchDest = context.HttpContext.Request.Headers["Sec-Fetch-Dest"].ToString();
                var secFetchMode = context.HttpContext.Request.Headers["Sec-Fetch-Mode"].ToString();

                if (secFetchDest != FetchDestValue || secFetchMode != FetchModeValue)
                {
                    // 如果不符合预期，则返回403 Forbidden状态码
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return; // 短路请求，不再继续执行后续动作
                }
            }

            if (userAgent.Contains("okhttp"))
            {
                //屏蔽app模拟请求
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            var xRequestedWith = context.HttpContext.Request.Headers["x-requested-with"] + "";
            if (string.IsNullOrEmpty(xRequestedWith) == false &&
                xRequestedWith.Contains("tvbox"))
            {
                //屏蔽tvbox特定
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                kdyLog?.LogWarning("检测到非法访问,RequestedWith:{with}", xRequestedWith);
                return;
            }

            if (string.IsNullOrEmpty(xRequestedWith) == false &&
                xRequestedWith.Contains("XMLHttpRequest") == false)
            {
                kdyLog?.LogError("检测到异常头部请求,RequestedWith:{with}", xRequestedWith);
            }

            base.OnActionExecuting(context);
        }
    }
}
