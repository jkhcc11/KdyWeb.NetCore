using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 剧集 服务实现
    /// </summary>
    public class VideoEpisodeService : BaseKdyService, IVideoEpisodeService
    {
        private readonly IKdyRepository<VideoEpisode, long> _videoEpisodeRepository;
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;

        public VideoEpisodeService(IUnitOfWork unitOfWork, IKdyRepository<VideoEpisode, long> videoEpisodeRepository, IKdyRepository<VideoMain, long> videoMainRepository)
            : base(unitOfWork)
        {
            _videoEpisodeRepository = videoEpisodeRepository;
            _videoMainRepository = videoMainRepository;
        }


        /// <summary>
        /// 更新剧集
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> UpdateEpisodeAsync(List<UpdateEpisodeInput> input)
        {
            var epIds = input.Select(a => a.Id);
            var dbEpisode = await _videoEpisodeRepository.GetQuery()
                .Where(a => epIds.Contains(a.Id))
                .ToListAsync();
            if (dbEpisode.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "更新失败剧集集合不存在");
            }

            foreach (var dbItem in dbEpisode)
            {
                var item = input.FirstOrDefault(a => a.Id == dbItem.Id);

                item?.ToDbEpisode(dbItem);
            }

            _videoEpisodeRepository.Update(dbEpisode);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success("更新成功");
        }

        /// <summary>
        /// 创建剧集
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateEpisodeAsync(CreateEpisodeInput input)
        {
            if (input.EpItems == null || input.EpItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "剧集不能为空");
            }

            var dbEpisode = await _videoEpisodeRepository.GetAsNoTracking()
                .Where(a => a.VideoEpisodeGroup.MainId == input.MainId)
                .ToListAsync();
            if (dbEpisode.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "更新失败，不存在剧集");
            }

            //已存在得剧集Url
            var epUrls = dbEpisode.Select(a => a.EpisodeUrl).ToList();
            var canEditEp = input.EpItems
                .Where(a => epUrls.Contains(a.EpisodeUrl) == false)
                .ToList();
            if (canEditEp.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "可新增剧集为空");
            }

            var groupId = dbEpisode.First().EpisodeGroupId;
            //生成数据
            var dbList = canEditEp.Select(episodeItem => new VideoEpisode(episodeItem.EpisodeName, episodeItem.EpisodeUrl)
            {
                OrderBy = episodeItem.OrderBy ?? 0,
                EpisodeGroupId = groupId
            }).ToList();

            await _videoEpisodeRepository.CreateAsync(dbList);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success("剧集创建成功");
        }

        /// <summary>
        /// 批量删除剧集
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteEpisodeAsync(BatchDeleteForLongKeyInput input)
        {
            if (input.Ids == null || input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var dbEp = await _videoEpisodeRepository.GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            _videoEpisodeRepository.Delete(dbEp);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success("剧集删除成功");
        }

        /// <summary>
        /// 根据剧集Id获取影片数据
        /// </summary>
        /// <param name="epId">剧集Id</param>
        /// <returns></returns>
        public async Task<KdyResult<GetEpisodeInfoDto>> GetEpisodeInfoAsync(long epId)
        {
            var epInfo = await _videoEpisodeRepository.GetQuery()
                .Include(a => a.VideoEpisodeGroup)
                .ThenInclude(a => a.Episodes)
                .FirstOrDefaultAsync(a => a.Id == epId);
            if (epInfo == null)
            {
                return KdyResult.Error<GetEpisodeInfoDto>(KdyResultCode.Error, "剧集Id错误");
            }

            //主表信息
            var dbMain = await _videoMainRepository.FirstOrDefaultAsync(a => a.Id == epInfo.VideoEpisodeGroup.MainId);

            var result = epInfo.MapToExt<GetEpisodeInfoDto>();
            result.VideoMainInfo = dbMain.MapToExt<VideoMainDto>();

            return KdyResult.Success(result);
        }

    }
}
