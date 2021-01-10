using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 剧集 服务接口
    /// </summary>
    public interface IVideoEpisodeService : IKdyService
    {
        /// <summary>
        /// 更新剧集
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpdateEpisodeAsync(List<UpdateEpisodeInput> input);

        /// <summary>
        /// 创建剧集
        /// </summary>
        /// <remarks>
        /// 1、根据当前主键查询所有剧集
        /// 2、根据剧集名查找新增的修改的
        /// </remarks>
        Task<KdyResult> CreateEpisodeAsync(CreateEpisodeInput input);

        /// <summary>
        /// 批量删除剧集
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteEpisodeAsync(BatchDeleteForLongKeyInput input);

        /// <summary>
        /// 根据剧集Id获取影片数据
        /// </summary>
        /// <param name="epId">剧集Id</param>
        /// <returns></returns>
        Task<KdyResult<GetEpisodeInfoDto>> GetEpisodeInfoAsync(long epId);

        /// <summary>
        /// 更新未完结影片数据
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpdateNotEndVideoAsync(UpdateNotEndVideoInput input);
    }
}
