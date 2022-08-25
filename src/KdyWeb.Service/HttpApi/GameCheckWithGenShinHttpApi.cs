using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi;
using KdyWeb.Dto.HttpApi.GameCheck.GenShin;
using KdyWeb.IService.HttpApi;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace KdyWeb.Service.HttpApi
{
    /// <summary>
    /// Genshin检查 服务实现
    /// </summary>
    public class GameCheckWithGenShinHttpApi : BaseKdyService, IGameCheckWithGenShinHttpApi
    {
        /// <summary>
        /// BBS api
        /// </summary>
        private const string BbsApi = "https://api-takumi.mihoyo.com";
        /// <summary>
        /// 便签Api
        /// </summary>
        private const string DailyNoteApi = "https://api-takumi-record.mihoyo.com/game_record/app/genshin";
        public GameCheckWithGenShinHttpApi(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task<GenShinResult<DailyNoteResult>> QueryDailyNoteAsync(QueryDailyNoteInput input)
        {
            var request = new RestRequest("/api/dailyNote", Method.GET);
            request.AddQueryParameter("server", input.Region);
            request.AddQueryParameter("role_id", input.Uid);
            request.AddHeader("DS", BuildDs(input.Salt, $"server={input.Region}&role_id={input.Uid}"));
            request.AddHeader("Cookie", input.Cookie);
            request.AddHeader("x-rpc-app_version", input.Version);
            var restClient = await GetRestClient();
            var response = await restClient.ExecuteAsync<GenShinResult<DailyNoteResult>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            KdyLog.LogError(response.ErrorException, "请求genshin Api异常,Input:{0},Response:{1}", request.Resource, response.Content);
            throw new KdyCustomException($"请求异常:{response.ErrorMessage}");
        }

        public async Task<GenShinResult<SignRewardResult>> BBsSignRewardAsync(BBsSignRewardInput input)
        {
            var request = new RestRequest("/event/bbs_sign_reward/sign", Method.POST);
            var reqBody = new
            {
                act_id = input.ActId,
                region = input.Region,
                uid = input.Uid
            };
            request.AddJsonBody(reqBody);
            request.AddHeader("DS", BuildDsWithBbs(input.Salt));
            request.AddHeader("Cookie", input.Cookie);
            request.AddHeader("Referer", "https://app.mihoyo.com");
            request.AddHeader("x-rpc-app_version", input.Version);
            var restClient = await GetBbsRestClient();
            var response = await restClient.ExecuteAsync<GenShinResult<SignRewardResult>>(request);
            KdyLog.LogDebug($"用户UID:{input.Uid},Sign返回：{response.Content}");
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            KdyLog.LogError(response.ErrorException, "请求genshin sign Api异常,Input:{0},Response:{1}", request.Resource, response.Content);
            throw new KdyCustomException($"请求异常:{response.ErrorMessage}");
        }

        public async Task<GenShinResult<QuerySignInfoResult>> QuerySignInfoAsync(QuerySignInfoInput input)
        {
            var request = new RestRequest("/event/bbs_sign_reward/info", Method.GET);
            request.AddQueryParameter("act_id", input.ActId);
            request.AddQueryParameter("region", input.Region);
            request.AddQueryParameter("uid", input.Uid);
            request.AddHeader("Cookie", input.Cookie);
            request.AddHeader("Referer", "https://app.mihoyo.com");
            request.AddHeader("x-rpc-app_version", input.Version);
            var restClient = await GetBbsRestClient();
            var response = await restClient.ExecuteAsync<GenShinResult<QuerySignInfoResult>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            KdyLog.LogError(response.ErrorException, "请求genshin sign Api异常,Input:{0},Response:{1}", request.Resource, response.Content);
            throw new KdyCustomException($"请求异常:{response.ErrorMessage}");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<GenShinResult<QueryUserBindInfoByCookieResult>> QueryUserBindInfoByCookieAsync(QueryUserBindInfoByCookieInput input)
        {
            var request = new RestRequest("/binding/api/getUserGameRolesByCookie", Method.GET);
            request.AddHeader("Cookie", input.Cookie);
            request.AddHeader("Referer", "https://app.mihoyo.com");
            request.AddHeader("x-rpc-app_version", input.Version);
            var restClient = await GetBbsRestClient();
            var response = await restClient.ExecuteAsync<GenShinResult<QueryUserBindInfoByCookieResult>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            KdyLog.LogError(response.ErrorException, "请求genshin sign Api异常,Input:{0},Response:{1}", request.Resource, response.Content);
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
                BaseUrl = new Uri(DailyNoteApi),
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54",
            };
            restClient.AddDefaultHeader("x-requested-with", "com.mihoyo.hyperion");
            restClient.AddDefaultHeader("x-rpc-client_type", "5");

            restClient.AddDefaultHeader("Accept", "application/json,text/plain,*/*");
            restClient.UseNewtonsoftJson();
            return restClient;
        }

        /// <summary>
        /// 获取BBs Rest客户端
        /// </summary>
        /// <returns></returns>
        private async Task<RestClient> GetBbsRestClient()
        {
            await Task.CompletedTask;
            var restClient = new RestClient
            {
                BaseUrl = new Uri(BbsApi),
                UserAgent = "okhttp/4.8.0",
            };
            restClient.AddDefaultHeader("x-rpc-client_type", "2");
            restClient.AddDefaultHeader("x-rpc-sys_version", "6.0.1");
            restClient.AddDefaultHeader("x-rpc-channel", "miyousheluodi");
            restClient.AddDefaultHeader("x-rpc-device_id", Guid.NewGuid().ToString("N"));
            restClient.AddDefaultHeader("x-rpc-device_name", GetRandomStr());
            restClient.AddDefaultHeader("x-rpc-device_model", "Mi 10");
            restClient.UseNewtonsoftJson();
            return restClient;
        }

        /// <summary>
        /// 生成Ds
        /// </summary>
        /// <param name="salt">salt</param>
        /// <param name="queryParams">请求参数  xx=bb& y=cc</param>
        /// <param name="postJson">post json</param>
        /// <returns></returns>
        private string BuildDs(string salt, string queryParams, string postJson = "")
        {
            string time = DateTime.Now.ToSecondTimestamp() + "",
                randomStr = GetRandomStr(),
                queryStr = string.Join("&", queryParams.Split('&').OrderBy(x => x)),
                postStr = postJson;

            var signStr = $"salt={salt}&t={time}&r={randomStr}&b={postStr}&q={queryStr}".Md5Ext(solt: "");
            return $"{time},{randomStr},{signStr}";
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        private string GetRandomStr()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var rand = random.Next(100000, 200000);
            if (rand == 100000)
            {
                rand = 642367;
            }

            return rand.ToString();
        }

        /// <summary>
        /// 生成bbs Ds
        /// </summary>
        /// <param name="salt">salt</param>
        /// <returns></returns>
        private string BuildDsWithBbs(string salt)
        {
            string time = DateTime.Now.ToSecondTimestamp() + "",
                randomStr = GetRandomStr();

            var signStr = $"salt={salt}&t={time}&r={randomStr}".Md5Ext(solt: "");
            return $"{time},{randomStr},{signStr}";
        }
    }
}
