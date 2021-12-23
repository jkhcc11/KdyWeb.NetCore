using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Cache;

namespace KdyWeb.IService.CrossRequest
{
    /// <summary>
    /// 跨域访问 服务接口
    /// </summary>
    public interface ICrossRequestService : IKdyService
    {
        /// <summary>
        /// 获取跨域访问Token
        /// </summary>
        /// <returns></returns>
        Task<CrossTokenCacheItem> GetCrossTokenAsync();

        /// <summary>
        /// 获取授权中心管理Token
        /// </summary>
        /// <returns></returns>
        Task<CrossTokenCacheItem> GetAuthCenterMgrTokenAsync();

        /// <summary>
        /// 通过用户名或邮箱获取Token
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<CrossTokenCacheItem>> GetAccessTokenByUserNameOrEmailAsync(string userNameOrEmail, string pwd);
    }
}
