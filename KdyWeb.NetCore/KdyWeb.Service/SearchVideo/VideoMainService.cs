using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.IService.SearchVideo
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
        }

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
    }
}
