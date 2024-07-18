using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 绑定影片豆瓣信息
    /// </summary>
    /// <remarks>
    /// 根据豆瓣Id绑定影片豆瓣信息
    /// </remarks>
    [Queue(HangFireQueue.DouBan)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 20, 30, 40, 50 })]
    public class BindVodDouBanInfoJobService : BaseKdyJob<BindVodDouBanInfoJobInput>
    {
        private readonly IVideoMainService _videoMainService;

        public BindVodDouBanInfoJobService(IVideoMainService videoMainService)
        {
            _videoMainService = videoMainService;
        }

        /// <summary>
        /// 具体执行 异步
        /// </summary>
        public override async Task ExecuteAsync(BindVodDouBanInfoJobInput input)
        {
            var matchResult = await _videoMainService.BindDouBanInfoAsync(new MatchDouBanInfoInput()
            {
                DouBanId = input.DouBanId,
                KeyId = input.MainId
            });

            KdyLog.LogInformation("影片绑定豆瓣信息结果。影片Id:{keyId},豆瓣Id:{douBanId},匹配结果：{msg}"
                , input.MainId, input.DouBanId, matchResult.Msg);
        }
    }
}
