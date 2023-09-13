using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Job;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 基于Server酱发送消息job
    /// </summary>
    [Queue(HangFireQueue.GameCheck)]
    [AutomaticRetry(Attempts = 1)]
    public class SendMsgWithFtqqJobService : BaseKdyJob<SendMsgWithFtqqJobInput>
    {
        public override async Task ExecuteAsync(SendMsgWithFtqqJobInput input)
        {
            var request = new RestRequest("SCT166795Tp7Q1qE4FRHy70pP7AAE0HVzL.send", Method.Post);
            request.AddParameter("title", input.Title);
            request.AddParameter("desp", input.Content);

            var client = await GetRestClient();
            var response = await client.ExecuteAsync(request);
            KdyLog.LogInformation($"消息推送返回：{response.Content}");
        }

        /// <summary>
        /// 获取Rest客户端
        /// </summary>
        /// <returns></returns>
        private async Task<RestClient> GetRestClient()
        {
            await Task.CompletedTask;
            //var restClient = new RestClient
            //{
            //    BaseUrl = new Uri("https://sctapi.ftqq.com"),
            //    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54",
            //};
            var options = new RestClientOptions("https://sctapi.ftqq.com");
            var restClient = new RestClient(options, configureSerialization: cfg => cfg.UseNewtonsoftJson());

            restClient.AddDefaultHeader("Accept", "application/json,text/plain,*/*");
            //restClient.UseNewtonsoftJson();
            return restClient;
        }
    }
}
