using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SequenceRecord;

namespace KdyWeb.IService.SequenceRecord
{
    /// <summary>
    /// 场馆配置 服务接口
    /// </summary>
    public interface IVenuesConfigService : IKdyService
    {
        /// <summary>
        /// 分页查询场馆配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryPageVenuesConfigDto>>> QueryPageVenuesConfigAsync(QueryPageVenuesConfigInput input);

        /// <summary>
        /// 创建场馆配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateVenuesConfigAsync(CreateVenuesConfigInput input);

        /// <summary>
        /// 禁用场馆奖励计算
        /// </summary>
        /// <param name="venuesConfigId">场馆配置Id</param>
        /// <returns></returns>
        Task<KdyResult> BanVenuesGiftAsync(long venuesConfigId);
    }
}
