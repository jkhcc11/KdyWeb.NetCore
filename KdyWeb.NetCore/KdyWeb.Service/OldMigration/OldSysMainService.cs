using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Entity;
using KdyWeb.Entity.OldVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.OldMigration;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.OldMigration
{
    /// <summary>
    /// 旧版影视迁移
    /// </summary>
    public class OldSysMainService : BaseKdyService, IOldSysMainService
    {
        private readonly IKdyRepository<OldSearchSysMain, int> _mainRepository;
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;

        private readonly IKdyRepository<OldSearchSysUser> _oldUserRepository;
        private readonly IKdyRepository<OldUserHistory> _oldUserHistoryRepository;
        private readonly IKdyRepository<KdyUser, long> _kdyUseRepository;
        private readonly IKdyRepository<UserHistory, long> _userHistoryRepository;

        public OldSysMainService(IKdyRepository<OldSearchSysMain, int> mainRepository, IKdyRepository<VideoMain, long> videoMainRepository,
            IUnitOfWork unitOfWork, IKdyRepository<OldSearchSysUser> oldUserRepository, IKdyRepository<OldUserHistory> oldUserHistoryRepository, IKdyRepository<KdyUser, long> kdyUseRepository, IKdyRepository<UserHistory, long> userHistoryRepository) : base(unitOfWork)
        {
            _mainRepository = mainRepository;
            _videoMainRepository = videoMainRepository;
            _oldUserRepository = oldUserRepository;
            _oldUserHistoryRepository = oldUserHistoryRepository;
            _kdyUseRepository = kdyUseRepository;
            _userHistoryRepository = userHistoryRepository;
        }

        public async Task<KdyResult> OldToNewMain(int page, int pageSize)
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
                if (string.IsNullOrEmpty(item.ResultImg))
                {
                    KdyLog.Warn($"主表图片为空跳过，{item.KeyWord} {item.Id}", new Dictionary<string, object>()
                    {
                        {"MainInfo",item}
                    });
                    continue;
                }

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
            if (oldKeyIds.Any() == false)
            {
                KdyLog.Warn($"未发现迁移数据,Page:{page} PageSize:{pageSize}");
                return KdyResult.Error(KdyResultCode.Error, "未发现迁移数据");
            }

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

        public async Task<KdyResult> OldToNewUser(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            //旧用户
            var main = await _oldUserRepository.GetQuery()
                .OrderByDescending(a => a.CreatedTime)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            if (main.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无数据");
            }

            var desKey = KdyConfiguration.GetValue<string>("DesKey");
            //生成新数据
            var newDb = new List<KdyUser>();
            foreach (var item in main)
            {
                var roleId = item.UserRole == 3 ? 3 : 1;
                var pwd = item.UserPwd.DesHexToStr(desKey);

                var userItem = new KdyUser(item.UserName, item.UserNick, item.UserEmail, $"{pwd}{KdyWebConst.UserSalt}".Md5Ext(), roleId)
                {
                    OldUserId = item.Id
                };

                newDb.Add(userItem);
            }

            //待添加的旧值
            var oldKeyIds = newDb.Select(a => a.OldUserId).ToList();
            //数据库已存在的旧值
            var dbOldKeyIds = await _kdyUseRepository.GetQuery()
                .Where(a => oldKeyIds.Contains(a.OldUserId))
                .Select(a => a.OldUserId)
                .ToListAsync();

            var newAddDb = newDb.Where(a => dbOldKeyIds.Contains(a.OldUserId) == false)
                .ToList();

            if (newAddDb.Any())
            {
                await _kdyUseRepository.CreateAsync(newAddDb);
                await UnitOfWork.SaveChangesAsync();
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult> OldToNewUserHistory(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            //旧用户
            var main = await _oldUserHistoryRepository.GetQuery()
                .OrderByDescending(a => a.CreatedTime)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            if (main.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无数据");
            }

            //所有Old影片数据Key 和Old用户数据Key
            var oldMainKeyIds = main.Select(a => a.KeyId).ToList();
            var oldUserIds = main.Select(a => a.UserId).ToList();

            //新影片数据
            var videoMain = await _videoMainRepository.GetQuery()
                .Include(a => a.EpisodeGroup)
                .ThenInclude(a => a.Episodes)
                .Where(a => oldMainKeyIds.Contains(a.OldKeyId))
                .ToListAsync();

            //新用户数据
            var userInfo = await _kdyUseRepository.GetAsNoTracking()
                .Where(a => oldUserIds.Contains(a.OldUserId))
                .ToListAsync();

            //生成新数据
            var newDb = new List<UserHistory>();
            foreach (var item in main)
            {
                //新用户
                var userItem = userInfo.FirstOrDefault(a => a.OldUserId == item.UserId);
                //新影片
                var videoItem = videoMain.FirstOrDefault(a => a.OldKeyId == item.KeyId);
                //新剧集
                var epItem = videoItem?.EpisodeGroup.SelectMany(a => a.Episodes)
                    .FirstOrDefault(a => a.OldEpId == item.EpId);
                if (userItem == null || videoItem == null || epItem == null)
                {
                    continue;
                }

                var historyItem = new UserHistory(userItem.Id, videoItem.Id, epItem.Id)
                {
                    EpName = epItem.EpisodeName,
                    VodName = videoItem.KeyWord,
                    UserName = userItem.UserName,
                    VodUrl = $"/Movie/vod/{epItem.Id}"
                };

                newDb.Add(historyItem);
            }

            if (newDb.Any())
            {
                await _userHistoryRepository.CreateAsync(newDb);
                await UnitOfWork.SaveChangesAsync();
            }

            return KdyResult.Success();
        }
    }
}
