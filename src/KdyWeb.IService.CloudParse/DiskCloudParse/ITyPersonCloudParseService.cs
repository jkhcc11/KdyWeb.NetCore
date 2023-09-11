using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.Dto.HttpCapture.KdyCloudParse.Cache;

namespace KdyWeb.IService.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 天翼网盘解析 接口
    /// </summary>
    /// <remarks>
    /// 适用于个人云 https://cloud.189.cn/web/login.html
    /// </remarks>
    public interface ITyPersonCloudParseService : IKdyCloudParseService<string, BaseResultOut>
    {
        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
        Task<TyLoginInfoCache> GetLoginInfoAsync();

        /// <summary>
        /// 批量改名
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input);
    }
}
