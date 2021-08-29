using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 影片采集 Job
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    [AutomaticRetry(Attempts = 3)]
    public class VideoCaptureJobService : BaseKdyJob<VideoCaptureJobInput>
    {
        private readonly IVideoCaptureService _videoCaptureService;
        public VideoCaptureJobService(IVideoCaptureService videoCaptureService)
        {
            _videoCaptureService = videoCaptureService;
        }

        public override async Task ExecuteAsync(VideoCaptureJobInput input)
        {
            KdyResult result;
            switch (input.ServiceFullName)
            {
                case VideoCaptureJobInput.ServiceFullNameConst.DownServiceFullName:
                    {
                        result = await _videoCaptureService.CreateVideoDownByDetailAsync(new CreateVideoInfoByDetailInput()
                        {
                            DetailUrl = input.DetailUrl,
                            VideoName = input.VideoName
                        });
                        break;
                    }
                default:
                    {
                        result = await _videoCaptureService.CreateVideoInfoByDetailAsync(new CreateVideoInfoByDetailInput()
                        {
                            DetailUrl = input.DetailUrl,
                            VideoName = input.VideoName
                        });
                        break;
                    }
            }

            KdyLog.LogInformation("影片采集返回：{result},入参：{input}", result.ToJsonStr(), input.ToJsonStr());
        }
    }
}
