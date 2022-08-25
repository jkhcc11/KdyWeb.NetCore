using Hangfire;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Dto.HttpApi;
using KdyWeb.Dto.HttpApi.GameCheck;
using KdyWeb.Dto.Job;
using KdyWeb.IService.HttpApi;
using KdyWeb.Utility;
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
        private readonly IKdyRedisCache _redisCache;

        public GameCheckRecurringJobService(IGameCheckWithGenShinHttpApi gameCheckWithGenShinHttpApi,
            IOptions<GameCheckConfig> options, IKdyRedisCache redisCache)
        {
            _gameCheckWithGenShinHttpApi = gameCheckWithGenShinHttpApi;
            _redisCache = redisCache;
            _gameCheckConfig = options.Value;
        }

        public void Execute(PerformContext context)
        {
            var db = _redisCache.GetDb(2);
            var cookieList = KdyAsyncHelper.Run(async () => await db.HashGetAllAsync(KdyServiceCacheKey.GameCheckKey));
            if (cookieList.Any() == false)
            {
                return;
            }

            foreach (var cookieItem in cookieList)
            {
                string currentUid = cookieItem.Name,
                    currentCookie = cookieItem.Value;

                //签到
                var input = new BBsSignRewardInput(currentUid, _gameCheckConfig.BbsSalt, _gameCheckConfig.BbsVersion,
                    currentCookie);
                var signResult = KdyAsyncHelper.Run(async () => await _gameCheckWithGenShinHttpApi.BBsSignRewardAsync(input));

                //查询
                var querySignInput = new QuerySignInfoInput(currentUid, _gameCheckConfig.BbsSalt,
                    _gameCheckConfig.BbsVersion,
                    currentCookie);
                var querySignResult = KdyAsyncHelper.Run(async () => await _gameCheckWithGenShinHttpApi.QuerySignInfoAsync(querySignInput));

                var tipInput = new SendMsgWithFtqqJobInput()
                {
                    Title = $"{currentUid} 签到通知",
                    Content = $"签到结果：{(signResult.IsSuccess ? $"成功:{signResult.Data.IsSuccess}" : $"失败,{signResult.Message}")} \r\n " +
                              $"累计签到天数：{querySignResult.Data.TotalSignDay}"
                };
                BackgroundJob.Enqueue<SendMsgWithFtqqJobService>(a => a.ExecuteAsync(tipInput));
            }

            //for (var i = 0; i < _gameCheckConfig.Uid.Count; i++)
            //{
            //    string currentUid = _gameCheckConfig.Uid[i],
            //        currentCookie = "_gameCheckConfig.Cookie[i]";
            //    var queryDailyInput = new QueryDailyNoteInput(currentUid, _gameCheckConfig.GenshinSalt,
            //        "2.23.1", currentCookie);

            //    var result = KdyAsyncHelper.Run(async () => await _gameCheckWithGenShinHttpApi.QueryDailyNoteAsync(queryDailyInput));
            //    if (result.IsSuccess == false)
            //    {
            //        return;
            //    }

            //    var ts = TimeSpan.FromSeconds(result.Data.ResinRecoveryTime);
            //    var recoveryTime = DateTime.Now.AddSeconds(result.Data.ResinRecoveryTime);
            //    var tipInput = new SendMsgWithFtqqJobInput()
            //    {
            //        Title = $"{currentUid} 剩余通知",
            //        Content = $"树脂将于{recoveryTime:yyyy-MM-dd HH:mm:ss}完全恢复\r\n剩余周本：{result.Data.DiscountNum}"
            //    };
            //    BackgroundJob.Enqueue<SendMsgWithFtqqJobService>(a => a.ExecuteAsync(tipInput));

            //    var finishInput = new SendMsgWithFtqqJobInput()
            //    {
            //        Title = $"{currentUid}  用户提醒",
            //        Content = "树脂已恢复，赶紧上班了"
            //    };
            //    BackgroundJob.Schedule<SendMsgWithFtqqJobService>(a => a.ExecuteAsync(finishInput), ts);
            //}
        }
    }
}
