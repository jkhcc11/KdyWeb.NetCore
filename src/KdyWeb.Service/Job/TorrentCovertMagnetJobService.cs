using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.GameDown;
using KdyWeb.Dto.Job;
using KdyWeb.IService.GameDown;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 种子转磁力Job
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 20, 30, 40, 50 })]
    public class TorrentCovertMagnetJobService : BaseKdyJob<TorrentCovertMagnetJobInput>
    {
        private readonly IGameDownService _gameDownService;
        private readonly IGameDownWithByrutService _gameDownWithByrutService;
        private readonly IConfiguration _configuration;

        public TorrentCovertMagnetJobService(IGameDownWithByrutService gameDownWithByrutService,
            IConfiguration configuration, IGameDownService gameDownService)
        {
            _gameDownWithByrutService = gameDownWithByrutService;
            _configuration = configuration;
            _gameDownService = gameDownService;
        }

        /// <summary>
        /// 具体执行 异步
        /// </summary>
        public override async Task ExecuteAsync(TorrentCovertMagnetJobInput input)
        {
            var ua = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.GameDownUaWithByrut);
            var cookie = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.GameDownCookieWithByrut);

            var downInput = new ConvertMagnetByByTorrentInput(input.TorrentUrl, cookie, ua)
            {
                Referer = input.Referer,
            };
            var covertResult = await _gameDownWithByrutService.ConvertMagnetByByTorrentUrlAsync(downInput);
            if (covertResult != null)
            {
                await _gameDownService.SaveMagnetByTorrentUrlAsync(input.DownId, covertResult.MagnetLink);
            }
        }
    }
}
