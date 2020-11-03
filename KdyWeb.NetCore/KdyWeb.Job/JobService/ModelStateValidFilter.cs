using System.Linq;
using KdyWeb.BaseInterface.BaseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KdyWeb.Job.JobService
{
    /// <summary>
    /// ModelState 入参校验
    /// </summary>
    public class ModelStateValidFilter : IActionFilter
    {
        /// <summary>
        /// 执行action后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 执行action前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            //校验参数失败
            var errorMsg = context.ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();
            var errMsg = string.IsNullOrWhiteSpace(errorMsg) ? "参数校验失败，请检查" : errorMsg;
            var result = KdyResult.Error(KdyResultCode.Error, errMsg);
            context.Result = new OkObjectResult(result);
        }
    }
}
