using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse.CacheItem;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService;
using KdyWeb.IService.CloudParse;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 子账号服务
    /// </summary>
    public class SubAccountService : BaseKdyService, ISubAccountService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IKdyRepository<CloudParseUserChildren, long> _cloudParseUserChildrenRepository;
        private readonly IKdyRepository<CloudParseCookieType, long> _cloudParseCookieTypeRepository;

        public SubAccountService(IUnitOfWork unitOfWork, IDistributedCache distributedCache,
            IKdyRepository<CloudParseUserChildren, long> cloudParseUserChildrenRepository,
            IKdyRepository<CloudParseCookieType, long> cloudParseCookieTypeRepository) : base(unitOfWork)
        {
            _distributedCache = distributedCache;
            _cloudParseUserChildrenRepository = cloudParseUserChildrenRepository;
            _cloudParseCookieTypeRepository = cloudParseCookieTypeRepository;
        }

        /// <summary>
        /// 获取子账号缓存
        /// </summary>
        /// <returns></returns>
        public async Task<CloudParseUserChildrenCacheItem> GetSubAccountCacheAsync(long subAccountId)
        {
            var dbSubAccount = await _cloudParseUserChildrenRepository.FirstOrDefaultAsync(a => a.Id == subAccountId);
            if (dbSubAccount == null)
            {
                return new CloudParseUserChildrenCacheItem()
                {
                    Id = subAccountId
                };
            }

            var result = dbSubAccount.MapToExt<CloudParseUserChildrenCacheItem>();
            result.ShowName = result.Alias.IsEmptyExt() ?
                $"{LoginUserInfo.UserName}-{result.Id.ToString().Substring(0, 3)}" : result.Alias;

            return result;
        }

        /// <summary>
        /// 获取所有Cookie类型缓存
        /// </summary>
        /// <returns></returns>
        public async Task<List<CloudParseCookieTypeCacheItem>> GetAllCookieTypeCacheAsync()
        {
            var cache = await _distributedCache.GetValueAsync<List<CloudParseCookieTypeCacheItem>>(CacheKeyConst
                .KdyCacheName.AllCookieTypeKey);
            if (cache.Any())
            {
                return cache;
            }

            var dbList = await _cloudParseCookieTypeRepository.GetQuery()
                .ToListAsync();
            if (dbList.Any() == false)
            {
                return new List<CloudParseCookieTypeCacheItem>();
            }

            cache = dbList.MapToListExt<CloudParseCookieTypeCacheItem>().ToList();
            await _distributedCache.SetValueAsync(CacheKeyConst
                .KdyCacheName.AllCookieTypeKey, cache, TimeSpan.FromMinutes(30));

            return cache;
        }
    }
}
