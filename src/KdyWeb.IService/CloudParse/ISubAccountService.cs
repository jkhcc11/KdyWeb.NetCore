using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse.CacheItem;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// 子账号服务 接口
    /// </summary>
    public interface ISubAccountService : IKdyService
    {
        /// <summary>
        /// 获取子账号缓存
        /// </summary>
        /// <returns></returns>
        Task<CloudParseUserChildrenCacheItem> GetSubAccountCacheAsync(long subAccountId);
    }
}
