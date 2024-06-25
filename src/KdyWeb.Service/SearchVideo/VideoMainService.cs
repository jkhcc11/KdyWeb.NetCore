﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.Entity.VideoConverts.Enum;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Service.Job;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Snowflake.Core;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 影片主表 服务实现
    /// </summary>
    public class VideoMainService : BaseKdyService, IVideoMainService
    {
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;
        private readonly IKdyRepository<DouBanInfo> _douBanInfoRepository;
        private readonly IKdyRepository<UserSubscribe, long> _userSubscribeRepository;
        private readonly IKdyRepository<VideoMainInfo, long> _videoMainInfoRepository;
        private readonly IKdyRepository<UserHistory, long> _userHistoryRepository;
        private readonly IKdyRepository<VideoSeriesList, long> _videoSeriesListRepository;
        private readonly IKdyRepository<VideoSeries, long> _videoSeriesRepository;
        private readonly IdWorker _idWorker;

        public VideoMainService(IKdyRepository<VideoMain, long> videoMainRepository, IKdyRepository<DouBanInfo> douBanInfoRepository,
            IUnitOfWork unitOfWork, IKdyRepository<UserSubscribe, long> userSubscribeRepository,
            IKdyRepository<VideoMainInfo, long> videoMainInfoRepository, IKdyRepository<UserHistory, long> userHistoryRepository,
            IKdyRepository<VideoSeriesList, long> videoSeriesListRepository, IKdyRepository<VideoSeries, long> videoSeriesRepository,
            IdWorker idWorker) :
            base(unitOfWork)
        {
            _videoMainRepository = videoMainRepository;
            _douBanInfoRepository = douBanInfoRepository;
            _userSubscribeRepository = userSubscribeRepository;
            _videoMainInfoRepository = videoMainInfoRepository;
            _userHistoryRepository = userHistoryRepository;
            _videoSeriesListRepository = videoSeriesListRepository;
            _videoSeriesRepository = videoSeriesRepository;
            _idWorker = idWorker;

            CanUpdateFieldList.AddRange(new[]
            {
                "VideoContentFeature","Subtype",
                "IsEnd","VideoMainStatus","IsMatchInfo","SourceUrl"
            });
        }

        /// <summary>
        /// 通过豆瓣信息创建影片信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<CreateForDouBanInfoDto>> CreateForDouBanInfoAsync(CreateForDouBanInfoInput input)
        {
            //获取豆瓣信息
            var douBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == input.DouBanInfoId);
            if (douBanInfo == null)
            {
                return KdyResult.Error<CreateForDouBanInfoDto>(KdyResultCode.Error, "豆瓣信息Id错误");
            }

            //是否存在
            var anyVideoMain = await _videoMainRepository
                .GetAsNoTracking()
                .AnyAsync(a => a.VideoInfoUrl != null &&
                            a.VideoInfoUrl.Contains(douBanInfo.VideoDetailId));
            if (anyVideoMain)
            {
                return KdyResult.Error<CreateForDouBanInfoDto>(KdyResultCode.Error, "影片已存在 创建失败");
            }

            //剧集列表
            var episodes = input.EpUrl
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(GetVideoEpisodeByString)
                .ToList();

            //生成影片信息
            var dbVideoMain = new VideoMain(douBanInfo.Subtype, douBanInfo.VideoTitle, douBanInfo.VideoImg,
                VideoMain.SystemInput, VideoMain.SystemInput);
            dbVideoMain.ToVideoMain(douBanInfo);
            dbVideoMain.EpisodeGroup = new List<VideoEpisodeGroup>()
            {
                new VideoEpisodeGroup(input.EpisodeGroupType,"默认组")
                {
                    Episodes = episodes
                }
            };
            dbVideoMain.IsMatchInfo = true;
            dbVideoMain.IsEnd = true;
            await _videoMainRepository.CreateAsync(dbVideoMain);

            douBanInfo.SetSearchEnd();
            //douBanInfo.DouBanInfoStatus = DouBanInfoStatus.SearchEnd;
            _douBanInfoRepository.Update(douBanInfo);
            await UnitOfWork.SaveChangesAsync();

            #region 用户行为记录
            var recordType = VodManagerRecordType.SaveMove;
            if (episodes.Count > 10)
            {
                recordType = VodManagerRecordType.SaveTv;
            }
            var jobInput = new CreateVodManagerRecordInput(LoginUserInfo.GetUserId(), recordType)
            {
                BusinessId = dbVideoMain.Id,
                Remark = $"剧集更新数量：{episodes.Count}",
                LoginUserName = LoginUserInfo.UserName
            };
            BackgroundJob.Enqueue<CreateVodManagerRecordJobService>(a => a.ExecuteAsync(jobInput));

            await CreateVodManagerRecordAsync(dbVideoMain, douBanInfo);
            #endregion

            var result = dbVideoMain.MapToExt<CreateForDouBanInfoDto>();
            return KdyResult.Success(result);
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
            if (main.IsLoginView() &&
                LoginUserInfo.IsLogin == false)
            {
                //屏蔽分组
                result.EpisodeGroup = new();
            }
            else
            {
                //普通用户clear
                var isClear = LoginUserInfo.IsSuperAdmin == false &&
                              LoginUserInfo.IsVodAdmin == false;
                result.EpisodeGroup = result.EpisodeGroup.OrderByExt(isClear);
            }

            result.ImgHandler();
            if (LoginUserInfo.IsLogin)
            {
                //登录用户就获取最新历史记录
                var dbNewHistory = await _userHistoryRepository.GetAsNoTracking()
                    .Where(a => a.KeyId == keyId && a.CreatedUserId == LoginUserInfo.UserId)
                    .OrderByDescending(a => a.ModifyTime)
                    .ThenByDescending(a => a.CreatedTime)
                    .FirstOrDefaultAsync();
                result.NewUserHistory = dbNewHistory?.MapToExt<QueryUserHistoryDto>();

                //是否订阅
                var subscribeId = await _userSubscribeRepository.GetAsNoTracking()
                    .Where(a => a.BusinessId == main.Id &&
                                a.UserSubscribeType == UserSubscribeType.Vod &&
                                a.CreatedUserId == LoginUserInfo.UserId)
                    .Select(a => a.Id)
                    .FirstOrDefaultAsync();

                result.SubscribeId = subscribeId;
                result.IsSubscribe = subscribeId != default;
            }

            //影片所属系列
            var dbVideoSeries = await _videoSeriesListRepository.GetAsNoTracking()
                .Include(a => a.VideoSeries)
                .Where(a => a.KeyId == keyId)
                .Select(a => a.VideoSeries)
                .FirstOrDefaultAsync();
            result.VideoSeries = dbVideoSeries?.MapToExt<QueryVideoSeriesDto>();

            if (LoginUserInfo.IsSuperAdmin == false)
            {
                //非超管
                result.SourceUrl = result.SourceUrl.StrToHex();
            }

            #region 自动匹配豆瓣
            if (main.IsMatchInfo == false)
            {
                var autoMatchInput = new AutoMatchDouBanInfoJobInput()
                {
                    MainId = main.Id,
                    VodTitle = main.KeyWord,
                    VodYear = main.VideoYear
                };
                BackgroundJob.Enqueue<AutoMatchDouBanInfoJobService>(a => a.ExecuteAsync(autoMatchInput));
            }
            #endregion

            #region 自动匹配资源
            if (main.IsMatchZy())
            {
                var autoMatchInput = new AutoMatchDouBanInfoJobInput()
                {
                    MainId = main.Id,
                    VodTitle = main.KeyWord,
                    VodYear = main.VideoYear
                };
                BackgroundJob.Enqueue<AutoMatchMovieInfoByZyJobService>(a => a.ExecuteAsync(autoMatchInput));
            }
            #endregion

            if (result.IsEnd)
            {
                //已完结 不用更新
                return KdyResult.Success(result);
            }

            //var jobInput = new UpdateNotEndVideoMainJobInput(main.Id, main.SourceUrl, main.VideoContentFeature)
            //{
            //    KeyWord = main.KeyWord
            //};
            //BackgroundJob.Enqueue<UpdateNotEndVideoJobService>(a => a.ExecuteAsync(jobInput));
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
                        Key = nameof(VideoMain.OrderBy),
                        OrderBy = KdyEfOrderBy.Desc
                    },
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(VideoMain.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            //生成条件和排序规则
            var query = _videoMainRepository.GetQuery()
                .Include(a => a.VideoMainInfo)
                .CreateConditions(input);
            if (input.VideoCountries.HasValue)
            {
                var str = input.VideoCountries.Value.GetDisplayName();
                query = query.Where(a => a.VideoMainInfo.VideoCountries.Contains(str));
            }

            switch (input.SearchType)
            {
                case SearchType.IsNoEnd:
                    {
                        query = query.Where(a => a.IsEnd == false);
                        break;
                    }
                case SearchType.IsToday:
                    {
                        query = query.Where(a => a.ModifyTime != null &&
                        a.ModifyTime.Value.Date == DateTime.Today);
                        break;
                    }
                case SearchType.IsNoMatchDouBan:
                    {
                        query = query.Where(a => a.IsMatchInfo == false);
                        break;
                    }
                case SearchType.IsNarrateUrl:
                    {
                        query = query.Where(a => string.IsNullOrEmpty(a.VideoMainInfo.NarrateUrl) == false);
                        break;
                    }
                case SearchType.LowScore:
                    {
                        query = query.Where(a => a.VideoDouBan <= VideoMain.LowScoreStandard &&
                                                 (a.SourceUrl != VideoMain.SystemInput ||
                                                 a.VideoContentFeature != VideoMain.SystemInput) &&
                                                 a.SourceUrl.Contains(VideoMain.ZyFlag) == false);
                        //高分优先
                        input.OrderBy = new List<KdyEfOrderConditions>()
                        {
                            new KdyEfOrderConditions()
                            {
                                Key = nameof(VideoMain.VideoDouBan),
                                OrderBy = KdyEfOrderBy.Desc
                            },
                            new KdyEfOrderConditions()
                            {
                                Key = nameof(VideoMain.CreatedTime),
                                OrderBy = KdyEfOrderBy.Desc
                            }
                        };
                        break;
                    }
                case SearchType.ToBeMaintained:
                    {
                        query = query.Where(a => (a.SourceUrl != VideoMain.SystemInput ||
                                                 a.VideoContentFeature != VideoMain.SystemInput) &&
                                                 a.SourceUrl.Contains(VideoMain.ZyFlag) == false);
                        break;
                    }
                case SearchType.ZyFlag:
                    {
                        query = query.Where(a => a.SourceUrl.Contains(VideoMain.ZyFlag));
                        break;
                    }
            }

            var count = await query.CountAsync();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                //关键字不为空时 按照长度排序
                query = query
                   .OrderBy(a => a.KeyWord.Length)
                   .KdyThenOrderBy(input)
                   .KdyPageList(input);
            }
            else
            {
                query = query.KdyOrderBy(input).KdyPageList(input);
            }

            var data = await query.ToListAsync();
            var result = new PageList<QueryVideoMainDto>(input.Page, input.PageSize)
            {
                DataCount = count,
                Data = data.MapToListExt<QueryVideoMainDto>()
            };

            foreach (var item in result.Data)
            {
                item.ImgHandler();
            }
            return KdyResult.Success(result);
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

        /// <summary>
        /// 匹配豆瓣信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> MatchDouBanInfoAsync(MatchDouBanInfoInput input)
        {
            var dbMain = await _videoMainRepository
                .GetQuery()
                .Include(a => a.VideoMainInfo)
                .FirstOrDefaultAsync(a => a.Id == input.KeyId);
            if (dbMain == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "影片信息不存在");
            }

            if (dbMain.IsMatchInfo)
            {
                return KdyResult.Error(KdyResultCode.Error, "影片已匹配，匹配失败");
            }

            var dbDouBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == input.DouBanId);
            if (dbDouBanInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "豆瓣信息不存在");
            }

            if (dbDouBanInfo.VideoCountries.IsEmptyExt())
            {
                return KdyResult.Error(KdyResultCode.Error, "豆瓣信息缺少国家，匹配信息");
            }

            dbMain.ToVideoMain(dbDouBanInfo);
            dbMain.VideoImg = dbDouBanInfo.VideoImg;
            if (dbDouBanInfo.VideoCountries.IsEmptyExt() == false &&
                dbDouBanInfo.VideoDirectors.IsEmptyExt() == false)
            {
                dbMain.SetMatchDouBanInfo();
            }

            if (dbMain.IsEnd == false)
            {
                //间隔大于1年 直接完结
                var isEnd = (DateTime.Now.Year - dbDouBanInfo.VideoYear) >= 1;
                dbMain.SetSysInput(isEnd);
            }

            switch (dbMain.VideoDouBan)
            {
                case 0:
                    //todo:2024匹配豆瓣时 评分为0自动删除
                    dbMain.IsDelete = true;
                    break;
                case <= 3:
                case <= 5 when
                    dbDouBanInfo.RatingsCount is < 1000:
                    {
                        dbMain.SetDown();
                        break;
                    }
            }

            _videoMainRepository.Update(dbMain);

            dbDouBanInfo.SetSearchEnd();
            //dbDouBanInfo.DouBanInfoStatus = DouBanInfoStatus.SearchEnd;
            _douBanInfoRepository.Update(dbDouBanInfo);

            await UnitOfWork.SaveChangesAsync();

            await CreateVodManagerRecordAsync(dbMain, dbDouBanInfo);
            return KdyResult.Success();
        }

        /// <summary>
        /// 更新影片主表信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyVideoMainAsync(ModifyVideoMainInput input)
        {
            var main = await _videoMainRepository.GetWriteQuery()
                .Include(a => a.VideoMainInfo)
                .Include(a => a.EpisodeGroup)
                .ThenInclude(a => a.Episodes)
                .Where(a => a.Id == input.Id)
                .FirstOrDefaultAsync();
            if (main == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "keyId错误");
            }

            input.MapToPartExt(main);
            if (input.VideoCountries.IsEmptyExt() == false &&
                input.VideoDirectors.IsEmptyExt() == false)
            {
                main.SetMatchDouBanInfo();
            }

            main.EpisodeGroup ??= new List<VideoEpisodeGroup>();

            #region 剧集下载更新
            var dbDownEpGroup = main.EpisodeGroup.FirstOrDefault(a => a.EpisodeGroupType == EpisodeGroupType.VideoDown);
            var epItemWithDown = input.EpisodeGroup
                   .FirstOrDefault(a => a.EpisodeGroupType == EpisodeGroupType.VideoDown &&
                               a.Episodes != null &&
                               a.Episodes.Any());
            var validItemWithDown = epItemWithDown?.Episodes
                .Where(a => string.IsNullOrEmpty(a.EpisodeName) == false &&
                            string.IsNullOrEmpty(a.EpisodeUrl) == false)
                .ToList();
            if (epItemWithDown != null && validItemWithDown.Any())
            {
                var groupId = _idWorker.NextId();
                if (dbDownEpGroup == null)
                {
                    //剧集组为空新增
                    dbDownEpGroup = new VideoEpisodeGroup(EpisodeGroupType.VideoDown, epItemWithDown.GroupName)
                    {
                        Episodes = validItemWithDown.Select(item => new VideoEpisode(item.EpisodeName,
                            item.EpisodeUrl)
                        {
                            EpisodeGroupId = groupId
                        }).ToList(),
                        Id = groupId
                    };
                    main.EpisodeGroup.Add(dbDownEpGroup);
                }
                else
                {
                    //剧集组更新
                    dbDownEpGroup.GroupName = epItemWithDown.GroupName;
                    validItemWithDown.EpisodeUpdate(dbDownEpGroup.Episodes, dbDownEpGroup);
                }
            }
            #endregion

            #region 播放更新
            var dbPlayEpGroup = main.EpisodeGroup.FirstOrDefault(a => a.EpisodeGroupType == EpisodeGroupType.VideoPlay);
            var epItemWithPlay = input.EpisodeGroup
                .FirstOrDefault(a => a.EpisodeGroupType == EpisodeGroupType.VideoPlay &&
                                     a.Episodes != null &&
                                     a.Episodes.Any());
            var validItemWithPlay = epItemWithPlay?.Episodes
                .Where(a => string.IsNullOrEmpty(a.EpisodeName) == false &&
                            string.IsNullOrEmpty(a.EpisodeUrl) == false)
                .ToList();
            if (epItemWithPlay != null && validItemWithPlay.Any())
            {
                var groupId = _idWorker.NextId();
                if (dbPlayEpGroup == null)
                {
                    //剧集组为空新增
                    dbPlayEpGroup = new VideoEpisodeGroup(EpisodeGroupType.VideoPlay, epItemWithPlay.GroupName)
                    {
                        Episodes = validItemWithPlay.Select(item => new VideoEpisode(item.EpisodeName,
                            item.EpisodeUrl)
                        {
                            EpisodeGroupId = groupId
                        }).ToList(),
                        Id = groupId
                    };
                    main.EpisodeGroup.Add(dbPlayEpGroup);
                }
                else
                {
                    //剧集组更新
                    dbPlayEpGroup.GroupName = epItemWithPlay.GroupName;
                    validItemWithPlay.EpisodeUpdate(dbPlayEpGroup.Episodes, dbPlayEpGroup);
                }
            }
            #endregion

            _videoMainRepository.Update(main);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取影片统计信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetCountInfoBySubtypeDto>>> GetCountInfoBySubtypeAsync(GetCountInfoBySubtypeInput input)
        {
            var query = _videoMainRepository.GetAsNoTracking();
            if (input.StartTime != null)
            {
                query = query.Where(a => a.CreatedTime >= input.StartTime.Value);
            }

            if (input.EndTime != null)
            {
                query = query.Where(a => a.CreatedTime <= input.EndTime.Value);
            }

            var dbCount = await query
                .GroupBy(a => a.Subtype)
                .Select(a => new GetCountInfoBySubtypeDto
                {
                    Subtype = a.Key,
                    Count = a.Count()
                })
                .ToListAsync();

            return KdyResult.Success(dbCount);
        }

        /// <summary>
        /// 强制同步影片主表
        /// </summary>
        /// <param name="mainId">影片Id</param>
        /// <returns></returns>
        public async Task<KdyResult> ForceSyncVideoMainAsync(long mainId)
        {
            var main = await _videoMainRepository.GetAsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == mainId);
            if (main == null)
            {
                return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.Error, "keyId错误");
            }

            if (main.VideoContentFeature == VideoMain.SystemInput &&
                main.SourceUrl == VideoMain.SystemInput)
            {
                return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.Error, "无需操作");
            }
            //if (main.IsEnd)
            //{
            //    return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.Error, "影片已完结，无需同步");
            //}

            main.IsEnd = true;
            main.VideoContentFeature = VideoMain.SystemInput;
            main.SourceUrl = VideoMain.SystemInput;
            _videoMainRepository.Update(main);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();

            //var db = KdyRedisCache.GetDb(1);
            //await db.KeyDeleteAsync($"{KdyServiceCacheKey.NotEndKey}:{mainId}");

            //var jobInput = new UpdateNotEndVideoMainJobInput(main.Id, main.SourceUrl, main.VideoContentFeature)
            //{
            //    KeyWord = main.KeyWord
            //};
            //var jobId = BackgroundJob.Enqueue<UpdateNotEndVideoJobService>(a => a.ExecuteAsync(jobInput));

            //return KdyResult.Success($"任务Id:{jobId} 已添加");

        }

        /// <summary>
        /// 查询同演员影片列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<QuerySameVideoByActorDto>>> QuerySameVideoByActorAsync(QuerySameVideoByActorInput input)
        {
            //先获取数据库10条 然后程序处理 数据库用的是字符串 无法精确匹配 eg: input:张三  =>  db:张三1  也可以搜索
            var query = _videoMainInfoRepository.GetAsNoTracking()
                .Include(a => a.VideoMain)
                .Where(a => a.VideoCasts.Contains(input.Actor) ||
                          a.VideoDirectors.Contains(input.Actor));
            var dbResult = await query.Take(10)
                .OrderBy(a => Guid.NewGuid())
                .ToListAsync();

            var result = new List<QuerySameVideoByActorDto>();
            foreach (var item in dbResult)
            {
                if (result.Count >= 6)
                {
                    //最多6条
                    continue;
                }

                if (item.VideoCasts.IsEmptyExt() == false &&
                    item.VideoCasts.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).Any(a => a == input.Actor))
                {
                    var temp = item.MapToExt<QuerySameVideoByActorDto>();
                    temp.ImgHandler();
                    result.Add(temp);
                    continue;
                }

                if (item.VideoDirectors.IsEmptyExt() == false &&
                    item.VideoDirectors.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).Any(a => a == input.Actor))
                {
                    var temp = item.MapToExt<QuerySameVideoByActorDto>();
                    temp.ImgHandler();
                    result.Add(temp);
                }
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 分页查询影视库(普通查询)
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryVideoMainDto>>> QueryVideoByNormalAsync(QueryVideoByNormalInput input)
        {
            if (input.Genres.IsEmptyExt() == false &&
                KdyBaseConst.GetAllowGenreArray().Contains(input.Genres) == false)
            {
                return KdyResult.Error<PageList<QueryVideoMainDto>>(KdyResultCode.ParError, "参数错误,genres");
            }

            if (input.OrderBy == null || input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(VideoMain.OrderBy),
                        OrderBy = KdyEfOrderBy.Desc
                    },
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(VideoMain.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }else if (input.OrderBy.Any())
            {
                var allowFiled = new List<string>()
                {
                    nameof(VideoMain.OrderBy).ToLower(), 
                    nameof(VideoMain.CreatedTime).ToLower(),
                    nameof(VideoMain.VideoDouBan).ToLower(),
                    nameof(VideoMain.VideoYear).ToLower(),
                };
                var inputKey = input.OrderBy.Select(a => a.Key.ToLower()).ToList();
                if (allowFiled.Intersect(inputKey).Count() != inputKey.Count)
                {
                    return KdyResult.Error<PageList<QueryVideoMainDto>>(KdyResultCode.ParError, "参数错误,orderBy");
                }
            }

            //生成条件和排序规则
            var query = _videoMainRepository.GetQuery()
                .Where(a => a.VideoMainStatus != VideoMainStatus.Down)
                .CreateConditions(input);
            if (input.VideoCountries.HasValue)
            {
                var str = input.VideoCountries.Value.GetDisplayName();
                query = query.Where(a => a.VideoMainInfo.VideoCountries.Contains(str));
            }

            var minYear = DateTime.Today.AddYears(-15).Year;
            if (input.Year.HasValue)
            {
                if (input.Year is -1 ||
                    minYear > input.Year)
                {

                    query = query.Where(a => a.VideoYear < minYear);
                }
                else
                {
                    query = query.Where(a => a.VideoYear == input.Year.Value);
                }
            }

            var count = await query.CountAsync();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                //关键字不为空时 按照长度排序
                query = query
                   .OrderBy(a => a.KeyWord.Length)
                   .KdyThenOrderBy(input)
                   .KdyPageList(input);
            }
            else
            {
                query = query.KdyOrderBy(input).KdyPageList(input);
            }

            var data = await query.ToListAsync();
            if (data.Any() == false &&
                string.IsNullOrEmpty(input.KeyWord) == false)
            {
                KdyLog.LogWarning($"用户搜索关键字：{input.KeyWord},未搜索到。请留意");
            }

            var result = new PageList<QueryVideoMainDto>(input.Page, input.PageSize)
            {
                DataCount = count,
                Data = data.MapToListExt<QueryVideoMainDto>()
            };

            foreach (var item in result.Data)
            {
                item.SourceUrl = string.Empty;
                item.ImgHandler();
            }
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 随机影片(普通查询)
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<IList<QueryVideoMainDto>>> RandVideoByNormalAsync(int count)
        {
            if (count > 50 ||
                count <= 0)
            {
                count = 12;
            }

            //生成条件和排序规则
            var dbData = await _videoMainRepository.GetQuery()
                .Include(a => a.VideoMainInfo)
                .Where(a => a.VideoDouBan > 0 &&
                          a.VideoContentFeature == VideoMain.SystemInput)
                .OrderBy(a => Guid.NewGuid())
                .Take(count)
                .ToListAsync();

            var result = dbData.MapToListExt<QueryVideoMainDto>();
            foreach (var item in result)
            {
                item.SourceUrl = string.Empty;
                item.ImgHandler();
            }
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 上|下架影片
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> UpAndDownVodAsync(long mainId)
        {
            if (LoginUserInfo.IsNormal)
            {
                return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.Error, "操作失败,无权");
            }

            var main = await _videoMainRepository.FirstOrDefaultAsync(a => a.Id == mainId);
            if (main == null)
            {
                return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.ParError, "keyId错误");
            }

            if (main.VideoMainStatus == VideoMainStatus.Down)
            {
                main.SetUp();
            }
            else
            {
                main.SetDown();
            }

            _videoMainRepository.Update(main);
            await UnitOfWork.SaveChangesAsync();

            var jobInput = new CreateVodManagerRecordInput(LoginUserInfo.GetUserId(), VodManagerRecordType.Down)
            {
                BusinessId = mainId,
                Remark = $"影片名：{main.KeyWord}",
                LoginUserName = LoginUserInfo.UserName
            };
            BackgroundJob.Enqueue<CreateVodManagerRecordJobService>(a => a.ExecuteAsync(jobInput));
            return KdyResult.Success();
        }

        /// <summary>
        /// 通过豆瓣信息创建影片信息（新版）
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateForDouBanInfoNewAsync(CreateForDouBanInfoNewInput input)
        {
            if (input.EpItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "剧集不能为空");
            }

            //获取豆瓣信息
            var douBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == input.DouBanInfoId);
            if (douBanInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "豆瓣信息Id错误");
            }

            //是否存在
            var anyVideoMain = await _videoMainRepository
                .GetAsNoTracking()
                .AnyAsync(a => a.VideoInfoUrl != null &&
                            a.VideoInfoUrl.Contains(douBanInfo.VideoDetailId));
            if (anyVideoMain)
            {
                return KdyResult.Error(KdyResultCode.Error, "影片已存在 创建失败");
            }

            //生成影片信息
            var dbVideoMain = new VideoMain(douBanInfo.Subtype,
                douBanInfo.VideoTitle, douBanInfo.VideoImg,
                VideoMain.SystemInput, VideoMain.SystemInput);
            dbVideoMain.ToVideoMain(douBanInfo);
            dbVideoMain.EpisodeGroup = new List<VideoEpisodeGroup>()
            {
                new(input.EpisodeGroupType,"默认组")
                {
                    Episodes = input.EpItems.Select(item => new VideoEpisode(item.EpisodeName,item.EpisodeUrl)
                    {
                        OrderBy = item.OrderBy
                    }).ToList()
                }
            };
            dbVideoMain.IsMatchInfo = true;
            dbVideoMain.IsEnd = true;
            await _videoMainRepository.CreateAsync(dbVideoMain);

            douBanInfo.SetSearchEnd();
            _douBanInfoRepository.Update(douBanInfo);

            if (input.SeriesId.HasValue)
            {
                await CreateSeriesListAsync(dbVideoMain.Id, input.SeriesId.Value);
            }
            await UnitOfWork.SaveChangesAsync();

            #region 用户行为记录
            var recordType = VodManagerRecordType.SaveMove;
            if (input.EpItems.Count > 10)
            {
                recordType = VodManagerRecordType.SaveTv;
            }
            var jobInput = new CreateVodManagerRecordInput(LoginUserInfo.GetUserId(), recordType)
            {
                BusinessId = dbVideoMain.Id,
                Remark = $"影片名：{dbVideoMain.KeyWord}。剧集更新数量：{input.EpItems.Count}",
                LoginUserName = LoginUserInfo.UserName
            };
            BackgroundJob.Enqueue<CreateVodManagerRecordJobService>(a => a.ExecuteAsync(jobInput));

            await CreateVodManagerRecordAsync(dbVideoMain, douBanInfo);
            #endregion
            return KdyResult.Success();
        }

        /// <summary>
        /// 通过豆瓣信息更新影片信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> UpdateVodForDouBanInfoAsync(UpdateVodForDouBanInfoInput input)
        {
            #region 校验
            if (input.EpItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "剧集不能为空");
            }

            //获取豆瓣信息
            var douBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == input.DouBanInfoId);
            if (douBanInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "豆瓣信息Id错误");
            }

            if (douBanInfo.VideoCountries.IsEmptyExt())
            {
                return KdyResult.Error(KdyResultCode.Error, "豆瓣信息缺少国家，匹配信息");
            }

            //影片信息
            var dbVideoMain = await _videoMainRepository.GetQuery()
                .Include(a => a.VideoMainInfo)
                .Include(a => a.EpisodeGroup)
                .ThenInclude(a => a.Episodes)
                .Where(a => a.Id == input.VodId)
                .FirstOrDefaultAsync();
            if (dbVideoMain == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "影片不存在，更新失败");
            }

            //分组剧集
            var epGroup = dbVideoMain.EpisodeGroup?.FirstOrDefault(a => a.Id == input.EpGroupId);
            if (epGroup == null ||
                epGroup.Episodes == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "分组不存在，更新失败");
            }
            #endregion

            #region 豆瓣信息匹配
            dbVideoMain.ToVideoMain(douBanInfo);
            if (douBanInfo.VideoCountries.IsEmptyExt() == false &&
                douBanInfo.VideoDirectors.IsEmptyExt() == false)
            {
                dbVideoMain.SetMatchDouBanInfo();
            }

            if (dbVideoMain.IsEnd == false)
            {
                //间隔大于1年 直接完结
                var isEnd = (DateTime.Now.Year - douBanInfo.VideoYear) >= 1;
                dbVideoMain.SetSysInput(isEnd);
            }
            else
            {
                dbVideoMain.SetSysInput(true);
            }
            #endregion

            #region 更新剧集
            var inputEpIds = input.EpItems.Select(a => a.Id).ToArray();
            //先移除不存在的
            var notExist = epGroup.Episodes
                .Where(a => inputEpIds.Contains(a.Id) == false)
                .ToList();
            if (notExist.Any())
            {
                //软删
                var notExistIds = notExist.Select(a => a.Id).ToArray();
                foreach (var notItem in epGroup.Episodes.Where(a => notExistIds.Contains(a.Id)))
                {
                    notItem.IsDelete = true;
                }
            }

            //更新
            foreach (var epItem in epGroup.Episodes)
            {
                var currentInput = input.EpItems.FirstOrDefault(a => a.Id == epItem.Id);
                if (currentInput == null)
                {
                    //todo:暂时不考虑 新增剧集
                    continue;
                }

                epItem.EpisodeName = currentInput.EpisodeName;
                epItem.OrderBy = currentInput.OrderBy;
                epItem.EpisodeUrl = currentInput.EpisodeUrl;
            }
            #endregion

            douBanInfo.SetSearchEnd();
            _douBanInfoRepository.Update(douBanInfo);
            _videoMainRepository.Update(dbVideoMain);
            if (input.SeriesId.HasValue)
            {
                await CreateSeriesListAsync(dbVideoMain.Id, input.SeriesId.Value);
            }

            await UnitOfWork.SaveChangesAsync();
            await CreateVodManagerRecordAsync(dbVideoMain, douBanInfo);
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取首页配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<HomeDataItem>>> GetHomeDataAsync()
        {
            var dbMovie = await _videoMainRepository.GetAsNoTracking()
                .Where(a => a.VideoMainStatus == VideoMainStatus.Normal &&
                            a.Subtype == Subtype.Movie)
                .OrderByDescending(a => a.OrderBy)
                .ThenByDescending(a => a.ModifyTime)
                .ThenByDescending(a => a.CreatedTime)
                .Take(12)
                .ToListAsync();

            var dbTv = await _videoMainRepository.GetAsNoTracking()
                .Where(a => a.VideoMainStatus == VideoMainStatus.Normal &&
                            a.Subtype == Subtype.Tv)
                .OrderByDescending(a => a.OrderBy)
                .ThenByDescending(a => a.ModifyTime)
                .ThenByDescending(a => a.CreatedTime)
                .Take(12)
                .ToListAsync();

            var dbSeries = await _videoSeriesRepository.GetAsNoTracking()
                .OrderByDescending(a => a.OrderBy)
                .ThenByDescending(a => a.ModifyTime)
                .ThenByDescending(a => a.CreatedTime)
                .Take(12)
                .ToListAsync();

            var result = new List<HomeDataItem>()
            {
                new()
                {
                    TypeName = Subtype.Movie.GetDisplayName(),
                    TypeValue = Subtype.Movie.ToString().ToLower(),
                    TypeDataItems = dbMovie.Select(item=>new HomeTypeDataItem()
                    {
                        KeyWord = item.KeyWord,
                        VideoYear = item.VideoYear,
                        CreatedTime = item.CreatedTime,
                        ModifyTime = item.ModifyTime,
                        VideoDouBan = item.VideoDouBan,
                        VideoImg = item.VideoImg,
                        DetailUrl = $"/vod-detail/{item.Id}",
                        Id = item.Id
                    })
                },
                new()
                {
                    TypeName = Subtype.Tv.GetDisplayName(),
                    TypeValue = Subtype.Tv.ToString().ToLower(),
                    TypeDataItems = dbTv.Select(item=>new HomeTypeDataItem()
                    {
                        KeyWord = item.KeyWord,
                        VideoYear = item.VideoYear,
                        CreatedTime = item.CreatedTime,
                        ModifyTime = item.ModifyTime,
                        VideoDouBan = item.VideoDouBan,
                        VideoImg = item.VideoImg,
                        DetailUrl = $"/vod-detail/{item.Id}",
                        Id = item.Id
                    })
                },
                new()
                {
                    TypeName ="系列合集",
                    TypeValue = "/vod-series",
                    IsUrl = true,
                    TypeDataItems = dbSeries.Select(item=>new HomeTypeDataItem()
                    {
                        KeyWord = item.SeriesName,
                        CreatedTime = item.CreatedTime,
                        ModifyTime = item.ModifyTime,
                        VideoImg = item.SeriesImg,
                        DetailUrl = $"/vod-series/{item.Id}",
                        Id = item.Id
                    })
                },
            };

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 创建影片管理记录
        /// </summary>
        /// <returns></returns>
        private async Task CreateVodManagerRecordAsync(VideoMain videoMain, DouBanInfo douBanInfo)
        {
            if (LoginUserInfo.IsLogin == false)
            {
                //Job进来时，不需要
                return;
            }

            await Task.CompletedTask;
            var recordType = VodManagerRecordType.UpdateMainInfo;
            if (douBanInfo.CreatedUserId == LoginUserInfo.GetUserId())
            {
                recordType = VodManagerRecordType.UpdateMainInfoSelf;
            }

            var jobInput = new CreateVodManagerRecordInput(LoginUserInfo.GetUserId(), recordType)
            {
                BusinessId = videoMain.Id,
                Remark = $"影片名：{videoMain.KeyWord} 豆瓣信息编码:{douBanInfo.Id}",
                LoginUserName = LoginUserInfo.UserName
            };
            BackgroundJob.Enqueue<CreateVodManagerRecordJobService>(a => a.ExecuteAsync(jobInput));
        }

        /// <summary>
        /// 获取EpInfo
        /// </summary>
        /// <returns></returns>
        private VideoEpisode GetVideoEpisodeByString(string url)
        {
            if (url.Contains("$") == false)
            {
                return new VideoEpisode("极速", url);
            }

            var tempArray = url.Split('$').ToArray();
            return new VideoEpisode(tempArray.First().GetNumber() + "", tempArray.Last());
        }

        /// <summary>
        /// 创建系列影片
        /// </summary>
        /// <param name="keyId">影片Id</param>
        /// <param name="seriesId">系列Id</param>
        /// <returns></returns>
        private async Task CreateSeriesListAsync(long keyId, long seriesId)
        {
            //系列
            var series = await _videoSeriesRepository.GetAsNoTracking()
                .Where(a => a.Id == seriesId)
                .FirstOrDefaultAsync();
            if (series == null)
            {
                return;
            }

            //查重
            var dbAny = await _videoSeriesListRepository.GetAsNoTracking()
                .AnyAsync(a => keyId == a.KeyId &&
                               a.SeriesId == seriesId);
            if (dbAny)
            {
                return;
            }

            await _videoSeriesListRepository.CreateAsync(new VideoSeriesList()
            {
                SeriesId = series.Id,
                KeyId = keyId
            });
        }
    }
}
