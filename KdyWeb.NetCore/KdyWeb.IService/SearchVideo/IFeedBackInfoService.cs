using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 反馈信息及录入 服务接口
    /// </summary>
    public interface IFeedBackInfoService : IKdyService
    {
        /// <summary>
        /// 分页获取反馈信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<GetFeedBackInfoDto>>> GetPageFeedBackInfoAsync(GetFeedBackInfoInput input);

        /// <summary>
        /// 创建反馈信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateFeedBackInfoAsync(CreateFeedBackInfoInput input);

        /// <summary>
        /// 变更反馈信息状态
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ChangeFeedBackInfoAsync(ChangeFeedBackInfoInput input);
    }
}
