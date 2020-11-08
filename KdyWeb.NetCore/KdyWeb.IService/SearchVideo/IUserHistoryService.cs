using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 用户播放记录 服务接口
    /// </summary>
    public interface IUserHistoryService : IKdyService
    {
        /// <summary>
        /// 创建用户播放记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<KdyResult> CreateUserHistoryAsync(CreateUserHistoryInput input);
    }
}
