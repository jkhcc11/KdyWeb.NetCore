using System.Collections.Generic;
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
        /// 查询子账号列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryParseUserSubAccountDto>>> QueryParseUserSubAccountAsync(QueryParseUserSubAccountInput input);

        /// <summary>
        /// 创建和更新子账号信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateSubAccountAsync(CreateAndUpdateSubAccountInput input);

        /// <summary>
        /// 根据类型获取子账号列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetSubAccountByTypeDto>>> GetSubAccountByTypeIdAsync(long cookieTypeId);

        /// <summary>
        /// 用户所有子账号列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<IList<QueryParseUserSubAccountDto>>> GetUserAllSubAccountAsync();

        /// <summary>
        /// 创建解析用户
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateParesUserAsync();

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryParseUserDto>>> QueryParseUserAsync(QueryParseUserInput input);

        /// <summary>
        /// 审批用户
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> AuditAsync(long userId);

        /// <summary>
        /// 延期用户使用时间
        /// </summary>
        /// <param name="parseUserId">解析用户Id</param>
        /// <returns></returns>
        Task<KdyResult> DelayDateAsync(long parseUserId);
    }
}
