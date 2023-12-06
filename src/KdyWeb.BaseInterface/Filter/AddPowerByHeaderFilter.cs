using Microsoft.AspNetCore.Mvc.Filters;

namespace KdyWeb.BaseInterface.Filter
{
    /// <summary>
    /// 添加版权
    /// </summary>
    public class AddPowerByHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // 在响应头中添加自定义的PowerBy头部
            context.HttpContext.Response.Headers.Add("PowerBy", "VGdfemN5MjAyMw==");
            base.OnActionExecuted(context);
        }
    }
}
