using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;

namespace KdyWeb.IService.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 天翼家庭网盘解析 接口
    /// </summary>
    /// <remarks>
    /// 适用于家庭云 https://h5.cloud.189.cn/main.html#/family
    /// </remarks>
    public interface ITyFamilyCloudParseService : IKdyCloudParseService<string, BaseResultOut>
    {
        /// <summary>
        /// 获取当前用户加入的家庭云列表
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetFamilyListAsync();

        /// <summary>
        /// 批量改名
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input);

        /// <summary>
        /// 同步名称和Id映射
        /// </summary>
        /// <remarks>
        /// 没有搜索功能,只能映射
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> SyncNameIdMapAsync(List<BatchUpdateNameInput> input);
    }
}
