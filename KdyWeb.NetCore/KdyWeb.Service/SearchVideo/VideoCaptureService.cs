using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Service.Job;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 影片采集 服务实现
    /// </summary>
    public class VideoCaptureService : BaseKdyService, IVideoCaptureService
    {
        private readonly IKdyRepository<VideoMain, long> _videoMainRepository;
        private readonly IPageSearchConfigService _pageSearchConfigService;
        private readonly IDouBanInfoService _douBanInfoService;

        private readonly IKdyRepository<PageSearchConfig, long> _pageSearchRepository;
        private readonly IKdyRepository<DouBanInfo> _douBanInfoRepository;
        public VideoCaptureService(IUnitOfWork unitOfWork, IKdyRepository<VideoMain, long> videoMainRepository,
            IPageSearchConfigService pageSearchConfigService, IDouBanInfoService douBanInfoService,
            IKdyRepository<PageSearchConfig, long> pageSearchRepository, IKdyRepository<DouBanInfo> douBanInfoRepository) : base(unitOfWork)
        {
            _videoMainRepository = videoMainRepository;
            _pageSearchConfigService = pageSearchConfigService;
            _douBanInfoService = douBanInfoService;
            _pageSearchRepository = pageSearchRepository;
            _douBanInfoRepository = douBanInfoRepository;
        }

        /// <summary>
        /// 根据影片源详情创建影片 
        /// </summary>
        /// <remarks>
        ///  1、根据源详情获取影片信息 <br/>
        ///  2、根据名称如果存在则跳过
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> CreateVideoInfoByDetailAsync(CreateVideoInfoByDetailInput input)
        {
            var host = new Uri(input.DetailUrl).Host;

            //获取实例
            var pageService = await _pageSearchConfigService.GetPageParseInstanceAsync(new GetPageParseInstanceInput()
            {
                BaseHost = host
            });
            if (pageService.IsSuccess == false)
            {
                return KdyResult.Error(pageService.Code, $"获取站点配置失败,{pageService.Msg}");
            }

            //检查详情
            var any = await _videoMainRepository.GetAsNoTracking()
                .AnyAsync(a => a.SourceUrl == input.DetailUrl);
            if (any)
            {
                return KdyResult.Error(KdyResultCode.Error, $"影片详情已存在,影片采集失败。{input.DetailUrl}");
            }

            #region 获取最新结果
            var pageResult = await pageService.Data.Instance.GetResultAsync(new NormalPageParseInput()
            {
                ConfigId = pageService.Data.ConfigId,
                Detail = input.DetailUrl
            });
            if (pageResult.IsSuccess == false)
            {
                KdyLog.Warn($"影片采集失败，{pageResult.Msg}", new Dictionary<string, object>()
                {
                    {"JobInput",input},
                    {"PageResult",pageResult}
                }, input.DetailUrl);

                return KdyResult.Error(pageResult.Code, $"获取详情失败,{pageResult.Msg}");
            }
            #endregion

            var name = input.VideoName;
            if (string.IsNullOrEmpty(name))
            {
                name = pageResult.Data.ResultName;
            }

            //检查名称
            any = await _videoMainRepository.GetAsNoTracking()
               .AnyAsync(a => a.KeyWord == name);
            if (any)
            {
                return KdyResult.Error(KdyResultCode.Error, $"影片名称已存在,影片采集失败。{name}");
            }

            //豆瓣信息
            var douBanInfo = await _douBanInfoService.CreateForKeyWordAsync(name, pageResult.Data.VideoYear);
            if (douBanInfo.IsSuccess == false)
            {
                return KdyResult.Error(douBanInfo.Code, $"获取豆瓣信息失败，{douBanInfo.Msg}");
            }

            //更新豆瓣状态
            var dbDouBan = await _douBanInfoRepository
                .GetWriteQuery()
                .FirstOrDefaultAsync(a => a.Id == douBanInfo.Data.Id);
            dbDouBan.DouBanInfoStatus = DouBanInfoStatus.SearchEnd;
            _douBanInfoRepository.Update(dbDouBan);

            //生成影片信息
            var dbVideoMain = new VideoMain(douBanInfo.Data.Subtype, name, douBanInfo.Data.VideoImg, input.DetailUrl, pageResult.Data.PageMd5);
            dbVideoMain.ToVideoMain(douBanInfo.Data);

            var epList = pageResult.Data.Results
                .Select(a => new VideoEpisode(a.ResultName, a.ResultUrl))
                .ToList();

            dbVideoMain.EpisodeGroup = new List<VideoEpisodeGroup>()
            {
                new VideoEpisodeGroup(EpisodeGroupType.VideoPlay,"默认组")
                {
                    Episodes = epList
                }
            };
            dbVideoMain.IsMatchInfo = false;
            dbVideoMain.IsEnd = pageResult.Data.IsEnd;
            await _videoMainRepository.CreateAsync(dbVideoMain);

            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 创建定时影片录入Job
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateRecurringVideoJobAsync()
        {
            var normalList = await _pageSearchRepository.GetAsNoTracking()
                .Where(a => a.SearchConfigStatus == SearchConfigStatus.Normal)
                .ToListAsync();
            var jobCron = KdyConfiguration.GetValue<string>(KdyWebServiceConst.JobCron.RecurringVideoJob);

            foreach (var item in normalList)
            {
                if (item.CaptureDetailUrl == null ||
                    item.CaptureDetailUrl.Any() == false)
                {
                    continue;
                }

                foreach (var detailItem in item.CaptureDetailUrl)
                {
                    var recurringJobInput = new RecurringVideoJobInput()
                    {
                        BaseHost = item.BaseHost,
                        CaptureDetailNameSplit = item.CaptureDetailNameSplit,
                        CaptureDetailXpath = item.CaptureDetailXpath,
                        OriginUrl = $"{item.BaseHost}{detailItem}"
                    };

                    var jobId = $"Capture.RecurringJob.{item.BaseHost.Replace("http://", "").Replace("https://", "")}";

                    RecurringJob.AddOrUpdate<RecurringVideoJobService>(jobId, a => a.ExecuteAsync(recurringJobInput), jobCron);
                }
            }

            return KdyResult.Success();
        }
    }
}
