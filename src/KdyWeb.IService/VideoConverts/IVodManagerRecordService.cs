using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.IService.VideoConverts
{
    /// <summary>
    /// 影片管理者记录 服务接口
    /// </summary>
    public interface IVodManagerRecordService : IKdyService
    {
        /// <summary>
        /// 查询影片管理者记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryVodManagerRecordDto>>> QueryVodManagerRecordAsync(QueryVodManagerRecordInput input);

        /// <summary>
        /// 更新记录实际金额
        /// </summary>
        /// <remarks>
        ///  有些积分需要人工核对
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> UpdateRecordActualAmountAsync(UpdateRecordActualAmountInput input);

        /// <summary>
        /// 批量结算记录
        /// </summary>
        /// <remarks>
        ///  只能全部为有效数据才结算成功
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> BatchCheckoutRecordAsync(BatchDeleteForLongKeyInput input);

        /// <summary>
        /// 创建影片管理者记录
        /// </summary>
        /// <remarks>
        /// 对内服务
        /// </remarks>
        /// <param name="recordType">记录类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="businessId">业务id</param>
        /// <param name="checkoutAmount">结算金额</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        Task CreateVodManagerRecordAsync(VodManagerRecordType recordType
            , long userId, long businessId, decimal? checkoutAmount = null, string remark = "");
    }
}
