using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// ServerCookie 服务接口
    /// </summary>
    public interface IServerCookieService : IKdyService
    {
        /// <summary>
        /// 查询服务器Cookie列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryServerCookieDto>>> QueryServerCookieAsync(QueryServerCookieInput input);

        /// <summary>
        /// 创建和更新服务器Cookie
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateServerCookieAsync(CreateAndUpdateServerCookieInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchDeleteForLongKeyInput input);

        /// <summary>
        /// 审批
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> AuditAsync(long id);
    }
}
