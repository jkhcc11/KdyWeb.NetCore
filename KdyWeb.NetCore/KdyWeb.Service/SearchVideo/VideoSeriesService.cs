using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 影片系列 服务实现
    /// </summary>
    public class VideoSeriesService : BaseKdyService, IVideoSeriesService
    {
        private readonly IKdyRepository<VideoSeries, long> _videoSeriesRepository;
        private readonly IKdyRepository<VideoSeriesList, long> _videoSeriesListRepository;

        public VideoSeriesService(IUnitOfWork unitOfWork, IKdyRepository<VideoSeries, long> videoSeriesRepository,
            IKdyRepository<VideoSeriesList, long> videoSeriesListRepository) : base(unitOfWork)
        {
            _videoSeriesRepository = videoSeriesRepository;
            _videoSeriesListRepository = videoSeriesListRepository;
        }

        /// <summary>
        /// 创建影片系列
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateVideoSeriesAsync(CreateVideoSeriesInput input)
        {
            var dbInput = input.MapToExt<VideoSeries>();
            var exit = await _videoSeriesRepository.GetAsNoTracking()
                .AnyAsync(a => a.SeriesName == dbInput.SeriesName);
            if (exit)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建系列失败，名称已存在");
            }

            await _videoSeriesRepository.CreateAsync(dbInput);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 查询影片系列
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryVideoSeriesDto>>> QueryVideoSeriesAsync(QueryVideoSeriesInput input)
        {
            var pageList = await _videoSeriesRepository.GetQuery()
                .GetDtoPageListAsync<VideoSeries, QueryVideoSeriesDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 查询影片系列列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryVideoSeriesListDto>>> QueryVideoSeriesListAsync(QueryVideoSeriesListInput input)
        {
            var pageList = await _videoSeriesListRepository.GetQuery()
                .Include(a => a.VideoMain)
                .GetDtoPageListAsync<VideoSeriesList, QueryVideoSeriesListDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 修改影片系列
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyVideoSeriesAsync(ModifyVideoSeriesInput input)
        {
            var dbSeries = await _videoSeriesRepository.FirstOrDefaultAsync(a => a.Id == input.SeriesId);
            if (dbSeries == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "修改失败，剧集Id失败");
            }

            if (dbSeries.SeriesName != input.SeriesName)
            {
                var exit = await _videoSeriesRepository.GetAsNoTracking()
                    .AnyAsync(a => a.SeriesName == input.SeriesName);
                if (exit)
                {
                    return KdyResult.Error(KdyResultCode.Error, "修改失败，系列名称已存在");
                }
            }

            VideoSeriesEdit(dbSeries, input);
            _videoSeriesRepository.Update(dbSeries);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        private void VideoSeriesEdit(VideoSeries dbSeries, BaseVideoSeriesInput input)
        {
            dbSeries.OrderBy = input.OrderBy;
            dbSeries.LiveUrl = input.LiveUrl;
            dbSeries.SeriesDesUrl = input.SeriesDesUrl;
            dbSeries.SeriesImg = input.SeriesImg;
            dbSeries.SeriesName = input.SeriesName;
            dbSeries.SeriesRemark = input.SeriesRemark;
        }
    }
}
