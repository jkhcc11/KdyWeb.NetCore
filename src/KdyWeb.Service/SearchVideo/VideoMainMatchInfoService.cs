using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.HttpApi;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Service.HttpCapture;
using KdyWeb.Service.Job;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 影片主表匹配信息相关 服务接口
    /// </summary>
    public class VideoMainMatchInfoService : BaseKdyService, IVideoMainMatchInfoService
    {
        private readonly IDouBanHttpApi _douBanHttpApi;
        private readonly IDouBanInfoService _douBanInfoService;
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;
        private readonly IKdyRepository<DouBanInfo, int> _douBanInfoRepository;

        private readonly IPageSearchConfigService _pageSearchConfigService;
        private readonly IVideoEpisodeService _videoEpisodeService;

        public VideoMainMatchInfoService(IUnitOfWork unitOfWork, IDouBanHttpApi douBanHttpApi,
            IDouBanInfoService douBanInfoService, IKdyRepository<VideoMain, long> videoMainRepository,
            IPageSearchConfigService pageSearchConfigService, IVideoEpisodeService videoEpisodeService,
            IKdyRepository<DouBanInfo, int> douBanInfoRepository)
            : base(unitOfWork)
        {
            _douBanHttpApi = douBanHttpApi;
            _douBanInfoService = douBanInfoService;
            _videoMainRepository = videoMainRepository;
            _pageSearchConfigService = pageSearchConfigService;
            _videoEpisodeService = videoEpisodeService;
            _douBanInfoRepository = douBanInfoRepository;
        }

        /// <summary>
        /// 匹配豆瓣信息
        /// </summary>
        /// <remarks>
        ///  仅根据关键字匹配豆瓣信息，如果匹配成功(关键字和年份一致)，则保存豆瓣信息
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> MatchDouBanInfoAsync(long mainId)
        {
            var matchCacheKey = $"MatchDouBanInfo:{mainId}";
            var isRun = await KdyRedisCache.GetCache().GetStringAsync(matchCacheKey);
            if (string.IsNullOrEmpty(isRun) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "Run");
            }

            var main = await _videoMainRepository.GetAsNoTracking()
                .Where(a => a.Id == mainId)
                .FirstOrDefaultAsync();
            if (main == null)
            {
                return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.Error, "keyId错误");
            }

            if (main.IsMatchInfo)
            {
                return KdyResult.Error(KdyResultCode.Error, "not need match");
            }

            await KdyRedisCache.GetCache().SetStringAsync(matchCacheKey, DateTime.Now.ToShortTimeString(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });

            //搜索豆瓣
            var searchResult = await _douBanHttpApi.SearchSuggestAsync(main.KeyWord);
            if (searchResult.IsSuccess == false)
            {
                searchResult = await _douBanHttpApi.KeyWordSearchAsync(main.KeyWord, 1);
            }

            if (searchResult.IsSuccess == false)
            {
                KdyLog.LogWarning("影片关键字：{0}，匹配豆瓣失败，无法检索到豆瓣信息。", main.KeyWord);
                return KdyResult.Error(KdyResultCode.Error, "match warning");
            }

            //开始匹配
            bool isMatchSuccess = false;
            foreach (var searchItem in searchResult.Data)
            {
                var douBanTitle = searchItem.Title;
                var vodTitle = main.KeyWord;
                if (douBanTitle.RemoveSpecialCharacters() != vodTitle.RemoveSpecialCharacters() ||
                    searchItem.Year != main.VideoYear)
                {
                    continue;
                }

                //存豆瓣信息
                var douBanInfo = await _douBanInfoService.CreateForSubjectIdAsync(searchItem.SubjectId);
                if (douBanInfo.IsSuccess == false)
                {
                    KdyLog.LogWarning("豆瓣主题Id:{subject}，获取豆瓣信息异常，异常信息：{errorMsg}"
                        , searchItem.SubjectId
                        , douBanInfo.Msg);

                    if (douBanInfo.Code == KdyResultCode.HttpError)
                    {
                        return KdyResult.Error(KdyResultCode.Error, "get douban error");
                    }
                }
                else
                {
                    isMatchSuccess = true;

                    //豆瓣信息保存成功，开始绑定影片
                    var bindInput = new BindVodDouBanInfoJobInput()
                    {
                        DouBanId = douBanInfo.Data.Id,
                        MainId = main.Id
                    };

                    BackgroundJob.Enqueue<BindVodDouBanInfoJobService>(a => a.ExecuteAsync(bindInput));
                }

                break;
            }

            if (isMatchSuccess == false)
            {
                KdyLog.LogError("影片关键字：{0}，匹配豆瓣失败，无法匹配到。影片Id:{1},豆瓣数量：{2}"
                    , main.KeyWord
                    , main.Id
                    , searchResult.Data.Count);
            }

            await KdyRedisCache.GetCache().RemoveAsync(matchCacheKey);
            return KdyResult.Success();
        }

        /// <summary>
        /// 匹配资源信息
        /// </summary>
        /// <remarks>
        ///  1、仅根据关键字匹配资源站，如果匹配成功(关键字和年份一致)，则保存资源
        ///  2、不是人工匹配的且是之前的资源站，就重新匹配资源
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> MatchZyAsync(long mainId)
        {
            var matchCacheKey = $"MatchZyInfo:{mainId}";
            var isRun = await KdyRedisCache.GetCache().GetStringAsync(matchCacheKey);
            if (string.IsNullOrEmpty(isRun) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "Run");
            }

            var main = await _videoMainRepository.GetAsNoTracking()
                .Where(a => a.Id == mainId)
                .FirstOrDefaultAsync();
            if (main == null)
            {
                return KdyResult.Error<GetVideoDetailDto>(KdyResultCode.Error, "keyId错误");
            }

            if (main.IsMatchZy() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "not need match");
            }

            //开始匹配资源
            var zyEngine = await _pageSearchConfigService.QueryShowPageConfigAsync(new SearchConfigInput()
            {
                KeyWord = nameof(ZyPageParseForJsonService)
            });
            if (zyEngine.IsSuccess == false ||
                zyEngine.Data.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "no config engine");
            }

            await KdyRedisCache.GetCache().SetStringAsync(matchCacheKey, DateTime.Now.ToShortTimeString(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });

            #region 搜索资源站并匹配
            var parseInput = new GetPageParseInstanceInput()
            {
                ConfigId = zyEngine.Data.First().Id
            };

            var pageResult = await _pageSearchConfigService.GetPageParseInstanceAsync(parseInput);
            if (pageResult.IsSuccess == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "get engine error");
            }

            var result = await pageResult.Data.Instance.GetResultAsync(new NormalPageParseInput()
            {
                ConfigId = zyEngine.Data.First().Id,
                KeyWord = main.KeyWord
            });

            if (result.IsSuccess == false)
            {
                KdyLog.LogWarning("影片关键字：{0}，KeyId:{1},匹配资源失败，资源站匹配资源失败。",
                    main.KeyWord,
                    main.Id);
                return KdyResult.Error(KdyResultCode.Error, "match error,empty with search");
            }

            if (result.Data.ResultName.RemoveSpecialCharacters() != main.KeyWord.RemoveSpecialCharacters() ||
                result.Data.VideoYear != main.VideoYear)
            {
                KdyLog.LogWarning("影片关键字：{0}，KeyId:{1},匹配资源失败，资源站匹配资源失败。",
                    main.KeyWord,
                    main.Id);
                return KdyResult.Error(KdyResultCode.Error, "match error,not match keyword");
            }
            #endregion

            //更新资源
            var tempEpItems = result.Data.Results
                .Select(a => new EditEpisodeItem()
                {
                    EpisodeName = a.ResultName,
                    EpisodeUrl = a.ResultUrl
                }).ToList();
            if (tempEpItems.Count == 1)
            {
                result.Data.Results.First().ResultName = "备用";
            }

            var updateNotEndInput = new UpdateNotEndVideoInput(main.Id,
                result.Data.PageMd5,
                result.Data.IsEnd,
                tempEpItems)
            {
                SourceUrl = result.Data.DetailUrl
            };
            await _videoEpisodeService.UpdateNotEndVideoAsync(updateNotEndInput);

            await KdyRedisCache.GetCache().RemoveAsync(matchCacheKey);
            return KdyResult.Success();
        }

        /// <summary>
        /// 自动匹配并保存剧集
        /// </summary>
        /// 1、豆瓣信息事先匹配好了，直接丢进来保存剧集
        /// 2、根据豆瓣名称+年份匹配影视库
        /// 3、全都匹配成功，更新剧集信息或者保存剧集信息
        /// <returns></returns>
        public async Task<KdyResult<string>> AutoMatchSaveEpAsync(AutoMatchSaveEpInput input)
        {
            if (input.EpItems == null || input.EpItems.Any() == false)
            {
                return KdyResult.Error<string>(KdyResultCode.ParError, "无效剧集");
            }

            var matchCacheKey = $"AutoMatchSaveEp:{input.DouBanInfoId}";
            var isRun = await KdyRedisCache.GetCache().GetStringAsync(matchCacheKey);
            if (string.IsNullOrEmpty(isRun) == false)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "Run");
            }

            var dbDouBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == input.DouBanInfoId);
            if (dbDouBanInfo == null)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "豆瓣信息无效");
            }

            await KdyRedisCache.GetCache().SetStringAsync(matchCacheKey, DateTime.Now.ToShortTimeString(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });

            #region 2、使用豆瓣标题和年份匹配影片
            var subjectId = dbDouBanInfo.VideoDetailId;
            var keyWord = dbDouBanInfo.VideoTitle;
            var videoYear = dbDouBanInfo.VideoYear;
            var vodMains = await _videoMainRepository.GetQuery()
                .Where(a => (a.KeyWord.Contains(keyWord) &&
                          a.VideoYear == videoYear) ||
                            (a.VideoInfoUrl != null &&
                             a.VideoInfoUrl.Contains(subjectId)))
                .ToListAsync();
            if (vodMains.Any() == false)
            {
                await KdyRedisCache.GetCache().RemoveAsync(matchCacheKey);
                return await NotExistVodCreateAsync(dbDouBanInfo, input);
            }

            if (vodMains.Count > 1)
            {
                await KdyRedisCache.GetCache().RemoveAsync(matchCacheKey);
                return KdyResult.Error<string>(KdyResultCode.Error, "匹配影视库失败，存在多个同名");
            }
            #endregion

            #region 3、存在且匹配成功更新剧集
            var main = vodMains.First();
            if (string.IsNullOrEmpty(input.ZyDetailUrl) == false ||
                string.IsNullOrEmpty(input.ZyPageMd5) == false)
            {
                //自助的 只要存在就不用提交了，提示前往反馈
                await KdyRedisCache.GetCache().RemoveAsync(matchCacheKey);
                return KdyResult.Error<string>(KdyResultCode.Error, "已存在影片，请直接前往播放页反馈即可");
            }

            if (main.VideoContentFeature == VideoMain.SystemInput ||
                main.SourceUrl == VideoMain.SystemInput)
            {
                await KdyRedisCache.GetCache().RemoveAsync(matchCacheKey);
                return KdyResult.Error<string>(KdyResultCode.Error, "更新剧集失败，影片可能为人工录入");
            }

            var updateNotEndInput = new UpdateNotEndVideoInput(main.Id,
                VideoMain.SystemInput,
                true,
                input.EpItems)
            {
                SourceUrl = VideoMain.SystemInput
            };
            await _videoEpisodeService.UpdateNotEndVideoAsync(updateNotEndInput);
            #endregion

            await KdyRedisCache.GetCache().RemoveAsync(matchCacheKey);
            return KdyResult.Success<string>(main.Id.ToString());
        }

        /// <summary>
        /// 不存在创建新的影片
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult<string>> NotExistVodCreateAsync(DouBanInfo douBanInfo, AutoMatchSaveEpInput input)
        {
            var sourceUrl = string.IsNullOrEmpty(input.ZyDetailUrl) ?
                VideoMain.SystemInput : input.ZyDetailUrl;
            var videoContentFeature = string.IsNullOrEmpty(input.ZyPageMd5) ?
                VideoMain.SystemInput : input.ZyPageMd5;

            //生成影片信息
            var dbVideoMain = new VideoMain(douBanInfo.Subtype, douBanInfo.VideoTitle, douBanInfo.VideoImg,
                sourceUrl, videoContentFeature);
            dbVideoMain.ToVideoMain(douBanInfo);
            dbVideoMain.EpisodeGroup = new List<VideoEpisodeGroup>()
            {
                new VideoEpisodeGroup(EpisodeGroupType.VideoPlay,"默认组")
                {
                    Episodes = input.EpItems.Select(a=>new VideoEpisode(a.EpisodeName,a.EpisodeUrl))
                        .ToList()
                }
            };

            dbVideoMain.IsMatchInfo = true;
            dbVideoMain.IsEnd = true;
            await _videoMainRepository.CreateAsync(dbVideoMain);

            douBanInfo.SetSearchEnd();
            _douBanInfoRepository.Update(douBanInfo);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success<string>(dbVideoMain.Id.ToString());
        }
    }
}
