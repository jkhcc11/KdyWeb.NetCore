using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.IService;
using Microsoft.Extensions.Caching.Distributed;

namespace KdyWeb.Service
{
    /// <summary>
    /// 用户Token 服务接口
    /// </summary>
    public class KdyUserTokenService : BaseKdyService, IKdyUserTokenService
    {
        public KdyUserTokenService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 用户Token校验
        /// </summary>
        /// <remarks>
        /// 注销后Token缓存 存在则注销
        /// </remarks>
        /// <returns></returns>
        public async Task<bool> UserTokenValidAsync(string token)
        {
            var cacheKey = $"{CacheKeyConst.KdyCacheName.UserLogoutKey}:{token}";
            var cacheV = await KdyRedisCache.GetCache().GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cacheV);
        }
    }
}
