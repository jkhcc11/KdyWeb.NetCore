using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;

namespace KdyWeb.IService.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 腾讯云分享解析 接口
    /// </summary>
    public interface ITShareCloudParseService : IKdyCloudParseService<string, BaseResultOut>
    {
        /// <summary>
        /// 批量改名
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input);
    }
}
