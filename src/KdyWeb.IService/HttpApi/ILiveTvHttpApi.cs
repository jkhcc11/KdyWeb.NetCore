using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.LiveTv;

namespace KdyWeb.IService.HttpApi
{
    /// <summary>
    /// IPTV直播 服务接口
    /// </summary>
    public interface ILiveTvHttpApi : IKdyService
    {
        /// <summary>
        /// 获取所有频道
        /// </summary>
        /// <returns></returns>
        Task<List<GetAllChannelsDto>> GetAllChannelsAsync();

        /// <summary>
        /// 获取实时流检查列表
        /// </summary>
        /// <returns></returns>
        Task<List<GetAllStreamsDto>> GetAllStreamsAsync();
    }
}
