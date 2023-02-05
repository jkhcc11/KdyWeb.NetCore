using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.Bilibili;

namespace KdyWeb.IService.HttpApi
{
    /// <summary>
    /// B站 Api
    /// </summary>
    public interface IBilibiliHttpApi : IKdyService
    {
        /// <summary>
        /// 根据视频详情页获取视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<KdyResult<GetVideoInfoByDetailUrlResponse>> GetVideoInfoByDetailUrlAsync(GetVideoInfoByDetailUrlRequest input);

        /// <summary>
        /// 根据弹幕用户Id获取用户Id
        /// </summary>
        /// <returns></returns>
        string GetUidByMid(string userMid);
    }
}
