using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.KdyUser;

namespace KdyWeb.IService
{
    /// <summary>
    /// 用户 服务接口
    /// </summary>
    public interface IKdyUserService : IKdyService
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateUserAsync(CreateUserInput input);

        /// <summary>
        /// 用户名或邮箱是否存在
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<bool>> CheckUserExitAsync(CheckUserExitInput input);

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> FindUserPwdAsync(FindUserPwdInput input);

        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifyUserPwdAsync(ModifyUserPwdInput input);

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifyUserInfoAsync(ModifyUserInfoInput input);

        /// <summary>
        /// 获取用户登录Token
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> GetLoginTokenAsync(GetLoginTokenInput input);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> LogoutAsync();
    }
}
