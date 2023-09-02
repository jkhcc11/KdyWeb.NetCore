using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 网盘基础
    /// </summary>
    public abstract class BaseCloudDiskApiController : BaseApiController
    {
        /// <summary>
        /// 获取当前网盘Cookie类型
        /// </summary>
        /// <returns></returns>
        public abstract Task<KdyResult<string>> GetCurrentCookieTypeAsync();
    }
}
