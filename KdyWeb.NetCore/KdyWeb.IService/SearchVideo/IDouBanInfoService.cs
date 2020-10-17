using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 豆瓣信息 服务接口
    /// </summary>
    public interface IDouBanInfoService : IKdyService
    {
        /// <summary>
        /// 创建豆瓣信息
        /// </summary>
        /// <param name="subjectId">豆瓣Id</param>
        /// <returns></returns>
        Task<KdyResult<CreateForSubjectIdDto>> CreateForSubjectIdAsync(string subjectId);
    }
}
