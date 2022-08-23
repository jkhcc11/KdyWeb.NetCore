using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.GameCheck;
using KdyWeb.Dto.HttpApi.GameCheck.GenShin;
using KdyWeb.IService.HttpApi;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace KdyWeb.Service.HttpApi
{
    /// <summary>
    /// Genshin检查 服务实现
    /// </summary>
    public class GameCheckWithGenShinHttpApi : BaseKdyService, IGameCheckWithGenShinHttpApi
    {
        private readonly GameCheckConfig _checkConfig;
        public GameCheckWithGenShinHttpApi(IUnitOfWork unitOfWork, IOptions<GameCheckConfig> options)
            : base(unitOfWork)
        {
            _checkConfig = options.Value;
        }

        public async Task<GenShinResult<DailyNoteResult>> QueryDailyNote(string uid, string cookie, string server = "cn_gf01")
        {
            var request = new RestRequest("/api/dailyNote", Method.GET);
            request.AddQueryParameter("server", server);
            request.AddQueryParameter("role_id", uid);
            request.AddHeader("DS", BuildDs($"server={server}&role_id={uid}"));
            request.AddHeader("Cookie", cookie);

            var restClient = await GetRestClient();
            var response = await restClient.ExecuteAsync<GenShinResult<DailyNoteResult>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            KdyLog.LogError(response.ErrorException, "请求genshin Api异常,Input:{0},Response:{1}", request.Resource, response.Content);
            throw new KdyCustomException($"请求异常:{response.ErrorMessage}");
        }

        /// <summary>
        /// 获取Rest客户端
        /// </summary>
        /// <returns></returns>
        private async Task<RestClient> GetRestClient()
        {
            await Task.CompletedTask;
            var restClient = new RestClient
            {
                BaseUrl = new Uri("https://api-takumi-record.mihoyo.com/game_record/app/genshin"),
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54",
            };
            restClient.AddDefaultHeader("x-requested-with", "com.mihoyo.hyperion");
            restClient.AddDefaultHeader("x-rpc-client_type", "5");
            restClient.AddDefaultHeader("x-rpc-app_version", "2.23.1");
            restClient.AddDefaultHeader("Accept", "application/json,text/plain,*/*");
            restClient.UseNewtonsoftJson();
            return restClient;
        }

        /// <summary>
        /// 生成Ds
        /// </summary>
        /// <param name="queryParams">请求参数  xx=bb&yy=cc</param>
        /// <param name="postJson">post json</param>
        /// <returns></returns>
        private string BuildDs(string queryParams, string postJson = "")
        {
            string GetRandomStr()
            {
                var random = new Random((int)DateTime.Now.Ticks);
                int rand = random.Next(100000, 200000);
                if (rand == 100000)
                {
                    rand = 642367;
                }

                return rand.ToString();
            }

            string time = DateTime.Now.ToSecondTimestamp() + "",
                randomStr = GetRandomStr(),
                queryStr = string.Join("&", queryParams.Split('&').OrderBy(x => x)),
                postStr = postJson;

            var signStr = $"salt={_checkConfig.GenshinSalt}&t={time}&r={randomStr}&b={postStr}&q={queryStr}".Md5Ext(solt: "");
            return $"{time},{randomStr},{signStr}";
        }
    }
}
