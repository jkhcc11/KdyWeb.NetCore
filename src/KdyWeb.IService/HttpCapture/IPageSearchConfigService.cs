using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    /// 站点页面搜索配置 服务接口
    /// </summary>
    public interface IPageSearchConfigService : IKdyService
    {
        /// <summary>
        /// 创建搜索配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateSearchConfigAsync(CreateSearchConfigInput input);

        /// <summary>
        /// 修改搜索配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifySearchConfigAsync(ModifySearchConfigInput input);

        /// <summary>
        /// 获取配置详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetDetailConfigDto>> GetDetailConfigAsync(long configId);

        /// <summary>
        /// 搜索配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<GetDetailConfigDto>>> SearchConfigAsync(SearchConfigInput input);

        /// <summary>
        /// 获取页面搜索实例
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetPageParseInstanceDto>> GetPageParseInstanceAsync(GetPageParseInstanceInput input);

        /// <summary>
        /// 一键复制站点配置
        /// </summary>
        /// <param name="oldKeyId">旧Id</param>
        /// <returns></returns>
        Task<KdyResult> OneCopyAsync(long oldKeyId);

        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BanAsync(long id);

        /// <summary>
        /// 查询前端可搜索配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<QueryShowPageConfigDto>>> QueryShowPageConfigAsync(SearchConfigInput input);
    }
}
