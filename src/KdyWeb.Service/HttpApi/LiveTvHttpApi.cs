using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.LiveTv;
using KdyWeb.IService.HttpApi;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace KdyWeb.Service.HttpApi
{
    /// <summary>
    /// IPTV直播 服务实现
    /// </summary>
    public class LiveTvHttpApi : BaseKdyService, ILiveTvHttpApi
    {
        /// <summary>
        ///  api
        /// </summary>
        private const string BaseApi = "https://iptv-org.github.io";

        public LiveTvHttpApi(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 获取所有频道
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetAllChannelsDto>> GetAllChannelsAsync()
        {
            var request = new RestRequest("/api/channels.json");
            var restClient = await GetRestClient();
            var response = await restClient.ExecuteAsync<List<GetAllChannelsDto>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            KdyLog.LogError(response.ErrorException, "请求iptv-org channels Api异常,Input:{0},Response:{1}", request.Resource, response.Content);
            throw new KdyCustomException($"请求异常:{response.ErrorMessage}");
        }

        /// <summary>
        /// 获取实时流检查列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetAllStreamsDto>> GetAllStreamsAsync()
        {
            var request = new RestRequest("/api/streams.json");
            var restClient = await GetRestClient();
            var response = await restClient.ExecuteAsync<List<GetAllStreamsDto>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            KdyLog.LogError(response.ErrorException, "请求iptv-org streams Api异常,Input:{0},Response:{1}", request.Resource, response.Content);
            throw new KdyCustomException($"请求异常:{response.ErrorMessage}");
        }

        /// <summary>
        /// 获取Rest客户端
        /// </summary>
        /// <returns></returns>
        private async Task<RestClient> GetRestClient()
        {
            await Task.CompletedTask;
            var options = new RestClientOptions(BaseApi)
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54",
            };
            var restClient = new RestClient(options, configureSerialization: cfg => cfg.UseNewtonsoftJson());

            //var restClient = new RestClient
            //{
            //    BaseUrl = new Uri(BaseApi),
            //    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54",
            //};
            //restClient.UseNewtonsoftJson();
            return restClient;
        }
    }
}
