using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;

namespace KdyWeb.IService.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 阿里云盘解析 接口 <br/>
    /// https://www.aliyundrive.com
    /// </summary>
    public interface IAliYunCloudParseService : IKdyCloudParseService<string, BaseResultOut, string>
    {
        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<AilYunCloudTokenCache>> GetLoginInfoAsync();

        /// <summary>
        /// 批量改名
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input);
    }
}
