using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Job;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.IService.Job;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// Hangfire Job初始化 服务实现
    /// </summary>
    public class JobInitService : BaseKdyService, IJobInitService
    {
        private readonly IKdyRepository<PageSearchConfig, long> _pageSearchRepository;
        private readonly IKdyRepository<RecurrentUrlConfig, long> _recurrentUrlConfigRepository;
        public JobInitService(IUnitOfWork unitOfWork, IKdyRepository<PageSearchConfig, long> pageSearchRepository,
            IKdyRepository<RecurrentUrlConfig, long> recurrentUrlConfigRepository) :
            base(unitOfWork)
        {
            _pageSearchRepository = pageSearchRepository;
            _recurrentUrlConfigRepository = recurrentUrlConfigRepository;
        }

        /// <summary>
        /// 初始化循环影片录入Job
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> InitRecurringVideoJobAsync()
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
                        OriginUrl = $"{item.BaseHost}{detailItem}",
                        ServiceFullName = item.ServiceFullName
                    };
                    var uri = new Uri(recurringJobInput.OriginUrl);
                    var jobFlag = uri.AbsolutePath;
                    if (string.IsNullOrEmpty(uri.Query) == false)
                    {
                        jobFlag = uri.Query.Md5Ext();
                    }

                    var jobId = $"Capture.RecurringJob.{uri.Host}{jobFlag}";

                    RecurringJob.AddOrUpdate<RecurringVideoJobService>(jobId, a => a.ExecuteAsync(recurringJobInput), jobCron);
                }
            }

            return KdyResult.Success();
        }

        /// <summary>
        /// 初始化循环UrlJob
        /// </summary>
        /// <returns></returns>
        [Obsolete("废弃，以使用RecurrentUrlConfigService更新并创建")]
        public async Task<KdyResult> InitRecurrentUrlJobAsync()
        {
            var recurrentUrlConfig = await _recurrentUrlConfigRepository.GetAsNoTracking()
                .Where(a => a.SearchConfigStatus == SearchConfigStatus.Normal)
                .ToListAsync();

            foreach (var item in recurrentUrlConfig)
            {
                if (string.IsNullOrEmpty(item.RequestUrl) ||
                    string.IsNullOrEmpty(item.UrlCron))
                {
                    continue;
                }

                var jobInput = item.MapToExt<RecurrentUrlJobInput>();
                var uri = new Uri(item.RequestUrl);
                var jobId = $"Capture.RecurringJobUrl.{uri.Host}{uri.AbsolutePath}";
                RecurringJob.AddOrUpdate<RecurringUrlJobService>(jobId, a => a.ExecuteAsync(jobInput), item.UrlCron);
            }

            return KdyResult.Success();
        }
    }
}
