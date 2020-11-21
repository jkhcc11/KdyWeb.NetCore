using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;

namespace KdyWeb.IService
{
    /// <summary>
    /// 用户 服务接口
    /// </summary>
    public interface IKdyUserService : IKdyService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetUserInfoDto>> GetUserInfoAsync(GetUserInfoInput input);
    }
}
