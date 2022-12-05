using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;

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
        Task<KdyResult<List<GetSubAccountByTypeDto>>> GetSubAccountByTypeAsync(CloudParseCookieType type);
    }
}
