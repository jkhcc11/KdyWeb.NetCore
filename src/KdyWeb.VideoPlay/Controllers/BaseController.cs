using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KdyWeb.VideoPlay.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [AllowAnonymous]
    public class BaseController : Controller
    {
        /// <summary>
        /// 转JsonStr
        /// </summary>
        /// <returns></returns>
        protected string ToJsonStr<T>(T input)
        {
            var setting = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(input, setting);
        }
    }
}
