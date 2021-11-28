using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 解析平台用户 服务实现
    /// </summary>
    public class CloudParseUserService : BaseKdyService, ICloudParseUserService
    {
        private readonly IKdyRepository<CloudParseUser, long> _cloudParseUserRepository;
        private readonly IKdyRepository<CloudParseUserChildren> _cloudParseUserChildrenRepository;
        private readonly IKdyRedisCache _kdyRedisCache;

        public CloudParseUserService(IUnitOfWork unitOfWork, IKdyRepository<CloudParseUser, long> cloudParseUserRepository,
            IKdyRepository<CloudParseUserChildren> cloudParseUserChildrenRepository, IKdyRedisCache kdyRedisCache) : base(unitOfWork)
        {
            _cloudParseUserRepository = cloudParseUserRepository;
            _cloudParseUserChildrenRepository = cloudParseUserChildrenRepository;
            _kdyRedisCache = kdyRedisCache;
        }

        public async Task<KdyResult<GetParseUserInfoDto>> LoginWithParseUserAsync(LoginWithParseUserInput input)
        {
            var dbUserInfo = await _cloudParseUserRepository.GetQuery()
                .FirstOrDefaultAsync(a => a.UserName == input.UserName ||
                                          a.UserEmail == input.UserName);
            if (dbUserInfo == null)
            {
                throw new KdyCustomException("用户不存在,获取用户失败");
            }

            if (dbUserInfo.UserStatus != KdyUserStatus.Normal)
            {
                throw new KdyCustomException("用户状态非正常,登录失败");
            }

            if (dbUserInfo.CheckPwd(input.Pwd) == false)
            {
                throw new KdyCustomException("用户名或密码错误");
            }

            //todo：改为ids4
            var loginCacheInfo = new
            {
                UserId = dbUserInfo.Id,
                UserName = dbUserInfo.UserName,
                UserEmail = dbUserInfo.UserEmail,
                UserNick = dbUserInfo.UserNick,
                IsSuperAdmin = dbUserInfo.UserName == "admin"
            };
            var cacheKey = $"parse:{dbUserInfo.Id}";
            var loginJsonStr = JsonConvert.SerializeObject(loginCacheInfo);
            await _kdyRedisCache.GetDb(1).StringSetAsync(cacheKey, loginJsonStr);
            var result = dbUserInfo.MapToExt<GetParseUserInfoDto>();
            result.AuthKey = cacheKey;
            return KdyResult.Success(result);
        }

        public async Task<KdyResult<GetParseUserInfoDto>> GetParseUserInfoAsync()
        {
            var dbUserInfo = await GetCurrentNormalParseUser();
            var result = dbUserInfo.MapToExt<GetParseUserInfoDto>();
            return KdyResult.Success(result);
        }

        public async Task<KdyResult> SaveParseUserInfoAsync(SaveParseUserInfoInput input)
        {
            var dbUserInfo = await GetCurrentNormalParseUser();
            input.MapToPartExt(dbUserInfo);
            _cloudParseUserRepository.Update(dbUserInfo);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        public async Task<KdyResult<PageList<GetParseUserInfoChildrenDto>>> GetParseUserInfoChildrenAsync(GetParseUserInfoChildrenInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            var result = await _cloudParseUserChildrenRepository.GetQuery()
                .Where(a => a.UserId == userId)
                .GetDtoPageListAsync<CloudParseUserChildren, GetParseUserInfoChildrenDto>(input);
            return KdyResult.Success(result);
        }

        public async Task<KdyResult> SaveParseUserInfoChildrenAsync(SaveParseUserInfoChildrenInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            var dbChildren = new CloudParseUserChildren(userId, input.CookieType, input.CookieInfo);
            if (await _cloudParseUserChildrenRepository.GetAsNoTracking().AnyAsync(a =>
                a.UserId == userId && a.CookieType == input.CookieType && a.CookieInfo == input.CookieInfo))
            {
                return KdyResult.Error(KdyResultCode.Error, "新增失败,当前信息已存在");
            }

            await _cloudParseUserChildrenRepository.CreateAsync(dbChildren);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        public async Task<KdyResult<GetParseUserInfoChildrenDto>> GetParseUserInfoChildrenAsync(int childrenId)
        {
            var userId = LoginUserInfo.GetUserId();
            var dbUserInfo = await _cloudParseUserChildrenRepository.GetAsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Id == childrenId);
            var result = dbUserInfo.MapToExt<GetParseUserInfoChildrenDto>();
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
    }
}
