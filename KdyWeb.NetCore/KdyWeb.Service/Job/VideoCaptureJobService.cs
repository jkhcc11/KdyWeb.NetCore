using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;

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
        public VideoCaptureJobService(IKdyLog kdyLog, IVideoCaptureService videoCaptureService) : base(kdyLog)
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

            KdyLog.Trace($"影片采集 {result.IsSuccess},详情：{input.DetailUrl}", new Dictionary<string, object>()
            {
                {"Input",input},
                {"Result",result}
            });
        }
    }
}
