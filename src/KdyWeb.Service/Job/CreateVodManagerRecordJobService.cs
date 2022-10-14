using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Job;
using KdyWeb.IService.VideoConverts;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 创建影片管理者记录 job
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    [AutomaticRetry(Attempts = 1, DelaysInSeconds = new[] { 10 })]
    public class CreateVodManagerRecordJobService : BaseKdyJob<CreateVodManagerRecordInput>
    {
        private readonly IVodManagerRecordService _vodManagerRecordService;

        public CreateVodManagerRecordJobService(IVodManagerRecordService vodManagerRecordService)
        {
            _vodManagerRecordService = vodManagerRecordService;
        }

        /// <summary>
        /// 具体执行 异步
        /// </summary>
        public override async Task ExecuteAsync(CreateVodManagerRecordInput input)
        {
            await _vodManagerRecordService.CreateVodManagerRecordAsync(input.RecordType
                , input.UserId, input.BusinessId, input.CheckoutAmount, input.Remark);
        }
    }
}
