using System;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyOptions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.AuthCenter;
using KdyWeb.ICommonService;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace KdyWeb.CommonService
{
    /// <summary>
    /// 授权中心Api 实现
    /// </summary>
    public class AuthCenterHttpApi : BaseKdyService, IAuthCenterHttpApi
    {
        private readonly ICrossRequestService _crossRequestService;
        private readonly KdyAuthServerOption _kdyAuthServerOption;
        public AuthCenterHttpApi(IUnitOfWork unitOfWork, ICrossRequestService crossRequestService,
            IOptions<KdyAuthServerOption> options) : base(unitOfWork)
        {
            _crossRequestService = crossRequestService;
            _kdyAuthServerOption = options.Value;
        }

        public async Task<KdyResult<CreateUserResponse>> CreateUserAsync(string userName, string userNick, string userEmail)
        {
            var request = new RestRequest("/api/Users", Method.Post);
            request.AddJsonBody(new
            {
                userName = userName,
                email = userEmail,
                emailConfirmed = true
            });
            var response = await SendHttp<CreateUserResponse>(request);
            if (response.IsSuccess)
            {
                return KdyResult.Success(response.Data);
            }

            var jsonObj = JObject.Parse(response.Msg);
            var exitsUser = jsonObj.GetValueExt("errors.DuplicateUserName");
            if (string.IsNullOrEmpty(exitsUser) == false)
            {
                return KdyResult.Error<CreateUserResponse>(KdyResultCode.Duplicate, "用户名已存在");
            }

            exitsUser = jsonObj.GetValueExt("errors.DuplicateEmail");
            if (string.IsNullOrEmpty(exitsUser) == false)
            {
                return KdyResult.Error<CreateUserResponse>(KdyResultCode.Duplicate, "邮箱已存在");
            }

            return KdyResult.Error<CreateUserResponse>(KdyResultCode.Error, "创建用户错误,请重试");
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ChangePwdAsync(long userId, string pwd)
        {
            var request = new RestRequest("/api/Users/ChangePassword", Method.Post);
            request.AddJsonBody(new
            {
                userId = userId,
                password = pwd,
                confirmPassword = pwd
            });
            var result = await SendHttp(request);
            if (result.IsSuccess == false)
            {
                return result;
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult> SetUserRoleAsync(long userId, long roleId)
        {
            var request = new RestRequest("/api/Users/Roles", Method.Post);
            request.AddJsonBody(new
            {
                userId = userId,
                roleId = roleId
            });
            var result = await SendHttp(request);
            if (result.IsSuccess == false)
            {
                return result;
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult<SearchUserResponse>> SearchUserAsync(string userEmail)
        {
            var request = new RestRequest("/api/Users");
            request.AddParameter("searchText", userEmail);
            var response = await SendHttp<SearchUserResponse>(request);
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<SearchUserResponse>(KdyResultCode.Error, response.Msg);
            }

            return KdyResult.Success(response.Data);
        }

        public async Task<KdyResult> SetUserClaimsAsync(long userId, string claimType, string claimValue)
        {
            var request = new RestRequest("/api/Users/Claims", Method.Post);
            request.AddJsonBody(new
            {
                userId = userId,
                claimType = claimType,
                claimValue = claimValue
            });
            var result = await SendHttp(request);
            if (result.IsSuccess == false)
            {
                return result;
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult> UpdateUserClaimsAsync(long userId, int claimId, string claimType, string claimValue)
        {
            var request = new RestRequest("/api/Users/Claims", Method.Put);
            request.AddJsonBody(new
            {
                claimId = claimId,
                userId = userId,
                claimType = claimType,
                claimValue = claimValue
            });
            var result = await SendHttp(request);
            if (result.IsSuccess == false)
            {
                return result;
            }

            return KdyResult.Success();
        }

        public async Task<KdyResult<GetUserClaimsResponse>> GetUserClaimsAsync(long userId)
        {
            var request = new RestRequest($"/api/Users/{userId}/Claims");
            var response = await SendHttp<GetUserClaimsResponse>(request);
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<GetUserClaimsResponse>(KdyResultCode.Error, response.Msg);
            }

            return KdyResult.Success(response.Data);
        }

        /// <summary>
        /// 发送Http
        /// </summary>
        /// <typeparam name="TResponse">返回值</typeparam>
        /// <returns></returns>
        private async Task<KdyResult<TResponse>> SendHttp<TResponse>(RestRequest request)
            where TResponse : new()
        {
            var restClient = await GetRestClient();
            var response = await restClient.ExecuteAsync<TResponse>(request);
            if (response.IsSuccessful)
            {
                return KdyResult.Success(response.Data);
            }

            //400需要返回
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return KdyResult.Error<TResponse>(KdyResultCode.Error, response.Content);
            }

            KdyLog.LogError(response.ErrorException, "请求授权中心异常,Input:{0},Response:{1}", request.Resource, response.Content);
            throw new KdyCustomException("请求授权异常01");
        }

        /// <summary>
        /// 发送Http
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> SendHttp(RestRequest request)
        {
            var restClient = await GetRestClient();
            var response = await restClient.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                return KdyResult.Success();
            }

            KdyLog.LogError(response.ErrorException, "请求授权中心异常,Input:{0},Response:{1}", request.Resource, response.Content);
            //400需要返回
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return KdyResult.Error(KdyResultCode.Error, response.Content);
            }

            throw new KdyCustomException("请求授权异常02");
        }

        /// <summary>
        /// 获取Rest客户端
        /// </summary>
        /// <returns></returns>
        private async Task<RestClient> GetRestClient()
        {
            var mgrToken = await _crossRequestService.GetAuthCenterMgrTokenAsync();

            var options = new RestClientOptions(_kdyAuthServerOption.AuthMgrApiHost)
            {
                Authenticator = new JwtAuthenticator(mgrToken.AccessToken)
            };
            return new RestClient(options);

            //old
            //var restClient = new RestClient
            //{
            //    Authenticator = new JwtAuthenticator(mgrToken.AccessToken),
            //    BaseUrl = new Uri()
            //};
            // return restClient;
        }

    }
}
