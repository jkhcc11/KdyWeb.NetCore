﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Kdy.StandardJob.JobInput;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Service.Job;
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
        private readonly IKdyRepository<VideoEpisode, long> _videoEpisodeRepository;
        private readonly IKdyRepository<VideoEpisodeGroup, long> _videoEpisodeGroupRepository;
        private readonly IKdyRepository<UserSubscribe, long> _userSubscribeRepository;
        private readonly IKdyRepository<VideoMainInfo, long> _videoMainInfoRepository;
        private readonly IKdyRepository<UserHistory, long> _userHistoryRepository;
        private readonly IKdyRepository<VideoSeriesList, long> _videoSeriesListRepository;

        public VideoMainService(IKdyRepository<VideoMain, long> videoMainRepository, IKdyRepository<DouBanInfo> douBanInfoRepository,
            IUnitOfWork unitOfWork, IKdyRepository<VideoEpisode, long> videoEpisodeRepository,
            IKdyRepository<VideoEpisodeGroup, long> videoEpisodeGroupRepository, IKdyRepository<UserSubscribe, long> userSubscribeRepository,
            IKdyRepository<VideoMainInfo, long> videoMainInfoRepository, IKdyRepository<UserHistory, long> userHistoryRepository, IKdyRepository<VideoSeriesList, long> videoSeriesListRepository) :
            base(unitOfWork)
        {
            _videoMainRepository = videoMainRepository;
            _douBanInfoRepository = douBanInfoRepository;
            _videoEpisodeRepository = videoEpisodeRepository;
            _videoEpisodeGroupRepository = videoEpisodeGroupRepository;
            _userSubscribeRepository = userSubscribeRepository;
            _videoMainInfoRepository = videoMainInfoRepository;
            _userHistoryRepository = userHistoryRepository;
            _videoSeriesListRepository = videoSeriesListRepository;

            CanUpdateFieldList.AddRange(new[]
            {
                "VideoContentFeature","Subtype","IsEnd","VideoMainStatus","IsMatchInfo","SourceUrl"
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
            result.EpisodeGroup = result.EpisodeGroup.OrderByExt();
            VideoDetailHandler(result);

            if (LoginUserInfo.UserId.HasValue)
            {
                //登录用户就获取最新历史记录
                var dbNewHistory = await _userHistoryRepository.GetAsNoTracking()
                    .Where(a => a.KeyId == keyId && a.CreatedUserId == LoginUserInfo.UserId)
                    .OrderByDescending(a => a.ModifyTime)
                    .ThenByDescending(a => a.CreatedTime)
                    .FirstOrDefaultAsync();
                result.NewUserHistory = dbNewHistory?.MapToExt<QueryUserHistoryDto>();

                //是否订阅
                result.IsSubscribe = await _userSubscribeRepository.GetAsNoTracking()
                    .AnyAsync(a => a.BusinessId == main.Id &&
                                   a.UserSubscribeType == UserSubscribeType.Vod &&
                                   a.CreatedUserId == LoginUserInfo.UserId);
            }

            //影片所属系列
            var dbVideoSeries = await _videoSeriesListRepository.GetAsNoTracking()
                .Include(a => a.VideoSeries)
                .Where(a => a.KeyId == keyId)
                .Select(a => a.VideoSeries)
                .FirstOrDefaultAsync();
            result.VideoSeries = dbVideoSeries?.MapToExt<QueryVideoSeriesDto>();

            if (result.IsEnd)
            {
                //已完结 不用更新
                return KdyResult.Success(result);
            }

            var jobInput = new UpdateNotEndVideoMainJobInput(main.Id, main.SourceUrl, main.VideoContentFeature)
            {
                KeyWord = main.KeyWord
            };
            BackgroundJob.Enqueue<UpdateNotEndVideoJobService>(a => a.ExecuteAsync(jobInput));
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
                VideoDetailHandler(item);
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

            var dbDouBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == input.DouBanId);
            if (dbDouBanInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "豆瓣信息不存在");
            }

            dbMain.ToVideoMain(dbDouBanInfo);
            dbMain.VideoImg = dbDouBanInfo.VideoImg;
            dbMain.IsMatchInfo = true;
            _videoMainRepository.Update(dbMain);

            dbDouBanInfo.DouBanInfoStatus = DouBanInfoStatus.SearchEnd;
            _douBanInfoRepository.Update(dbDouBanInfo);

            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 更新影片主表信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyVideoMainAsync(ModifyVideoMainInput input)
        {
            var main = await _videoMainRepository.GetAsNoTracking()
                .Include(a => a.VideoMainInfo)
                .Include(a => a.EpisodeGroup)
                .Where(a => a.Id == input.Id)
                .FirstOrDefaultAsync();
            if (main == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "keyId错误");
            }

            input.MapToPartExt(main);

            var downEpGroup = main.EpisodeGroup
                // .Select(a => a.Id)
                .FirstOrDefault(a => a.EpisodeGroupType == EpisodeGroupType.VideoDown);
            if (string.IsNullOrEmpty(input.DownUrl) == false)
            {
                #region 下载处理
                //格式
                //名称$下载地址
                //名称2$下载地址2
                //名称3$下载地址3
                var downEp = new List<VideoEpisode>();
                var tempDownArray = input.DownUrl.Split(new[] { '\r', '\n', '#' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var tempItem in tempDownArray)
                {
                    if (tempItem.Contains("$") == false)
                    {
                        continue;
                    }

                    var nameArray = tempItem.Split('$');

                    //剧集
                    var tempEpisode = new VideoEpisode(nameArray[0], nameArray[1]);
                    if (downEpGroup != null)
                    {
                        tempEpisode.SetEpisodeGroupId(downEpGroup.Id);
                    }

                    downEp.Add(tempEpisode);
                }

                if (downEpGroup != null)
                {
                    //删除旧的剧集 新增新的
                    await _videoEpisodeRepository.Delete(a => a.EpisodeGroupId == downEpGroup.Id);
                    await _videoEpisodeRepository.CreateAsync(downEp);
                }
                else
                {
                    //创建新的组
                    var downGroup = new VideoEpisodeGroup(EpisodeGroupType.VideoDown, "默认下载")
                    {
                        Episodes = downEp,
                        MainId = input.Id
                    };
                    await _videoEpisodeGroupRepository.CreateAsync(downGroup);
                }

                #endregion
            }

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

            if (main.IsEnd)
            {
                return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.Error, "影片已完结，无需同步");
            }

            var db = KdyRedisCache.GetDb(1);
            await db.KeyDeleteAsync($"{KdyServiceCacheKey.NotEndKey}:{mainId}");

            var jobInput = new UpdateNotEndVideoMainJobInput(main.Id, main.SourceUrl, main.VideoContentFeature)
            {
                KeyWord = main.KeyWord
            };
            var jobId = BackgroundJob.Enqueue<UpdateNotEndVideoJobService>(a => a.ExecuteAsync(jobInput));

            return KdyResult.Success($"任务Id:{jobId} 已添加");

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
                    result.Add(item.MapToExt<QuerySameVideoByActorDto>());
                    continue;
                }

                if (item.VideoDirectors.IsEmptyExt() == false &&
                    item.VideoDirectors.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).Any(a => a == input.Actor))
                {
                    result.Add(item.MapToExt<QuerySameVideoByActorDto>());
                }
            }

            return KdyResult.Success(result);
        }

        #region 私有
        /// <summary>
        /// 详情处理
        /// </summary>
        private void VideoDetailHandler(GetVideoDetailDto detail)
        {
            //var douBanProxy = KdyConfiguration.GetValue<string>(KdyWebServiceConst.DouBanProxyUrl);
            //if (string.IsNullOrEmpty(douBanProxy) ||
            //    string.IsNullOrEmpty(detail.VideoImg) ||
            //    detail.VideoImg.Contains("view/movie_poster_cover") == false)
            //{
            //    return;
            //}

            //https://img9.doubanio.com        /view/photo/s_ratio_poster/public/p2625825416.jpg
            //替换 https://img9.doubanio.com  /view/movie_poster_cover/lpst/public/p2625825416.jpg
            detail.VideoImg = detail.VideoImg.Replace("/view/photo/s_ratio_poster", "/view/movie_poster_cover/lpst");
        }

        /// <summary>
        /// 详情处理
        /// </summary>
        private void VideoDetailHandler(QueryVideoMainDto detail)
        {
            //https://img9.doubanio.com        /view/photo/s_ratio_poster/public/p2625825416.jpg
            //替换 https://img9.doubanio.com  /view/movie_poster_cover/lpst/public/p2625825416.jpg
            detail.VideoImg = detail.VideoImg.Replace("/view/photo/s_ratio_poster", "/view/movie_poster_cover/lpst");
        }
        #endregion
    }
}
