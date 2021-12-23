using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// 解析平台用户 服务接口
    /// </summary>
    public interface ICloudParseUserService : IKdyService
    {
        ///// <summary>
        ///// 解析用户登录
        ///// </summary>
        ///// <returns></returns>
        //Task<KdyResult<GetParseUserInfoDto>> LoginWithParseUserAsync(LoginWithParseUserInput input);

        /// <summary>
        /// 获取解析用户信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetParseUserInfoDto>> GetParseUserInfoAsync();

        /// <summary>
        /// 保存解析用户信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> SaveParseUserInfoAsync(SaveParseUserInfoInput input);

        /// <summary>
        /// 获取解析用户子账号列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<GetParseUserInfoChildrenDto>>> GetParseUserInfoChildrenAsync(GetParseUserInfoChildrenInput input);

        /// <summary>
        /// 保存解析用户子账号信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> SaveParseUserInfoChildrenAsync(SaveParseUserInfoChildrenInput input);

        /// <summary>
        /// 获取解析用户子账号信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetParseUserInfoChildrenDto>> GetParseUserInfoChildrenAsync(int childrenId);
    }
}
