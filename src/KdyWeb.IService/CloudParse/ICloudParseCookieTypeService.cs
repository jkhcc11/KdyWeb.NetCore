using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// Cookie类型 服务接口
    /// </summary>
    public interface ICloudParseCookieTypeService : IKdyService
    {
        /// <summary>
        /// 查询Cookie类型列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryCookieTypeDto>>> QueryCookieTypeAsync(QueryCookieTypeInput input);

        /// <summary>
        /// 创建和更新Cookie类型
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateCookieTypeAsync(CreateAndUpdateCookieTypeInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchDeleteForLongKeyInput input);
    }
}
