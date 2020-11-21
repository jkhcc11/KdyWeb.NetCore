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

        public KdyUserService(IUnitOfWork unitOfWork, IKdyRepository<KdyUser, long> kdyUserRepository) : base(unitOfWork)
        {
            _kdyUserRepository = kdyUserRepository;
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
    }
}
