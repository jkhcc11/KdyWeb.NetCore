using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.CloudParse.CacheItem;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 子账号服务
    /// </summary>
    public class SubAccountService : BaseKdyService, ISubAccountService
    {
        private readonly IKdyRepository<CloudParseUser, long> _cloudParseUserRepository;
        private readonly IKdyRepository<CloudParseUserChildren, long> _cloudParseUserChildrenRepository;
        private readonly IKdyRepository<CloudParseCookieType, long> _cloudParseCookieTypeRepository;

        public SubAccountService(IUnitOfWork unitOfWork,
            IKdyRepository<CloudParseUserChildren, long> cloudParseUserChildrenRepository,
            IKdyRepository<CloudParseCookieType, long> cloudParseCookieTypeRepository,
            IKdyRepository<CloudParseUser, long> cloudParseUserRepository) : base(unitOfWork)
        {
            _cloudParseUserChildrenRepository = cloudParseUserChildrenRepository;
            _cloudParseCookieTypeRepository = cloudParseCookieTypeRepository;
            _cloudParseUserRepository = cloudParseUserRepository;
        }

        /// <summary>
        /// 获取子账号缓存
        /// </summary>
        /// <param name="oldSubAccountInfo">兼容旧版使用 xxxx_id</param>
        /// <returns></returns>
        public async Task<CloudParseUserChildrenCacheItem> GetSubAccountCacheAsync(string oldSubAccountInfo)
        {
            //todo:如果旧信息不存在会一直请求db
            var dbSubAccount = await _cloudParseUserChildrenRepository
                  .FirstOrDefaultAsync(a => a.OldSubAccountInfo.Contains(oldSubAccountInfo));
            var result = dbSubAccount.MapToExt<CloudParseUserChildrenCacheItem>();
            if (result == null)
            {
                return default;
            }

            result.ShowName = result.Alias.IsEmptyExt() ?
                  $"{LoginUserInfo.UserName}-{result.Id.ToString().Substring(0, 3)}" : result.Alias;
            var cacheKey = GetSubAccountCacheKey(dbSubAccount.Id);
            var cacheValue = await GetCacheValueAsync(cacheKey, () =>
            {
                return Task.FromResult(result);
            }, TimeSpan.FromHours(12));

            return cacheValue;
        }

        /// <summary>
        /// 获取子账号缓存
        /// </summary>
        /// <returns></returns>
        public async Task<CloudParseUserChildrenCacheItem> GetSubAccountCacheAsync(long subAccountId)
        {
            var cacheKey = GetSubAccountCacheKey(subAccountId);
            var cacheValue = await GetCacheValueAsync(cacheKey, async () =>
            {
                var dbSubAccount = await _cloudParseUserChildrenRepository.FirstOrDefaultAsync(a => a.Id == subAccountId);
                var result = dbSubAccount.MapToExt<CloudParseUserChildrenCacheItem>();
                if (result != null)
                {
                    result.ShowName = result.Alias.IsEmptyExt() ?
                        $"{LoginUserInfo.UserName}-{result.Id.ToString().Substring(0, 3)}" : result.Alias;
                }

                return result;
            }, TimeSpan.FromHours(12));

            return cacheValue;
        }

        /// <summary>
        /// 获取所有Cookie类型缓存
        /// </summary>
        /// <returns></returns>
        public async Task<List<CloudParseCookieTypeCacheItem>> GetAllCookieTypeCacheAsync()
        {
            var cacheValue = await GetCacheValueAsync(CacheKeyConst.KdyCacheName.AllCookieTypeKey, async () =>
            {
                var dbList = await _cloudParseCookieTypeRepository.GetQuery()
                    .ToListAsync();

                return dbList.MapToListExt<CloudParseCookieTypeCacheItem>().ToList();
            }, TimeSpan.FromHours(12));

            return cacheValue;
        }

        /// <summary>
        /// 清楚子账号缓存
        /// </summary>
        /// <returns></returns>
        public async Task ClearSubAccountCacheAsync(long subAccountId)
        {
            var cacheKey = GetSubAccountCacheKey(subAccountId);
            await KdyRedisCache.GetDb(1).KeyDeleteAsync(cacheKey);
        }

        /// <summary>
        /// 清楚所有Cookie类型缓存
        /// </summary>
        /// <returns></returns>
        public async Task ClearAllCookieTypeCacheAsync()
        {
            await KdyRedisCache.GetDb(1).KeyDeleteAsync(CacheKeyConst.KdyCacheName.AllCookieTypeKey);
        }

        /// <summary>
        /// 单独更新子账号cookie信息
        /// </summary>
        /// <param name="subAccountId">子账号信息</param>
        /// <param name="cookieInfo">最新cookie 不一定是cookie</param>
        /// <remarks>
        /// 有些token啥的 需要在请求中更新
        /// </remarks>
        /// <returns></returns>
        public async Task UpdateSubAccountInfoAsync(long subAccountId, string cookieInfo)
        {
            var dbSubAccount = await _cloudParseUserChildrenRepository
                .FirstOrDefaultAsync(a => a.Id == subAccountId);
            dbSubAccount.CookieInfo = cookieInfo;
            _cloudParseUserChildrenRepository.Update(dbSubAccount);
            await UnitOfWork.SaveChangesAsync();

            //清子账号缓存
            var cacheKey = GetSubAccountCacheKey(subAccountId);
            await KdyRedisCache.GetDb(1).KeyDeleteAsync(cacheKey);
        }

        /// <summary>
        /// 获取用户信息缓存
        /// </summary>
        /// <returns></returns>
        public async Task<GetParseUserInfoDto> GetUserInfoCacheAsync(long userId)
        {
            var cacheKey = GetUserInfoCacheKey(userId);
            var cacheValue = await GetCacheValueAsync(cacheKey, async () =>
            {
                var dbUserInfo = await _cloudParseUserRepository.GetQuery()
                    .FirstOrDefaultAsync(a => a.UserId == userId);
                return dbUserInfo.MapToExt<GetParseUserInfoDto>();
            }, TimeSpan.FromHours(12));

            return cacheValue;
        }

        /// <summary>
        /// 清除用户信息缓存
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ClearUserInfoCacheAsync(long userId)
        {
            var cacheKey = GetUserInfoCacheKey(userId);
            return await KdyRedisCache.GetDb(1).KeyDeleteAsync(cacheKey);
        }

        /// <summary>
        /// 根据用户信息获取业务标识类型
        /// </summary>
        /// <param name="userInfo">xxx_xxx 旧    xxxxxxx 新</param>
        /// <param name="isNew">是否新版</param>
        /// <returns></returns>
        public async Task<string> GetBusinessFlagByUserIdAsync(string userInfo, bool isNew)
        {
            CloudParseUserChildrenCacheItem subAccountCacheItem;
            if (isNew)
            {
                long.TryParse(userInfo, out long userId);
                subAccountCacheItem = await GetSubAccountCacheAsync(userId);
            }
            else
            {
                subAccountCacheItem = await GetSubAccountCacheAsync(userInfo);
            }

            if (subAccountCacheItem == null)
            {
                return string.Empty;
            }

            var allCookieType = await GetAllCookieTypeCacheAsync();
            return allCookieType.FirstOrDefault(a => a.Id == subAccountCacheItem.CookieTypeId)?.BusinessFlag;
        }


        /// <summary>
        /// 获取子账号缓存Key
        /// </summary>
        /// <param name="subAccountId">
        /// 类型一：子账号Id 新版 <br/>
        /// 类型二：xxxx_Id 旧版 合并一起
        /// </param>
        /// <returns></returns>
        private string GetSubAccountCacheKey(long subAccountId)
        {
            return $"{CacheKeyConst.KdyCacheName.SubAccountCookieKey}:{subAccountId}";
        }

        private string GetUserInfoCacheKey(long userId)
        {
            return $"{CacheKeyConst.KdyCacheName.UserInfoCacheKey}:{userId}";
        }
    }
}
