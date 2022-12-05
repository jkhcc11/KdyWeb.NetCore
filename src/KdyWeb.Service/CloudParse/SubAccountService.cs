using System.Threading.Tasks;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse.CacheItem;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Utility;
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

        public SubAccountService(IUnitOfWork unitOfWork, IDistributedCache distributedCache,
            IKdyRepository<CloudParseUserChildren, long> cloudParseUserChildrenRepository) : base(unitOfWork)
        {
            _distributedCache = distributedCache;
            _cloudParseUserChildrenRepository = cloudParseUserChildrenRepository;
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
    }
}
