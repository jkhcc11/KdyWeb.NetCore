using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Repository;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 视频弹幕 服务接口
    /// todo:调整为websocket
    /// </summary>
    public class VideoDanMuService : BaseKdyService, IVideoDanMuService
    {
        private readonly IKdyRepository<VideoDanMu, long> _videoDanMuRepository;
        private readonly IKdyRepository<VideoEpisode, long> _videoEpisodeRepository;

        public VideoDanMuService(IUnitOfWork unitOfWork, IKdyRepository<VideoDanMu, long> videoDanMuRepository,
            IKdyRepository<VideoEpisode, long> videoEpisodeRepository) :
            base(unitOfWork)
        {
            _videoDanMuRepository = videoDanMuRepository;
            _videoEpisodeRepository = videoEpisodeRepository;
        }

        /// <summary>
        /// 创建弹幕
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateDanMuAsync(CreateDanMuInput input)
        {
            var dbInput = input.MapToExt<VideoDanMu>();
            await _videoDanMuRepository.CreateAsync(dbInput);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取视频剧集弹幕
        /// </summary>
        /// <param name="epId">剧集Id</param>
        /// <returns></returns>
        public async Task<KdyResult<string>> GetVideoDanMuAsync(long epId)
        {
            var cacheKey = $"{KdyServiceCacheKey.DanMuKey}:{epId}";
            var list = await GetCacheValueAsync(cacheKey, async () =>
            {
                return await _videoDanMuRepository.GetAsNoTracking()
                         .Where(a => a.EpId == epId)
                         .OrderBy(a => a.DTime)
                         .ToListAsync();
            });

            var dm = new StringBuilder();
            dm.Append("<i>");
            foreach (var item in list)
            {
                var unix = item.CreatedTime.ToSecondTimestamp();
                //时间节点，模式，字体大小，颜色，时间戳，stime,用户名，时间戳
                dm.AppendFormat($"<d p=\"{item.DTime},{item.DMode},{item.DSize},{item.DColor},{unix},0,游客,{unix}\">{item.Msg}</d>");
            }
            dm.Append("</i>");

            return KdyResult.Success(dm.ToString(), "获取成功");
        }

        /// <summary>
        /// 搜索弹幕
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<SearchDanMuDto>>> SearchDanMuAsync(SearchDanMuInput input)
        {
            if (input.OrderBy == null || input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(VideoDanMu.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            //分页数据
            var pageData = await _videoDanMuRepository.GetQuery()
                .GetDtoPageListAsync<VideoDanMu, SearchDanMuDto>(input);
            if (pageData.DataCount <= 0)
            {
                return KdyResult.Success(pageData);
            }

            //剧集信息
            var epInfo = await _videoEpisodeRepository.GetQuery()
                .Include(a => a.VideoEpisodeGroup)
                .ThenInclude(a => a.VideoMain)
                .Select(a => new
                {
                    a.EpisodeName,
                    EpId = a.Id,
                    VideoName = a.VideoEpisodeGroup.VideoMain.KeyWord
                })
                .ToListAsync();
            foreach (var item in pageData.Data)
            {
                var epItem = epInfo.FirstOrDefault(a => a.EpId == item.EpId);
                if (epItem == null)
                {
                    continue;
                }

                item.EpInfo = $"{epItem.VideoName}->{epItem.EpisodeName}";
            }

            return KdyResult.Success(pageData);
        }

        /// <summary>
        /// 批量删除弹幕
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            if (input.Ids == null || input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var dbEp = await _videoDanMuRepository.GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            _videoDanMuRepository.Delete(dbEp);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success("弹幕删除成功");
        }
    }
}
