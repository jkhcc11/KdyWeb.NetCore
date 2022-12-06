using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.GameDown;

namespace KdyWeb.IService.GameDown
{
    /// <summary>
    /// 游戏下载资源 服务接口
    /// </summary>
    public interface IGameDownService : IKdyService
    {
        /// <summary>
        /// 查询游戏下载列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryGameDownListWithAdminDto>>> QueryGameDownListWithAdminAsync(QueryGameDownListWithAdminInput input);
    }
}
