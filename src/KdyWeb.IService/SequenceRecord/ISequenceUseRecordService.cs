using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SequenceRecord;

namespace KdyWeb.IService.SequenceRecord
{
    /// <summary>
    /// 接龙使用记录 服务接口
    /// </summary>
    public interface ISequenceUseRecordService : IKdyService
    {
        /// <summary>
        /// 根据腾讯接龙记录创建接龙记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<IList<QueryPageSequenceUseRecordDto>>> CreateSequenceUseRecordByTxDocAsync(CreateSequenceUseRecordByTxDocInput input);

        /// <summary>
        /// 分页获取接龙使用记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryPageSequenceUseRecordDto>>> QueryPageSequenceUseRecordAsync(QueryPageSequenceUseRecordInput input);

        /// <summary>
        /// 分页获取用户接龙列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryPageSequenceUserRecordDto>>> QueryPageSequenceUserRecordAsync(QueryPageSequenceUserRecordInput input);

        /// <summary>
        /// 更新接龙使用记录
        /// </summary>
        /// <remarks>
        /// 人工干预调整奖励类型和价格
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> UpdateSequenceUseRecordAsync(UpdateSequenceUseRecordInput input);

        /// <summary>
        /// 获取所有用户接龙记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<SelectedItemOut>>> GetAllUserRecordAsync();
    }
}
