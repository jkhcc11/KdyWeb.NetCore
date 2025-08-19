using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SequenceRecord;

namespace KdyWeb.IService.SequenceRecord
{
    /// <summary>
    /// 奖励用户配置 服务接口
    /// </summary>
    public interface IGiftUserConfigService : IKdyService
    {
        /// <summary>
        /// 分页查询奖励用户配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryPageGiftUserConfigDto>>> QueryPageGiftUserConfigAsync(QueryPageGiftUserConfigInput input);

        /// <summary>
        /// 创建奖励用户配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateGiftUserConfigAsync(CreateGiftUserConfigInput input);

        /// <summary>
        /// 修改奖励用户配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifyGiftUserConfigAsync(ModifyGiftUserConfigInput input);

        /// <summary>
        /// 禁用奖励用户配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BanGiftUserConfigAsync(long configId);
    }
}
