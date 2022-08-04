using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.VideoConverts;

namespace KdyWeb.IService.VideoConverts
{
    /// <summary>
    /// 转码订单 服务接口
    /// </summary>
    public interface IConvertOrderService : IKdyService
    {
        /// <summary>
        /// 审批订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> AuditOrderAsync(AuditOrderInput input);

        /// <summary>
        /// 驳回订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> RejectedOrderAsync(RejectedOrderInput input);

        /// <summary>
        /// 查询转码订单列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryOrderListWithAdminDto>>> QueryOrderListWithAdminAsync(QueryOrderListWithAdminInput input);

        /// <summary>
        /// 查询我的转码订单列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryMeOrderListDto>>> QueryMeOrderListAsync(QueryMeOrderListInput input);
    }
}
