using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Job;
using KdyWeb.IService.GameDown;
using KdyWeb.IService.HttpApi;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 获取Steam信息Job
    /// </summary>
    [Queue(HangFireQueue.Capture)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 20, 30, 40, 50 })]
    public class SteamInfoJobService : BaseKdyJob<SteamInfoJobInput>
    {
        private readonly IGameDownService _gameDownService;
        private readonly IGameDownWithByrutService _gameDownWithByrutService;
        private readonly ISteamWebHttpApi _steamWebHttpApi;
        private readonly IConfiguration _configuration;

        public SteamInfoJobService(IGameDownWithByrutService gameDownWithByrutService,
            IConfiguration configuration, IGameDownService gameDownService, ISteamWebHttpApi steamWebHttpApi)
        {
            _gameDownWithByrutService = gameDownWithByrutService;
            _configuration = configuration;
            _gameDownService = gameDownService;
            _steamWebHttpApi = steamWebHttpApi;
        }

        /// <summary>
        /// 具体执行 异步
        /// </summary>
        public override async Task ExecuteAsync(SteamInfoJobInput input)
        {
            var steamResult = input.SteamUrl;
            if (input.IsSteamUrl == false)
            {
                #region 非真实steam地址，获取
                var ua = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.GameDownUaWithByrut);
                var cookie = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.GameDownCookieWithByrut);

                //获取SteamUrl
                steamResult = await _gameDownWithByrutService.GetSteamStoreUrlByIdAndUserHashAsync(ua, cookie, input.CustomId, input.UserHash);
                if (steamResult.IsEmptyExt())
                {
                    return;
                } 
                #endregion
            }

            //根据SteamUrl获取信息
            var steamResponse = await _steamWebHttpApi.GetGameInfoByStoreUrlAsync(steamResult);
            if (steamResponse.IsSuccess)
            {
                await _gameDownService.SaveSteamInfoByDownIdAsync(input.DownId, steamResponse.Data);
            }
            else if (steamResponse.Code == KdyResultCode.HttpError)
            {
                throw new KdyCustomException(steamResponse.Msg);
            }
        }
    }
}
