using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Entity;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.IService
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
            var dbUser = new KdyUser(input.UserName, input.UserNick, input.UserEmail, input.UserPwd, role.Id);

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
    }
}
