using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;

namespace KdyWeb.IService
{
    /// <summary>
    /// 用户Token 服务接口
    /// </summary>
    public interface IKdyUserTokenService : IKdyService
    {
        /// <summary>
        /// 用户Token校验
        /// </summary>
        /// <remarks>
        /// 注销后Token缓存 存在则注销
        /// </remarks>
        /// <returns></returns>
        Task<bool> UserTokenValidAsync(string token);
        //Task<bool> UserIdIsValidAsync(long userId);
    }
}
