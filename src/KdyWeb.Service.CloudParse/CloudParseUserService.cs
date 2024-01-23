using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.CloudParse;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.HttpApi.AuthCenter;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using KdyWeb.ICommonService;
using KdyWeb.IService.CloudParse;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 解析平台用户 服务实现
    /// </summary>
    public class CloudParseUserService : BaseKdyService, ICloudParseUserService
    {
        private readonly IAuthCenterHttpApi _authCenterHttpApi;
        private readonly IKdyRepository<CloudParseUser, long> _cloudParseUserRepository;
        private readonly IKdyRepository<CloudParseUserChildren, long> _cloudParseUserChildrenRepository;
        private readonly ISubAccountService _subAccountService;
        private readonly IKdyRepository<ServerCookie, long> _serverCookieRepository;

        public CloudParseUserService(IUnitOfWork unitOfWork, IKdyRepository<CloudParseUser, long> cloudParseUserRepository,
            IKdyRepository<CloudParseUserChildren, long> cloudParseUserChildrenRepository,
            IAuthCenterHttpApi authCenterHttpApi, ISubAccountService subAccountService,
            IKdyRepository<ServerCookie, long> serverCookieRepository) : base(unitOfWork)
        {
            _cloudParseUserRepository = cloudParseUserRepository;
            _cloudParseUserChildrenRepository = cloudParseUserChildrenRepository;
            _authCenterHttpApi = authCenterHttpApi;
            _subAccountService = subAccountService;
            _serverCookieRepository = serverCookieRepository;
        }

        public async Task<KdyResult<GetParseUserInfoDto>> GetParseUserInfoAsync()
        {
            var dbUserInfo = await GetCurrentNormalParseUser();
            var result = dbUserInfo.MapToExt<GetParseUserInfoDto>();
            result.UserNick = LoginUserInfo.UserNick;
            return KdyResult.Success(result);
        }

        public async Task<KdyResult> SaveParseUserInfoAsync(SaveParseUserInfoInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            var dbParseUser = await GetCurrentNormalParseUser();
            dbParseUser.IsHoldLink = input.IsHoldLink;
            dbParseUser.HoldLinkHost = input.HoldLinkHost;
            dbParseUser.SelfApiUrl = input.CustomUrl;
            _cloudParseUserRepository.Update(dbParseUser);
            await UnitOfWork.SaveChangesAsync();

            if (input.UserNick.IsEmptyExt() == false)
            {
                await UpdateUserClaimAsync(userId, CacheKeyConst.KdyCustomClaimType.UserNickClaimType, input.UserNick);
            }

            await _subAccountService.ClearUserInfoCacheAsync(userId);
            return KdyResult.Success();
        }

        public async Task<KdyResult<PageList<QueryParseUserSubAccountDto>>> QueryParseUserSubAccountAsync(QueryParseUserSubAccountInput input)
        {
            if (input.OrderBy == null ||
                input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new ()
                    {
                        Key = nameof(CloudParseUser.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var query = _cloudParseUserChildrenRepository.GetQuery();
            var userId = LoginUserInfo.GetUserId();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.UserId == userId);
            }

            if (input.KeyWord.IsEmptyExt() == false &&
                input.KeyWord.Length == 19 &&
                long.TryParse(input.KeyWord, out long tempId))
            {
                //id搜索
                //清掉原来的值
                input.KeyWord = string.Empty;
                query = query.Where(a => a.Id == tempId);
            }

            var result = await query.GetDtoPageListAsync<CloudParseUserChildren, QueryParseUserSubAccountDto>(input);
            if (result.DataCount == 0)
            {
                return KdyResult.Success(result);
            }

            var cacheType = await _subAccountService.GetAllCookieTypeCacheAsync();
            foreach (var dtoItem in result.Data)
            {
                dtoItem.SubAccountTypeStr = cacheType
                    .FirstOrDefault(a => a.Id == dtoItem.SubAccountTypeId)?.ShowText;
            }

            return KdyResult.Success(result);
        }

        public async Task<KdyResult> CreateAndUpdateSubAccountAsync(CreateAndUpdateSubAccountInput input)
        {
            input.BusinessId = input.BusinessId.TrimExt();
            input.Cookie = input.Cookie.TrimExt();
            input.Alias = input.Alias.TrimExt();
            input.OldSubAccountInfo = input.OldSubAccountInfo.TrimExt();

            var allCookieTypeCache = await _subAccountService.GetAllCookieTypeCacheAsync();
            var cookieTypeCache = allCookieTypeCache.FirstOrDefault(a => a.Id == input.SubAccountTypeId);
            if (cookieTypeCache == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,未知Cookie类型");
            }

            var userId = LoginUserInfo.GetUserId();
            var subAccountQuery = _cloudParseUserChildrenRepository.GetQuery();
            subAccountQuery = subAccountQuery.Where(a => a.CreatedUserId == userId &&
                                                         (a.Alias == input.Alias ||
                                                          a.CookieInfo == input.Cookie));

            bool isClearCache = false;
            IKdyCloudParseService kdyCloudParseService = null;
            CloudParseUserChildren dbChildren;
            if (input.SubAccountId.HasValue)
            {
                subAccountQuery = subAccountQuery.Where(a => a.Id != input.SubAccountId);
                if (await subAccountQuery.AnyAsync())
                {
                    return KdyResult.Error(KdyResultCode.Error, "操作失败,别名或cookie已存在");
                }

                //修改
                dbChildren =
                    await _cloudParseUserChildrenRepository.FirstOrDefaultAsync(a => a.Id == input.SubAccountId);
                if (dbChildren == null)
                {
                    return KdyResult.Error(KdyResultCode.Error, "修改失败,暂无信息");
                }

                #region 清除云盘缓存
                await _subAccountService.ClearSubAccountCacheAsync(dbChildren.Id);

                var oldCookieTypeCache = allCookieTypeCache.First(a => a.Id == dbChildren.CloudParseCookieTypeId);
                kdyCloudParseService = DiskCloudParseFactory.CreateKdyCloudParseService(oldCookieTypeCache.BusinessFlag,
                        dbChildren.Id);
                await kdyCloudParseService.ClearCacheAsync();
                isClearCache = true;
                #endregion

                dbChildren.Alias = input.Alias;
                dbChildren.CookieInfo = input.Cookie;
                dbChildren.CloudParseCookieTypeId = input.SubAccountTypeId;
                dbChildren.BusinessId = input.BusinessId;
                dbChildren.OldSubAccountInfo = input.OldSubAccountInfo;
                _cloudParseUserChildrenRepository.Update(dbChildren);

                if (input.IsSyncServerCookie)
                {
                    #region 同步更新服务器Cookie
                    var dbEntity = await _serverCookieRepository.FirstOrDefaultAsync(a => a.SubAccountId == dbChildren.Id);
                    if (dbEntity != null)
                    {
                        var cacheKey = GetServerCookieCacheKey(dbEntity.SubAccountId);
                        dbEntity.CookieInfo = input.Cookie;
                        _serverCookieRepository.Update(dbEntity);
                        await ClearCacheValueAsync(cacheKey);
                    }
                    #endregion
                }
            }
            else
            {
                if (await subAccountQuery.AnyAsync())
                {
                    return KdyResult.Error(KdyResultCode.Error, "操作失败,别名或cookie已存在");
                }

                //新增
                dbChildren = new CloudParseUserChildren(userId, input.SubAccountTypeId, input.Cookie)
                {
                    Alias = input.Alias,
                    BusinessId = input.BusinessId,
                    OldSubAccountInfo = input.OldSubAccountInfo
                };
                await _cloudParseUserChildrenRepository.CreateAsync(dbChildren);
            }

            await UnitOfWork.SaveChangesAsync();

            if (isClearCache)
            {
                //双清
                await Task.Delay(2500);
                await _subAccountService.ClearSubAccountCacheAsync(dbChildren.Id);
                await kdyCloudParseService.ClearCacheAsync();
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult<List<GetSubAccountByTypeDto>>> GetSubAccountByTypeIdAsync(long cookieTypeId)
        {
            var userId = LoginUserInfo.GetUserId();
            var userName = LoginUserInfo.UserName;
            var subAccountList = await _cloudParseUserChildrenRepository.GetAsNoTracking()
                .Where(a => a.UserId == userId &&
                            a.CloudParseCookieTypeId == cookieTypeId)
                .ToListAsync();
            var result = subAccountList.Select(a => new GetSubAccountByTypeDto()
            {
                ShowName = $"{(a.Alias.IsEmptyExt() ? $"{userName}-{a.Id.ToString().Substring(0, 3)}" : a.Alias)}",
                QueryValue = a.Id
            }).ToList();
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 用户所有子账号列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<IList<QueryParseUserSubAccountDto>>> GetUserAllSubAccountAsync()
        {
            var userId = LoginUserInfo.GetUserId();
            var query = _cloudParseUserChildrenRepository.GetQuery();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                var userIdStr = userId.ToString();
                //自己创建或共享的子账号信息
                query = query.Where(a => a.CreatedUserId == userId ||
                                         (string.IsNullOrEmpty(a.RelationalUserIds) == false
                                          && a.RelationalUserIds.Contains(userIdStr)));
            }

            var dbEntities = await query.ToListAsync();
            var result = dbEntities.MapToListExt<QueryParseUserSubAccountDto>();
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 创建解析用户
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateParesUserAsync()
        {
            var userId = LoginUserInfo.GetUserId();
            var query = _cloudParseUserRepository.GetQuery();
            query = query.Where(a => a.UserId == userId);
            if (await query.AnyAsync())
            {
                return KdyResult.Error(KdyResultCode.Error, "已提交申请");
            }

            var dbParseUser = new CloudParseUser(userId);
            dbParseUser.InitToken();
            await _cloudParseUserRepository.CreateAsync(dbParseUser);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success("提交成功,请等待审核");
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryParseUserDto>>> QueryParseUserAsync(QueryParseUserInput input)
        {
            if (input.OrderBy == null ||
                input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new ()
                    {
                        Key = nameof(CloudParseUser.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var query = _cloudParseUserRepository.GetQuery();
            var result = await query.GetDtoPageListAsync<CloudParseUser, QueryParseUserDto>(input);
            if (LoginUserInfo.IsSuperAdmin == false &&
                result.Data != null &&
                result.Data.Any())
            {
                //非管理不限制
                foreach (var dtoItem in result.Data)
                {
                    dtoItem.Remark = string.Empty;
                }
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 审批用户
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> AuditAsync(long userId)
        {
            var query = _cloudParseUserRepository.GetQuery();
            var dbUser = await query.FirstOrDefaultAsync(a => a.UserId == userId);
            dbUser.UserStatus = ServerCookieStatus.Normal;
            _cloudParseUserRepository.Update(dbUser);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 延期用户使用时间
        /// </summary>
        /// <param name="parseUserId">解析用户Id</param>
        /// <returns></returns>
        public async Task<KdyResult> DelayDateAsync(long parseUserId)
        {
            var parseUser = await _cloudParseUserRepository.FirstOrDefaultAsync(a => a.Id == parseUserId);
            if (parseUser == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效用户");
            }

            parseUser.DelayData();
            _cloudParseUserRepository.Update(parseUser);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 更新用户备注
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> UpdateUserRemarkAsync(UpdateUserRemarkInput input)
        {
            var dbUserInfo = await _cloudParseUserRepository.GetQuery()
                .FirstOrDefaultAsync(a => a.UserId == input.UserId);
            if (dbUserInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "用户不存在");
            }

            dbUserInfo.Remark = input.Remark;
            _cloudParseUserRepository.Update(dbUserInfo);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        internal async Task<CloudParseUser> GetCurrentNormalParseUser()
        {
            var userId = LoginUserInfo.GetUserId();
            var dbUserInfo = await _cloudParseUserRepository.GetQuery()
                .FirstOrDefaultAsync(a => a.UserId == userId);
            if (dbUserInfo == null ||
                dbUserInfo.UserStatus == ServerCookieStatus.Init)
            {
                return new CloudParseUser(userId);
                //throw new KdyCustomException("用户不存在,获取用户失败");
            }

            return dbUserInfo;

        }

        /// <summary>
        /// 获取当前用户声明列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<GetUserClaimsResponseItem>> GetCurrentUserClaims(long userId)
        {
            var result = await _authCenterHttpApi.GetUserClaimsAsync(userId);
            return result.Data.Claims;
        }

        /// <summary>
        /// 更新用户声明
        /// </summary>
        /// <remarks>
        ///  不存在则新增
        /// </remarks>
        /// <returns></returns>
        private async Task<KdyResult> UpdateUserClaimAsync(long userId, string claimType, string claimValue)
        {
            var currentClaims = await GetCurrentUserClaims(userId);
            var currentClaim = currentClaims.FirstOrDefault(a => a.ClaimType == claimType);
            if (currentClaims.Any() == false ||
                currentClaim == null)
            {
                //新增
                return await _authCenterHttpApi.SetUserClaimsAsync(userId, claimType, claimValue);
            }

            //修改
            return await _authCenterHttpApi.UpdateUserClaimsAsync(userId, currentClaim.ClaimId, claimType, claimValue);
        }

        /// <summary>
        /// 获取服务器缓存Key
        /// </summary>
        /// <returns></returns>
        private string GetServerCookieCacheKey(long subAccountId)
        {
            return $"{CacheKeyConst.KdyCacheName.ServerCookieKey}:{subAccountId}";
        }
    }
}
