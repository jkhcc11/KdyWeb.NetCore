using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Out;

namespace KdyWeb.IService.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 天翼企业云网盘解析 接口
    /// </summary>
    /// <remarks>
    /// 适用于企业云 https://b.cloud.189.cn/
    /// </remarks>
    public interface ITyCropCloudParseService : IKdyCloudParseService<string, BaseResultOut, string>
    {
        /// <summary>
        /// 获取当前用户加入的企业云列表
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetCropListAsync();
    }
}
