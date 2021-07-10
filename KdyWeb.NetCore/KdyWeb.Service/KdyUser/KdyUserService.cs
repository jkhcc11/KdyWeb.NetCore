using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Entity;
using KdyWeb.IService;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service
{
    /// <summary>
    /// 用户 服务实现
    /// </summary>
    public class KdyUserService : BaseKdyService, IKdyUserService
    {
        private readonly IKdyRepository<KdyUser, long> _kdyUserRepository;
        private readonly IKdyRepository<KdyRole> _kdyRoleRepository;

        public KdyUserService(IUnitOfWork unitOfWork, IKdyRepository<KdyUser, long> kdyUserRepository, IKdyRepository<KdyRole> kdyRoleRepository) : base(unitOfWork)
        {
            _kdyUserRepository = kdyUserRepository;
            _kdyRoleRepository = kdyRoleRepository;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetUserInfoDto>> GetUserInfoAsync(GetUserInfoInput input)
        {
            if (input.OldUserId <= 0 &&
                input.UserInfo.IsEmptyExt())
            {
                return KdyResult.Error<GetUserInfoDto>(KdyResultCode.ParError, "参数不能为空");
            }

            var userInfo = await _kdyUserRepository.GetAsNoTracking()
                .Where(a => a.IsDelete == false)
                .CreateConditions(input)
                .ProjectToExt<KdyUser, GetUserInfoDto>().FirstOrDefaultAsync();
            if (userInfo == null)
            {
                return KdyResult.Error<GetUserInfoDto>(KdyResultCode.Error, "用户名或密码错误");
            }

            return KdyResult.Success(userInfo);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateUserAsync(CreateUserInput input)
        {
            var exit = await _kdyUserRepository.GetAsNoTracking()
                .AnyAsync(a => a.UserName == input.UserName);
            if (exit)
            {
                return KdyResult.Error(KdyResultCode.Error, "用户名已存在");
            }

            var role = await _kdyRoleRepository.FirstOrDefaultAsync(a => a.KdyRoleType == KdyRoleType.Normal);
            var dbUser = new KdyUser(input.UserName, input.UserNick, input.UserEmail, role.Id);

            KdyUser.SetPwd(dbUser, input.UserPwd);
            await _kdyUserRepository.CreateAsync(dbUser);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 用户名或邮箱是否存在
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CheckUserExitAsync(CheckUserExitInput input)
        {
            var check = await _kdyUserRepository.GetAsNoTracking()
                .AnyAsync(a => a.UserName == input.UserName ||
                               a.UserEmail == input.UserName);
            if (check)
            {
                return KdyResult.Error(KdyResultCode.Error, "用户名或邮箱已存在，请直接登录");
            }

            return KdyResult.Success();
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> FindUserPwdAsync(FindUserPwdInput input)
        {
            var dbUser = await _kdyUserRepository.FirstOrDefaultAsync(a => a.Id == input.UserId);
            if (dbUser == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "用户信息错误，请重试");
            }

            KdyUser.SetPwd(dbUser, input.NewPwd);
            _kdyUserRepository.Update(dbUser);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyUserPwdAsync(ModifyUserPwdInput input)
        {
            var dbUser = await _kdyUserRepository.FirstOrDefaultAsync(a => a.Id == input.UserId);
            if (dbUser == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "用户信息错误，请重试");
            }

            var oldMd5 = $"{input.OldPwd}{KdyWebConst.UserSalt}".Md5Ext();
            if (oldMd5 != dbUser.UserPwd)
            {
                return KdyResult.Error(KdyResultCode.Error, "旧密码错误，请重试");
            }

            KdyUser.SetPwd(dbUser, input.NewPwd);
            _kdyUserRepository.Update(dbUser);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyUserInfoAsync(ModifyUserInfoInput input)
        {
            var dbUser = await _kdyUserRepository.FirstOrDefaultAsync(a => a.Id == input.UserId);
            if (dbUser == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "用户信息错误，请重试");
            }

            dbUser.SetUserInfo(input.UserEmail,input.UserNick);
            _kdyUserRepository.Update(dbUser);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
    }
}
