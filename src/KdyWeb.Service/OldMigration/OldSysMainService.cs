using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity;
using KdyWeb.Entity.OldVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.OldMigration;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KdyWeb.Service.OldMigration
{
    /// <summary>
    /// 旧版影视迁移
    /// </summary>
    [Obsolete("已升级部分，暂废弃")]
    public class OldSysMainService : BaseKdyService, IOldSysMainService
    {
        private readonly IKdyRepository<OldSearchSysMain> _mainRepository;
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;
        private readonly IKdyRepository<VideoEpisode, long> _videoEpisodeRepository;

        private readonly IKdyRepository<OldSearchSysUser> _oldUserRepository;
        private readonly IKdyRepository<OldUserHistory> _oldUserHistoryRepository;
        private readonly IKdyRepository<KdyUser, long> _kdyUseRepository;
        private readonly IKdyRepository<UserHistory, long> _userHistoryRepository;

        private readonly IKdyRepository<OldUserSubscribe> _oldSubscribeRepository;
        private readonly IKdyRepository<UserSubscribe, long> _userSubscribeRepository;

        private readonly IKdyRepository<OldSearchSysDanMu> _oldSearchSysDanMuRepository;
        private readonly IKdyRepository<VideoDanMu, long> _videoDanMuRepository;

        private readonly IKdyRepository<OldFeedBackInfo> _oldFeedBackInfoRepository;
        private readonly IKdyRepository<FeedBackInfo> _feedBackInfoRepository;

        private readonly IKdyRepository<OldSearchSysSeries> _oldSeriesRepository;
        private readonly IKdyRepository<OldSearchSysSeriesList> _oldSeriesListRepository;
        private readonly IKdyRepository<VideoSeries, long> _videoSeriesRepository;

        public OldSysMainService(IKdyRepository<OldSearchSysMain> mainRepository, IKdyRepository<VideoMain, long> videoMainRepository,
            IUnitOfWork unitOfWork, IKdyRepository<OldSearchSysUser> oldUserRepository, IKdyRepository<OldUserHistory> oldUserHistoryRepository,
            IKdyRepository<KdyUser, long> kdyUseRepository, IKdyRepository<UserHistory, long> userHistoryRepository, IKdyRepository<OldUserSubscribe> oldSubscribeRepository,
            IKdyRepository<UserSubscribe, long> userSubscribeRepository, IKdyRepository<OldSearchSysDanMu> oldSearchSysDanMuRepository, IKdyRepository<VideoDanMu, long> videoDanMuRepository,
            IKdyRepository<VideoEpisode, long> videoEpisodeRepository, IKdyRepository<OldFeedBackInfo> oldFeedBackInfoRepository, IKdyRepository<FeedBackInfo> feedBackInfoRepository,
            IKdyRepository<OldSearchSysSeries> oldSeriesRepository, IKdyRepository<OldSearchSysSeriesList> oldSeriesListRepository, IKdyRepository<VideoSeries, long> videoSeriesRepository) : base(unitOfWork)
        {
            _mainRepository = mainRepository;
            _videoMainRepository = videoMainRepository;
            _oldUserRepository = oldUserRepository;
            _oldUserHistoryRepository = oldUserHistoryRepository;
            _kdyUseRepository = kdyUseRepository;
            _userHistoryRepository = userHistoryRepository;
            _oldSubscribeRepository = oldSubscribeRepository;
            _userSubscribeRepository = userSubscribeRepository;
            _oldSearchSysDanMuRepository = oldSearchSysDanMuRepository;
            _videoDanMuRepository = videoDanMuRepository;
            _videoEpisodeRepository = videoEpisodeRepository;
            _oldFeedBackInfoRepository = oldFeedBackInfoRepository;
            _feedBackInfoRepository = feedBackInfoRepository;
            _oldSeriesRepository = oldSeriesRepository;
            _oldSeriesListRepository = oldSeriesListRepository;
            _videoSeriesRepository = videoSeriesRepository;
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
                    KdyLog.LogWarning("主表图片为空跳过.OldInfo:{item}", JsonConvert.SerializeObject(item));
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
                KdyLog.LogWarning($"未发现迁移数据,Page:{page} PageSize:{pageSize}");
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

                var userItem = new KdyUser(item.UserName, item.UserNick, item.UserEmail, roleId)
                {
                    OldUserId = item.Id
                };
                KdyUser.SetPwd(userItem, pwd);
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

                var historyItem = new UserHistory(videoItem.Id, epItem.Id)
                {
                    EpName = epItem.EpisodeName,
                    VodName = videoItem.KeyWord,
                    UserName = userItem.UserName,
                    VodUrl = $"/Movie/vod/{epItem.Id}",
                    CreatedUserId = userItem.Id
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

        public async Task<KdyResult> OldToNewUserSubscribe(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            //旧用户订阅
            var main = await _oldSubscribeRepository.GetQuery()
                .OrderByDescending(a => a.CreatedTime)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            if (main.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无数据");
            }

            //所有Old影片数据Key 和Old用户数据Key
            var oldMainKeyIds = main.Select(a => a.ObjId).ToList();
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
            var newDb = new List<UserSubscribe>();
            foreach (var item in main)
            {
                //新用户
                var userItem = userInfo.FirstOrDefault(a => a.OldUserId == item.UserId);
                //新影片
                var videoItem = videoMain.FirstOrDefault(a => a.OldKeyId == item.ObjId);
                if (userItem == null || videoItem == null)
                {
                    continue;
                }

                var historyItem = new UserSubscribe(videoItem.Id, videoItem.VideoContentFeature, UserSubscribeType.Vod)
                {
                    CreatedUserId = userItem.Id,
                    UserEmail = userItem.UserEmail
                };

                newDb.Add(historyItem);
            }

            if (newDb.Any())
            {
                await _userSubscribeRepository.CreateAsync(newDb);
                await UnitOfWork.SaveChangesAsync();
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult> OldToNewDanMu(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            //旧用户订阅
            var main = await _oldSearchSysDanMuRepository.GetQuery()
                .OrderByDescending(a => a.CreatedTime)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            if (main.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无数据");
            }

            //所有Old剧集数据Key
            var oldEpIds = main.Select(a => int.Parse(a.DVideoId)).ToList();

            //新剧集数据
            var videoEp = await _videoEpisodeRepository.GetQuery()
                .Where(a => oldEpIds.Contains(a.OldEpId))
                .ToListAsync();

            //生成新数据
            var newDb = new List<VideoDanMu>();
            foreach (var item in main)
            {
                //新剧集
                var epItem = videoEp.FirstOrDefault(a => a.OldEpId == int.Parse(item.DVideoId));
                if (epItem == null)
                {
                    continue;
                }

                var historyItem = new VideoDanMu()
                {
                    DTime = item.DTime,
                    DColor = item.DColor,
                    Msg = item.DMsg,
                    EpId = epItem.Id,
                    DMode = item.DMode,
                    DSize = item.DSize,
                    CreatedTime = item.CreatedTime,
                };
                newDb.Add(historyItem);
            }

            if (newDb.Any())
            {
                await _videoDanMuRepository.CreateAsync(newDb);
                await UnitOfWork.SaveChangesAsync();
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult> OldToNewFeedBackInfo(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            //旧用户订阅
            var main = await _oldFeedBackInfoRepository.GetQuery()
                .OrderByDescending(a => a.CreatedTime)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            if (main.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无数据");
            }

            ////所有Old豆瓣信息Id
            //var oldDouBanObjectId = main.Select(a => a.DouBanId).ToList();

            //旧用户email
            var oldEmail = main.Select(a => a.UserEmail).ToList();
            var userInfo = await _kdyUseRepository.GetQuery()
                .Where(a => oldEmail.Contains(a.UserEmail))
                .ToListAsync();

            //生成新数据
            var newDb = new List<FeedBackInfo>();
            foreach (var item in main)
            {
                //新用户
                var userItem = userInfo.FirstOrDefault(a => a.UserEmail == item.UserEmail);
                if (userItem == null)
                {
                    continue;
                }

                //影片信息
                //var videoItem = videoInfo.FirstOrDefault(a => a.VideoInfoUrl.Contains(item.DouBanId));
                //新影视数据
                var videoInfo = await _videoMainRepository.GetQuery()
                    .Where(a => a.VideoInfoUrl.Contains(item.DouBanId))
                    .Select(a => new
                    {
                        a.KeyWord,
                        a.VideoYear,
                        a.VideoInfoUrl
                    })
                    .FirstOrDefaultAsync();
                if (videoInfo == null)
                {
                    continue;
                }

                var newItem = new FeedBackInfo(UserDemandType.Input, item.Name)
                {
                    CreatedTime = item.CreatedTime,
                    CreatedUserId = userItem.Id,
                    VideoName = videoInfo.KeyWord
                };

                if (newItem.OriginalUrl.StartsWith("http:") ||
                    newItem.OriginalUrl.StartsWith("https:"))
                {
                    newItem.OriginalUrl = newItem.OriginalUrl.Replace("http:", "")
                        .Replace("https:", "");
                }

                //switch (item.Status)
                //{
                //    case "已忽略":
                //        {
                //            newItem.FeedBackInfoStatus = FeedBackInfoStatus.Ignore;
                //            break;
                //        }
                //    case "待审核":
                //        {
                //            newItem.FeedBackInfoStatus = FeedBackInfoStatus.Pending;
                //            break;
                //        }
                //    case "正常":
                //        {
                //            newItem.FeedBackInfoStatus = FeedBackInfoStatus.Processing;
                //            break;
                //        }
                //    case "资源录入完毕":
                //        {
                //            newItem.FeedBackInfoStatus = FeedBackInfoStatus.Normal;
                //            break;
                //        }
                //}

                newDb.Add(newItem);
            }

            if (newDb.Any())
            {
                await _feedBackInfoRepository.CreateAsync(newDb);
                await UnitOfWork.SaveChangesAsync();
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult> OldToNewSeries(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var main = await _oldSeriesRepository.GetQuery()
                .OrderByDescending(a => a.CreatedTime)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            if (main.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无数据");
            }

            //系列列表
            var allSeries = await _oldSeriesListRepository.GetAsNoTracking()
                .Where(a => main.Select(b => b.Id).Contains(a.SeriesId))
                .ToListAsync();

            //影片列表
            var oldIds = allSeries.Select(a => a.KeyId)
                .ToList();
            var allVideo = await _videoMainRepository.GetAsNoTracking()
                .Where(a => oldIds.Contains(a.OldKeyId))
                .ToListAsync();

            var newDb = new List<VideoSeries>();
            foreach (var item in main)
            {
                //生成系列信息
                var dbSeries = new VideoSeries(item.SeriesName, item.SeriesImg)
                {
                    SeriesRemark = item.SeriesRemark,
                    OrderBy = item.OrderBy,
                    LiveUrl = item.LiveUrl,
                    SeriesDesUrl = item.SeriesDesUrl,
                    SeriesList = new List<VideoSeriesList>()
                };
                if (string.IsNullOrEmpty(dbSeries.SeriesImg) == false &&
                    (dbSeries.SeriesImg.StartsWith("http:") ||
                     dbSeries.SeriesImg.StartsWith("https:")))
                {
                    dbSeries.SeriesImg = dbSeries.SeriesImg.Replace("http:", "")
                        .Replace("https:", "");
                }

                //当前系列列表
                var itemList = allSeries.Where(a => a.SeriesId == item.Id)
                    .ToList();
                foreach (var tempItem in itemList)
                {
                    var videoItem = allVideo.FirstOrDefault(a => a.OldKeyId == tempItem.KeyId);
                    if (videoItem == null)
                    {
                        continue;
                    }

                    dbSeries.SeriesList.Add(new VideoSeriesList()
                    {
                        OldKeyId = tempItem.KeyId,
                        KeyId = videoItem.Id
                    });
                }

                if (dbSeries.SeriesList.Any())
                {
                    newDb.Add(dbSeries);
                }
            }

            if (newDb.Any())
            {
                await _videoSeriesRepository.CreateAsync(newDb);
                await UnitOfWork.SaveChangesAsync();
            }

            return KdyResult.Success();
        }
    }
}
