using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Entity.OldVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.OldMigration;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.OldMigration
{
    /// <summary>
    /// 旧版影视迁移
    /// </summary>
    public class OldSysMainService : BaseKdyService, IOldSysMainService
    {
        private readonly IKdyRepository<OldSearchSysMain, int> _mainRepository;
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;

        public OldSysMainService(IKdyRepository<OldSearchSysMain, int> mainRepository, IKdyRepository<VideoMain, long> videoMainRepository, 
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mainRepository = mainRepository;
            _videoMainRepository = videoMainRepository;
        }

        public async Task<KdyResult> OldToNew(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var main = await _mainRepository.GetQuery()
                .Include(a => a.Episodes)
                .OrderByDescending(a => a.CreatedTime)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            if (main.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无数据");
            }

            var newDb = new List<VideoMain>();
            foreach (var item in main)
            {
                var subtype = Subtype.None;
                switch (item.MovieType)
                {
                    case "电影":
                        {
                            subtype = Subtype.Movie;
                            break;
                        }
                    case "电视剧":
                        {
                            subtype = Subtype.Tv;
                            break;
                        }
                    case "记录片":
                        {
                            subtype = Subtype.Documentary;
                            break;
                        }
                    case "综艺":
                        {
                            subtype = Subtype.TvShow;
                            break;
                        }
                    case "动画":
                        {
                            subtype = Subtype.Animation;
                            break;
                        }
                }

                //生成影片信息
                var dbVideoMain = new VideoMain(subtype, item.KeyWord, item.ResultImg, item.ResultUrl, item.VideoContentFeature);
                dbVideoMain.ToVideoMain(item);
                var epGroup = new VideoEpisodeGroup(EpisodeGroupType.VideoPlay, "默认播放组")
                {
                    Episodes = new List<VideoEpisode>()
                };
                foreach (var itemEp in item.Episodes)
                {
                    var temp = new VideoEpisode(itemEp.EpisodeName, itemEp.EpisodeUrl)
                    {
                        OldEpId = itemEp.Id
                    };
                    epGroup.Episodes.Add(temp);
                }

                dbVideoMain.EpisodeGroup = new List<VideoEpisodeGroup> { epGroup };

                dbVideoMain.IsMatchInfo = item.IsMatchInfo == 1;
                dbVideoMain.IsEnd = item.IsEnd == "是";
                newDb.Add(dbVideoMain);
            }

            //待添加的旧值
            var oldKeyIds = newDb.Select(a => a.OldKeyId).ToList();
            //数据库已存在的旧值
            var dbOldKeyIds = await _videoMainRepository.GetQuery()
                .Where(a => oldKeyIds.Contains(a.OldKeyId))
                .Select(a => a.OldKeyId)
                .ToListAsync();

            var newAddDb = newDb.Where(a => dbOldKeyIds.Contains(a.OldKeyId) == false)
                .ToList();

            if (newAddDb.Any())
            {
                await _videoMainRepository.CreateAsync(newAddDb);
                await UnitOfWork.SaveChangesAsync();
            }

            return KdyResult.Success();
        }
    }
}
