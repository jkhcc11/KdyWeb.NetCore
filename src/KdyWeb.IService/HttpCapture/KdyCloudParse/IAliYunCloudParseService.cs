using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;

namespace KdyWeb.IService.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 阿里云盘解析 接口 <br/>
    /// https://www.aliyundrive.com
    /// </summary>
    public interface IAliYunCloudParseService : IKdyCloudParseService<string, BaseResultOut, DownTypeExtData>
    {
        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<AilYunCloudTokenCache>> GetLoginInfoAsync();

        ///// <summary>
        ///// 清空子账号token缓存
        ///// </summary>
        ///// <remarks>
        /////  更换子账号token时需要清空
        ///// </remarks>
        ///// <returns></returns>
        //Task<KdyResult> ClearTokenCacheAsync(int childrenId);

        /// <summary>
        /// 根据文件名获取文件信息
        /// </summary>
        /// <param name="fileName">网盘文件名称</param>
        /// <returns></returns>
        Task<KdyResult<BaseResultOut>> GetFileInfoByFileNameAsync(string fileName);

        /// <summary>
        /// 批量改名
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input);
    }
}
