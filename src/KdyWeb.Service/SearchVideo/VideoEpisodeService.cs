﻿using System.Collections.Generic;
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
        /// <remarks>
        /// 1、根据当前主键查询所有剧集
        /// 2、根据剧集名查找新增的修改的
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> CreateEpisodeAsync(CreateEpisodeInput input)
        {
            if (input.EpItems == null || input.EpItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "剧集不能为空");
            }

            var saveResult = await SaveEpisodeInfo(input.MainId, input.EpItems);
            if (saveResult.IsSuccess == false)
            {
                return KdyResult.Error(saveResult.Code, saveResult.Msg);
            }

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

            result.VideoEpisodeGroup.OrderByExt();

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 更新未完结影片数据
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> UpdateNotEndVideoAsync(UpdateNotEndVideoInput input)
        {
            if (input.EpItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "剧集列表为空，更新失败");
            }

            var dbMain = await _videoMainRepository.FirstOrDefaultAsync(a => a.Id == input.MainId);
            if (dbMain == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "影片主键错误");
            }

            //更新主表
            dbMain.IsEnd = input.IsEnd;
            dbMain.VideoContentFeature = input.VideoContentFeature;
            _videoMainRepository.Update(dbMain);

            var saveResult = await SaveEpisodeInfo(input.MainId, input.EpItems);
            if (saveResult.IsSuccess == false)
            {
                return KdyResult.Error(saveResult.Code, saveResult.Msg);
            }

            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success("更新成功");
        }

        /// <summary>
        /// 保存剧集信息
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> SaveEpisodeInfo(long mainId, List<EditEpisodeItem> epItems)
        {
            //影片剧集信息
            var dbEpisode = await _videoEpisodeRepository.GetAsNoTracking()
                .Where(a => a.VideoEpisodeGroup.MainId == mainId)
                .ToListAsync();
            if (dbEpisode.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "更新失败，不存在剧集");
            }

            var result = epItems.GetEditInfoExt(dbEpisode);
            if (result.AddEpInfo.Any())
            {
                await _videoEpisodeRepository.CreateAsync(result.AddEpInfo);
            }

            if (result.UpdateEpInfo.Any())
            {
                _videoEpisodeRepository.Update(result.UpdateEpInfo);
            }

            return KdyResult.Success();
        }
    }
}
