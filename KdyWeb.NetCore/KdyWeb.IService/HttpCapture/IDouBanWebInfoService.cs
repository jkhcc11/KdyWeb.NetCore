using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    /// 豆瓣站点相关 服务接口
    /// </summary>
    public interface IDouBanWebInfoService : IKdyService
    {
        /// <summary>
        /// 根据豆瓣Id获取豆瓣信息
        /// </summary>
        /// <param name="subjectId">豆瓣Id</param>
        /// <returns></returns>
        Task<KdyResult<GetDouBanOut>> GetInfoBySubjectId(string subjectId);

        /// <summary>
        /// 根据豆瓣Id获取豆瓣信息
        /// </summary>
        /// <remarks>
        ///  PC版模拟
        /// </remarks>
        /// <param name="subjectId">豆瓣Id</param>
        /// <returns></returns>
        Task<KdyResult<GetDouBanOut>> GetInfoBySubjectIdForPcWeb(string subjectId);
    }
}
