using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.PageParse;

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
    }
}
