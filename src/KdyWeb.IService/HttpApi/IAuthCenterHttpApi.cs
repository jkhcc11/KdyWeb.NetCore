using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.AuthCenter;

namespace KdyWeb.IService.HttpApi
{
    /// <summary>
    /// 授权中心Api 接口
    /// </summary>
    public interface IAuthCenterHttpApi : IKdyService
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userNick">用户昵称</param>
        /// <param name="userEmail">邮箱</param>
        /// <returns></returns>
        Task<KdyResult<CreateUserResponse>> CreateUserAsync(string userName, string userNick, string userEmail);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ChangePwdAsync(long userId, string pwd);

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        Task<KdyResult> SetUserRoleAsync(long userId, long roleId);

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<SearchUserResponse>> SearchUserAsync(string userEmail);

        /// <summary>
        /// 设置用户声明
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="claimType">声明类型</param>
        /// <param name="claimValue">声明值</param>
        /// <returns></returns>
        Task<KdyResult> SetUserClaimsAsync(long userId, string claimType, string claimValue);

        /// <summary>
        /// 修改用户声明
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="claimId">声明Id</param>
        /// <param name="claimType">声明类型</param>
        /// <param name="claimValue">声明值</param>
        /// <returns></returns>
        Task<KdyResult> UpdateUserClaimsAsync(long userId, int claimId, string claimType, string claimValue);

        /// <summary>
        /// 获取用户声明列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task<KdyResult<GetUserClaimsResponse>> GetUserClaimsAsync(long userId);
    }
}
