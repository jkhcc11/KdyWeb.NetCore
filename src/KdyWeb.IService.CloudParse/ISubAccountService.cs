using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
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
        /// <param name="oldSubAccountInfo">兼容旧版使用 xxxx_id</param>
        /// <returns></returns>
        Task<CloudParseUserChildrenCacheItem> GetSubAccountCacheAsync(string oldSubAccountInfo);

        /// <summary>
        /// 获取子账号缓存
        /// </summary>
        /// <returns></returns>
        Task<CloudParseUserChildrenCacheItem> GetSubAccountCacheAsync(long subAccountId);

        /// <summary>
        /// 获取所有Cookie类型缓存
        /// </summary>
        /// <returns></returns>
        Task<List<CloudParseCookieTypeCacheItem>> GetAllCookieTypeCacheAsync();

        /// <summary>
        /// 清楚子账号缓存
        /// </summary>
        /// <returns></returns>
        Task ClearSubAccountCacheAsync(long subAccountId);

        /// <summary>
        /// 清楚所有Cookie类型缓存
        /// </summary>
        /// <returns></returns>
        Task ClearAllCookieTypeCacheAsync();

        /// <summary>
        /// 单独更新子账号cookie信息
        /// </summary>
        /// <param name="subAccountId">子账号信息</param>
        /// <param name="cookieInfo">最新cookie 不一定是cookie</param>
        /// <remarks>
        /// 有些token啥的 需要在请求中更新
        /// </remarks>
        /// <returns></returns>
        Task UpdateSubAccountInfoAsync(long subAccountId, string cookieInfo);

        /// <summary>
        /// 获取用户信息缓存
        /// </summary>
        /// <returns></returns>
        Task<GetParseUserInfoDto> GetUserInfoCacheAsync(long userId);


        /// <summary>
        /// 清除用户信息缓存
        /// </summary>
        /// <returns></returns>
        Task<bool> ClearUserInfoCacheAsync(long userId);

        /// <summary>
        /// 根据用户信息获取业务标识类型
        /// </summary>
        /// <param name="userInfo">xxx_xxx 旧    xxxxxxx 新</param>
        /// <param name="isNew">是否新版</param>
        /// <returns></returns>
        Task<string> GetBusinessFlagByUserIdAsync(string userInfo,bool isNew);
    }
}
