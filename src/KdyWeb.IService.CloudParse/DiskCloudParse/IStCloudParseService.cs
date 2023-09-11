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
    /// 盛天网盘解析 接口
    /// </summary>
    public interface IStCloudParseService : IKdyCloudParseService<string, BaseResultOut>
    {
        /// <summary>
        /// 离线下载
        /// </summary>
        /// <param name="url">待下载Url</param>
        /// <returns></returns>
        Task<KdyResult<string>> AddCloudDownloadAsync(string url);

        /// <summary>
        /// 离线下载状态查询
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<StCloudDownloadListOut>>> QueryCloudDownloadAsync(BaseQueryInput<string> input);

        /// <summary>
        /// 批量改名
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input);
    }
}
