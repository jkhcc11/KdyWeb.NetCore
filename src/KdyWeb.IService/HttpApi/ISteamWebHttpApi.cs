using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.Steam;

namespace KdyWeb.IService.HttpApi
{
    /// <summary>
    /// Steam WebApi
    /// </summary>
    public interface ISteamWebHttpApi : IKdyService
    {
        /// <summary>
        /// 根据商店Url获取游戏信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetGameInfoByStoreUrlResponse>> GetGameInfoByStoreUrlAsync(string storeUrl);
    }
}
