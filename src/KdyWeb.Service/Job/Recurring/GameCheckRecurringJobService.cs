using System;
using Hangfire;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using KdyWeb.BaseInterface;
using KdyWeb.Dto.HttpApi.GameCheck;
using KdyWeb.Dto.Job;
using KdyWeb.IService.HttpApi;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;

namespace KdyWeb.Service.Job.Recurring
{
    /// <summary>
    /// 游戏检查环循job
    /// </summary>
    public class GameCheckRecurringJobService : IRecurringJob
    {
        private readonly IGameCheckWithGenShinHttpApi _gameCheckWithGenShinHttpApi;
        private readonly GameCheckConfig _gameCheckConfig;

        public GameCheckRecurringJobService(IGameCheckWithGenShinHttpApi gameCheckWithGenShinHttpApi,
            IOptions<GameCheckConfig> options)
        {
            _gameCheckWithGenShinHttpApi = gameCheckWithGenShinHttpApi;
            _gameCheckConfig = options.Value;
        }

        public void Execute(PerformContext context)
        {
            if (_gameCheckConfig.Uid.Any() == false)
            {
                return;
            }

            for (var i = 0; i < _gameCheckConfig.Uid.Count; i++)
            {
                string currentUid = _gameCheckConfig.Uid[i],
                    currentCookie = _gameCheckConfig.Cookie[i];

                var result = KdyAsyncHelper.Run(async () => await _gameCheckWithGenShinHttpApi.QueryDailyNote(currentUid, currentCookie));
                if (result.IsSuccess == false)
                {
                    return;
                }

                var ts = TimeSpan.FromSeconds(result.Data.ResinRecoveryTime);
                var recoveryTime = DateTime.Now.AddSeconds(result.Data.ResinRecoveryTime);
                var tipInput = new SendMsgWithFtqqJobInput()
                {
                    Title = $"{currentUid} 剩余通知",
                    Content = $"树脂将于{recoveryTime:yyyy-MM-dd HH:mm:ss}完全恢复\r\n剩余周本：{result.Data.DiscountNum}"
                };
                BackgroundJob.Enqueue<SendMsgWithFtqqJobService>(a => a.ExecuteAsync(tipInput));

                var finishInput = new SendMsgWithFtqqJobInput()
                {
                    Title = $"{currentUid}  用户提醒",
                    Content = "树脂已恢复，赶紧上班了"
                };
                BackgroundJob.Schedule<SendMsgWithFtqqJobService>(a => a.ExecuteAsync(finishInput), ts);
            }

        }
    }
}
