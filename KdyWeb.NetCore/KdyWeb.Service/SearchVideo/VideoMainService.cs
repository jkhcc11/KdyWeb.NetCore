using System.Collections.Generic;
using System.Linq;
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
    /// 影片主表 服务实现
    /// </summary>
    public class VideoMainService : BaseKdyService, IVideoMainService
    {
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;
        private readonly IKdyRepository<DouBanInfo> _douBanInfoRepository;

        public VideoMainService(IKdyRepository<VideoMain, long> videoMainRepository, IKdyRepository<DouBanInfo> douBanInfoRepository, IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            _videoMainRepository = videoMainRepository;
            _douBanInfoRepository = douBanInfoRepository;

            CanUpdateFieldList.AddRange(new[]
            {
                "VideoContentFeature","Subtype","IsEnd","VideoMainStatus","IsMatchInfo","SourceUrl"
            });
        }

        /// <summary>
        /// 通过豆瓣信息创建影片信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateForDouBanInfoAsync(CreateForDouBanInfoInput input)
        {
            //获取豆瓣信息
            var douBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == input.DouBanInfoId);
            if (douBanInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "豆瓣信息Id错误");
            }

            var epName = input.EpisodeGroupType == EpisodeGroupType.VideoPlay ? "极速" : "点击下载";
            //生成影片信息
            var dbVideoMain = new VideoMain(douBanInfo.Subtype, douBanInfo.VideoTitle, douBanInfo.VideoImg, "systeminput", "systeminput");
            dbVideoMain.ToVideoMain(douBanInfo);
            dbVideoMain.EpisodeGroup = new List<VideoEpisodeGroup>()
            {
                new VideoEpisodeGroup(input.EpisodeGroupType,"默认组")
                {
                    Episodes = new List<VideoEpisode>()
                    {
                        new VideoEpisode(epName,input.EpUrl)
                    }
                }
            };
            dbVideoMain.IsMatchInfo = true;
            dbVideoMain.IsEnd = true;
            await _videoMainRepository.CreateAsync(dbVideoMain);

            douBanInfo.DouBanInfoStatus = DouBanInfoStatus.SearchEnd;
            _douBanInfoRepository.Update(douBanInfo);

            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 获取影片信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetVideoDetailDto>> GetVideoDetailAsync(long keyId)
        {
            var main = await _videoMainRepository.GetAsNoTracking()
                .Include(a => a.VideoMainInfo)
                .Include(a => a.EpisodeGroup)
                .ThenInclude(a => a.Episodes)
                .Where(a => a.Id == keyId)
                .FirstOrDefaultAsync();
            if (main == null)
            {
                return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.Error, "keyId错误");
            }

            var result = main.MapToExt<GetVideoDetailDto>();
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 分页查询影视库
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryVideoMainDto>>> QueryVideoMainAsync(QueryVideoMainInput input)
        {
            if (input.OrderBy == null || input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions()
                    {
                        Key = "CreatedTime",
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var pageData = await _videoMainRepository.GetQuery()
                .Include(a => a.VideoMainInfo)
                .GetDtoPageListAsync<VideoMain, QueryVideoMainDto>(input);
            return KdyResult.Success(pageData);
        }

        /// <summary>
        /// 更新字段值
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> UpdateValueByFieldAsync(UpdateValueByFieldInput input)
        {
            var dbMain = await _videoMainRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (dbMain == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "Id错误");
            }

            if (CanUpdateFieldList.Contains(input.Field) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, $"更新失败，当前字段：{input.Field} 不支持更新");
            }

            dbMain.UpdateModelField(input.Field, input.Value);

            _videoMainRepository.Update(dbMain);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除影片
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            if (input.Ids == null || input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var dbEp = await _videoMainRepository.GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            _videoMainRepository.Delete(dbEp);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success("剧集删除成功");
        }
    }
}
