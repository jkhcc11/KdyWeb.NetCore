using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 影片系列 服务接口
    /// </summary>
    public interface IVideoSeriesService : IKdyService
    {
        /// <summary>
        /// 创建影片系列
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateVideoSeriesAsync(CreateVideoSeriesInput input);

        /// <summary>
        /// 查询影片系列
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryVideoSeriesDto>>> QueryVideoSeriesAsync(QueryVideoSeriesInput input);

        /// <summary>
        /// 查询影片系列列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryVideoSeriesListDto>>> QueryVideoSeriesListAsync(QueryVideoSeriesListInput input);

        /// <summary>
        /// 修改影片系列
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifyVideoSeriesAsync(ModifyVideoSeriesInput input);

        /// <summary>
        /// 获取影片系列
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<SelectedItemOut>>> GetVideoSeriesListAsync();

        /// <summary>
        /// 获取影片系列详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryVideoSeriesDto>> GetVideoSeriesDetailAsync(long seriesId);

        /// <summary>
        /// 创建影片系列列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateVideoSeriesListAsync(CreateVideoSeriesListInput input);

        /// <summary>
        /// 删除系列
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(long seriesId);

        /// <summary>
        /// 删除系列影片列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteVodListAsync(BatchDeleteForLongKeyInput input);
    }
}
