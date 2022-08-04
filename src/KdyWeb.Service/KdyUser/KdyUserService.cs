using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.HttpApi.AuthCenter;
using KdyWeb.Dto.KdyUser;
using KdyWeb.Dto.VerificationCode;
using KdyWeb.IService;
using KdyWeb.IService.CrossRequest;
using KdyWeb.IService.HttpApi;
using KdyWeb.IService.VerificationCode;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace KdyWeb.Service
{
    /// <summary>
    /// 用户 服务实现
    /// </summary>
    public class KdyUserService : BaseKdyService, IKdyUserService
    {
        private readonly IAuthCenterHttpApi _authCenterHttpApi;
        private readonly ILoginUserInfo _loginUserInfo;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly ICrossRequestService _crossRequestService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KdyUserService(IUnitOfWork unitOfWork, IAuthCenterHttpApi authCenterHttpApi,
            ILoginUserInfo loginUserInfo, IVerificationCodeService verificationCodeService,
            ICrossRequestService crossRequestService, IHttpContextAccessor httpContextAccessor) : base(unitOfWork)
        {
            _authCenterHttpApi = authCenterHttpApi;
            _loginUserInfo = loginUserInfo;
            _verificationCodeService = verificationCodeService;
            _crossRequestService = crossRequestService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateUserAsync(CreateUserInput input)
        {
            //验证码校验
            var check = await _verificationCodeService.CheckVerificationCodeAsync(VerificationCodeType.Register,
                input.UserEmail, input.EmailCode);
            if (check.IsSuccess == false)
            {
                return KdyResult.Error(check.Code, check.Msg);
            }

            var response = await _authCenterHttpApi.CreateUserAsync(input.UserName, input.UserNick, input.UserEmail);
            if (response.IsSuccess == false)
            {
                if (response.Code == KdyResultCode.Duplicate)
                {
                    return KdyResult.Error(response.Code, "用户名或邮箱已存在,请使用找回密码功能");
                }

                return KdyResult.Error(response.Code, response.Msg);
            }

            var userId = response.Data.Id;
            //todo:中间断了目前没有处理

            //创建用户、设置密码、设置角色、设置昵称和修改时间
            await _authCenterHttpApi.ChangePwdAsync(userId, input.UserPwd);
            await _authCenterHttpApi.SetUserRoleAsync(userId, 1468250357867089920);
            await UpdateUserClaimAsync(userId, CacheKeyConst.KdyCustomClaimType.UserNickClaimType,
                input.UserNick);

            await UpdateUserClaimAsync(userId, CacheKeyConst.KdyCustomClaimType.RegTme,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
            return KdyResult.Success();

            //var exit = await _kdyUserRepository.GetAsNoTracking()
            //    .AnyAsync(a => a.UserName == input.UserName);
            //if (exit)
            //{
            //    return KdyResult.Error(KdyResultCode.Error, "用户名已存在");
            //}

            //var role = await _kdyRoleRepository.FirstOrDefaultAsync(a => a.KdyRoleType == KdyRoleType.Normal);
            //var dbUser = new KdyUser(input.UserName, input.UserNick, input.UserEmail, role.Id);

            //KdyUser.SetPwd(dbUser, input.UserPwd);
            //await _kdyUserRepository.CreateAsync(dbUser);
            //await UnitOfWork.SaveChangesAsync();
            //return KdyResult.Success();
        }

        /// <summary>
        /// 用户名或邮箱是否存在
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<bool>> CheckUserExitAsync(CheckUserExitInput input)
        {
            var searchResult = await _authCenterHttpApi.SearchUserAsync(input.UserName);
            var check = searchResult.Data.Users
                .Any(a => a.UserName == input.UserName ||
                               a.Email == input.UserName);
            if (check)
            {
                return KdyResult.Success(true, "用户名或邮箱已存在，请直接登录");
            }

            return KdyResult.Success(false);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> FindUserPwdAsync(FindUserPwdInput input)
        {
            //验证码校验
            var check = await _verificationCodeService.CheckVerificationCodeAsync(VerificationCodeType.FindPwd,
                input.UserEmail, input.EmailCode);
            if (check.IsSuccess == false)
            {
                return KdyResult.Error(check.Code, check.Msg);
            }

            //找用户
            var searchResult = await _authCenterHttpApi.SearchUserAsync(input.UserEmail);
            var dbUser = searchResult.Data.Users.FirstOrDefault(a => a.Email == input.UserEmail);
            if (dbUser == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "该邮箱不存在,修改失败");
            }

            await _authCenterHttpApi.ChangePwdAsync(dbUser.Id, input.NewPwd);
            await UpdateUserClaimAsync(dbUser.Id, CacheKeyConst.KdyCustomClaimType.UpAtTme,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
            return KdyResult.Success();
            //var dbUser = await _kdyUserRepository.FirstOrDefaultAsync(a => a.Id == input.UserId);
            //if (dbUser == null)
            //{
            //    return KdyResult.Error(KdyResultCode.Error, "用户信息错误，请重试");
            //}

            //KdyUser.SetPwd(dbUser, input.NewPwd);
            //_kdyUserRepository.Update(dbUser);
            //await UnitOfWork.SaveChangesAsync();
            //return KdyResult.Success();
        }

        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyUserPwdAsync(ModifyUserPwdInput input)
        {
            var userId = _loginUserInfo.GetUserId();
            var email = _loginUserInfo.UserEmail;
            //验证码校验
            var check = await _verificationCodeService.CheckVerificationCodeAsync(VerificationCodeType.ModifyPwd,
                email, input.EmailCode);
            if (check.IsSuccess == false)
            {
                return KdyResult.Error(check.Code, check.Msg);
            }

            await _authCenterHttpApi.ChangePwdAsync(userId, input.NewPwd);
            await UpdateUserClaimAsync(userId, CacheKeyConst.KdyCustomClaimType.UpAtTme,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
            return KdyResult.Success();
            //var dbUser = await _kdyUserRepository.FirstOrDefaultAsync(a => a.Id == input.UserId);
            //if (dbUser == null)
            //{
            //    return KdyResult.Error(KdyResultCode.Error, "用户信息错误，请重试");
            //}

            //var oldMd5 = $"{input.OldPwd}{KdyWebConst.UserSalt}".Md5Ext();
            //if (oldMd5 != dbUser.UserPwd)
            //{
            //    return KdyResult.Error(KdyResultCode.Error, "旧密码错误，请重试");
            //}

            //KdyUser.SetPwd(dbUser, input.NewPwd);
            //_kdyUserRepository.Update(dbUser);
            //await UnitOfWork.SaveChangesAsync();
            //return KdyResult.Success();
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyUserInfoAsync(ModifyUserInfoInput input)
        {
            var userId = _loginUserInfo.GetUserId();

            var setResult = await UpdateUserClaimAsync(userId, CacheKeyConst.KdyCustomClaimType.UserNickClaimType,
                input.UserNick);
            if (setResult.IsSuccess == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作异常,请重试");
            }

            await UpdateUserClaimAsync(userId, CacheKeyConst.KdyCustomClaimType.UpAtTme,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));

            return KdyResult.Success();
        }

        /// <summary>
        /// 获取用户登录Token
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> GetLoginTokenAsync(GetLoginTokenInput input)
        {
            var token = await _crossRequestService.GetAccessTokenByUserNameOrEmailAsync(input.UserName, input.UserPwd);
            if (token.IsSuccess == false)
            {
                return KdyResult.Error(token.Code, token.Msg);
            }

            return KdyResult.Success(token.Data.AccessToken, "登录成功");
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> LogoutAsync()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"] + "";
            var cacheKey = $"{CacheKeyConst.KdyCacheName.UserLogoutKey}:{token.Replace("Bearer ", "")}";
            await KdyRedisCache.GetCache().SetStringAsync(cacheKey, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365)
                });
            return KdyResult.Success();
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
        /// 通过Email查找用户信息
        /// </summary>
        /// <returns></returns>
        private async Task<SearchUserResponseItem> GetUserInfoByEmailAsync(string email)
        {
            //找用户
            var searchResult = await _authCenterHttpApi.SearchUserAsync(email);
            if (searchResult.IsSuccess == false ||
                searchResult.Data == null ||
                searchResult.Data.Users.Any() == false)
            {
                return null;
            }

            var dbUser = searchResult.Data.Users.FirstOrDefault(a => a.Email == email);
            return dbUser;
        }
    }
}
