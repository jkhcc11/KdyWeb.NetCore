using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Job;
using KdyWeb.IService.GameDown;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 游戏下载解析
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    [AutomaticRetry(Attempts = 3)]
    public class GameDownCaptureJobService : BaseKdyJob<GameDownCaptureJobInput>
    {
        private readonly IGameDownWithByrutService _gameDownWithByrutService;
        private readonly IConfiguration _configuration;

        public GameDownCaptureJobService(IGameDownWithByrutService gameDownWithByrutService,
            IConfiguration configuration)
        {
            _gameDownWithByrutService = gameDownWithByrutService;
            _configuration = configuration;
        }

        /// <summary>
        /// 具体执行 异步
        /// </summary>
        public override async Task ExecuteAsync(GameDownCaptureJobInput input)
        {
            var ua = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.GameDownUaWithByrut);
            var cookie = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.GameDownCookieWithByrut);

            await _gameDownWithByrutService.CreateDownInfoByDetailUrlAsync(input.DetailUrl, ua, cookie);
        }
    }
}
