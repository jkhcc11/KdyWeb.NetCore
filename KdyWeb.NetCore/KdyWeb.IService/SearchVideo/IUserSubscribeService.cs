using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 用户订阅 服务接口
    /// </summary>
    public interface IUserSubscribeService : IKdyService
    {
        /// <summary>
        /// 用户收藏查询
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryUserSubscribeDto>>> QueryUserSubscribeAsync(QueryUserSubscribeInput input);

        /// <summary>
        /// 创建用户收藏
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateUserSubscribeAsync(CreateUserSubscribeInput input);
    }
}
