using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.HttpApi.AuthCenter;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService;
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.HttpApi;
using KdyWeb.Repository;
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

        public CloudParseUserService(IUnitOfWork unitOfWork, IKdyRepository<CloudParseUser, long> cloudParseUserRepository,
            IKdyRepository<CloudParseUserChildren, long> cloudParseUserChildrenRepository,
            IAuthCenterHttpApi authCenterHttpApi) : base(unitOfWork)
        {
            _cloudParseUserRepository = cloudParseUserRepository;
            _cloudParseUserChildrenRepository = cloudParseUserChildrenRepository;
            _authCenterHttpApi = authCenterHttpApi;
        }

        public async Task<KdyResult<GetParseUserInfoDto>> GetParseUserInfoAsync()
        {
            var dbUserInfo = await GetCurrentNormalParseUser();
            var result = dbUserInfo.MapToExt<GetParseUserInfoDto>();
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

            return KdyResult.Success();
        }

        public async Task<KdyResult<PageList<QueryParseUserSubAccountDto>>> QueryParseUserSubAccountAsync(QueryParseUserSubAccountInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            var query = _cloudParseUserChildrenRepository.GetQuery()
                .Where(a => a.UserId == userId);
            if (input.SubAccountTypeId.HasValue)
            {
                query = query.Where(a => a.CloudParseCookieTypeId == input.SubAccountTypeId);
            }

            var result = await query.GetDtoPageListAsync<CloudParseUserChildren, QueryParseUserSubAccountDto>(input);
            return KdyResult.Success(result);
        }

        public async Task<KdyResult> CreateAndUpdateSubAccountAsync(CreateAndUpdateSubAccountInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            if (input.SubAccountId.HasValue)
            {
                //修改
                var dbChildren =
                    await _cloudParseUserChildrenRepository.FirstOrDefaultAsync(a => a.Id == input.SubAccountId);
                if (dbChildren == null)
                {
                    return KdyResult.Error(KdyResultCode.Error, "修改失败,暂无信息");
                }

                dbChildren.Alias = input.Alias;
                dbChildren.CookieInfo = input.Cookie;
                dbChildren.CloudParseCookieTypeId = input.SubAccountTypeId;
                _cloudParseUserChildrenRepository.Update(dbChildren);
            }
            else
            {
                //新增
                var dbChildren = new CloudParseUserChildren(userId, input.SubAccountTypeId, input.Cookie)
                {
                    Alias = input.Alias
                };
                await _cloudParseUserChildrenRepository.CreateAsync(dbChildren);
            }

            await UnitOfWork.SaveChangesAsync();
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
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        internal async Task<CloudParseUser> GetCurrentNormalParseUser()
        {
            var userId = LoginUserInfo.GetUserId();
            var dbUserInfo = await _cloudParseUserRepository.GetQuery()
                .FirstOrDefaultAsync(a => a.Id == userId);
            if (dbUserInfo == null)
            {
                throw new KdyCustomException("用户不存在,获取用户失败");
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
    }
}
