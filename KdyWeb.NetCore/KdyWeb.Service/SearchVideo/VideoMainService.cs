using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 影片主表 服务实现
    /// </summary>
    public class VideoMainService : BaseKdyService, IVideoMainService
    {
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;
        private readonly IKdyRepository<VideoEpisodeGroup, long> _videoEpisodeGroupRepository;
        private readonly IKdyRepository<VideoEpisode, long> _videoEpisodeRepository;

        public VideoMainService(IKdyRepository<VideoMain, long> videoMainRepository, IKdyRepository<VideoEpisodeGroup, long> videoEpisodeGroupRepository, IKdyRepository<VideoEpisode, long> videoEpisodeRepository)
        {
            _videoMainRepository = videoMainRepository;
            _videoEpisodeGroupRepository = videoEpisodeGroupRepository;
            _videoEpisodeRepository = videoEpisodeRepository;
        }

        public async Task<KdyResult> CreateVideoInfoAsync()
        {
            var dbVideoMain = new VideoMain(Subtype.Movie, "测试", "http://www.baidu.com", "systeminput", "systeminput")
            {
                EpisodeGroup = new List<VideoEpisodeGroup>()
                {
                    new VideoEpisodeGroup(0, "测试组", EpisodeGroupType.VideoPlay)
                    {
                        Episodes = new List<VideoEpisode>()
                        {
                            new VideoEpisode(0, "极速", "http://www.baidu.com/1.m3u8"),
                            new VideoEpisode(0, "极速1", "http://www.baidu.com/1.m3u8")

                        }
                    }
                }
            };
            dbVideoMain = await _videoMainRepository.CreateAsync(dbVideoMain);

            await UnitOfWork.SaveChangesAsync();

            await Task.Delay(5000);

            // var dbMain = await _videoMainRepository.FirstOrDefaultAsync(a => a.Id == dbVideoMain.Id);

            dbVideoMain.OrderBy = 6;
            _videoMainRepository.Update(dbVideoMain);

            //dbVideoMain = await _videoMainRepository.CreateAsync(dbVideoMain);
            //var dbVideoEpGroup = new VideoEpisodeGroup(dbVideoMain.Id, "测试组", EpisodeGroupType.VideoPlay);
            //dbVideoEpGroup = await _videoEpisodeGroupRepository.CreateAsync(dbVideoEpGroup);
            //var dbEp = new VideoEpisode(dbVideoEpGroup.Id, "极速", "http://www.baidu.com/1.m3u8");
            //dbEp = await _videoEpisodeRepository.CreateAsync(dbEp);

            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
    }
}
