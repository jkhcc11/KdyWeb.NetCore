using System.Threading.Tasks;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.HttpCapture.KdyCloudParse.Cache;

namespace KdyWeb.IService.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 天翼网盘解析 接口
    /// </summary>
    /// <remarks>
    /// 适用于个人云 https://cloud.189.cn/web/login.html
    /// </remarks>
    public interface ITyPersonCloudParseService : IKdyCloudParseService<string, BaseResultOut, string>
    {
        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
        Task<TyLoginInfoCache> GetLoginInfoAsync();
    }
}
