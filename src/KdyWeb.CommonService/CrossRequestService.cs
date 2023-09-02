using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.KdyOptions;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Cache;
using KdyWeb.ICommonService;
using Microsoft.Extensions.Options;

namespace KdyWeb.CommonService
{
    /// <summary>
    /// 跨域访问 服务实现
    /// </summary>
    public class CrossRequestService : BaseKdyService, ICrossRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly KdyAuthServerOption _kdyAuthServerOption;
        private readonly IKdyRedisCache _kdyRedisCache;
        private const string TokenCache = "Cross_Token_Cache";
        private const string AuthCenterMgrTokenCache = "AuthCenterMgr_Token_Cache";

        public CrossRequestService(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory,
            IOptions<KdyAuthServerOption> options, IKdyRedisCache kdyRedisCache) : base(unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _kdyRedisCache = kdyRedisCache;
            _kdyAuthServerOption = options.Value;
        }

        public async Task<CrossTokenCacheItem> GetCrossTokenAsync()
        {
            var reqToken = await _kdyRedisCache.GetCache().GetValueAsync<CrossTokenCacheItem>(TokenCache);
            if (reqToken != null)
            {
                return reqToken;
            }

            var tokenRequest = new TokenRequest()
            {
                Address = $"{_kdyAuthServerOption.AuthHost}/connect/token",
                GrantType = "client_credentials",
                ClientId = _kdyAuthServerOption.ClientId,
                ClientSecret = _kdyAuthServerOption.ClientSecret,
                Parameters = new Dictionary<string, string>()
                {
                    {"scope",$"kdy_cross_auth {_kdyAuthServerOption.Scope}"}
                }
            };

            var tokenResponse = await SendToken(tokenRequest);
            var tokenCache = new CrossTokenCacheItem()
            {
                AccessToken = tokenResponse.AccessToken,
                TokenType = tokenResponse.TokenType
            };
            await _kdyRedisCache.GetCache()
                .SetValueAsync(TokenCache, tokenCache, TimeSpan.FromMinutes(60 * 2 + 45));
            return tokenCache;
        }

        public async Task<CrossTokenCacheItem> GetAuthCenterMgrTokenAsync()
        {
            var reqToken = await _kdyRedisCache.GetCache().GetValueAsync<CrossTokenCacheItem>(AuthCenterMgrTokenCache);
            if (reqToken != null)
            {
                return reqToken;
            }

            var tokenRequest = new TokenRequest()
            {
                Address = $"{_kdyAuthServerOption.AuthHost}/connect/token",
                GrantType = "password",
                ClientId = "kdy_admin_client",
                ClientSecret = _kdyAuthServerOption.AuthMgrSecret,
                Parameters = new Dictionary<string, string>()
                {
                    {"scope","kdy_admin_client_api"},
                    {"username",_kdyAuthServerOption.AuthMgrUser},
                    {"password",_kdyAuthServerOption.AuthMgrUserPwd}
                }
            };

            var tokenResponse = await SendToken(tokenRequest);
            if (tokenResponse.IsError)
            {
                throw new KdyCustomException("获取Token失败");
            }

            var tokenCache = new CrossTokenCacheItem()
            {
                AccessToken = tokenResponse.AccessToken,
                TokenType = tokenResponse.TokenType
            };
            await _kdyRedisCache.GetCache()
                .SetValueAsync(AuthCenterMgrTokenCache, tokenCache, TimeSpan.FromMinutes(55));
            return tokenCache;
        }

        public async Task<KdyResult<CrossTokenCacheItem>> GetAccessTokenByUserNameOrEmailAsync(string userNameOrEmail, string pwd)
        {
            var tokenRequest = new TokenRequest()
            {
                Address = $"{_kdyAuthServerOption.AuthHost}/connect/token",
                GrantType = "kdy-login",
                ClientId = _kdyAuthServerOption.ClientId,
                ClientSecret = _kdyAuthServerOption.ClientSecret,
                Parameters = new Dictionary<string, string>()
                {
                    {"scope",_kdyAuthServerOption.AllScope},
                    {"username",userNameOrEmail},
                    {"password",pwd}
                }
            };

            var tokenResponse = await SendToken(tokenRequest);
            if (tokenResponse.IsError)
            {
                return KdyResult.Error<CrossTokenCacheItem>(KdyResultCode.Error, tokenResponse.ErrorDescription);
            }

            var tokenCache = new CrossTokenCacheItem()
            {
                AccessToken = tokenResponse.AccessToken,
                TokenType = tokenResponse.TokenType,
                RefreshToken = tokenResponse.RefreshToken,
            };
            return KdyResult.Success(tokenCache);
        }

        /// <summary>
        /// 通过刷新Token获取 访问Token
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<CrossTokenCacheItem>> GetAccessTokenByRefreshAsync(string refreshToken)
        {
            var tokenRequest = new RefreshTokenRequest()
            {
                Address = $"{_kdyAuthServerOption.AuthHost}/connect/token",
                RefreshToken = refreshToken,
                ClientId = _kdyAuthServerOption.ClientId,
                ClientSecret = _kdyAuthServerOption.ClientSecret,
            };

            var client = _httpClientFactory.CreateClient("RefreshToken");
            var tokenResponse = await client.RequestRefreshTokenAsync(tokenRequest);
            if (tokenResponse.IsError)
            {
                return KdyResult.Error<CrossTokenCacheItem>(KdyResultCode.Error, tokenResponse.ErrorDescription);
            }

            var tokenCache = new CrossTokenCacheItem()
            {
                AccessToken = tokenResponse.AccessToken,
                TokenType = tokenResponse.TokenType,
                RefreshToken = tokenResponse.RefreshToken,
            };
            return KdyResult.Success(tokenCache);
        }

        /// <summary>
        /// 发起Token请求
        /// </summary>
        /// <returns></returns>
        private async Task<TokenResponse> SendToken(TokenRequest tokenRequest)
        {
            var client = _httpClientFactory.CreateClient($"{Guid.NewGuid()}");
            var tokenResponse = await client.RequestTokenAsync(tokenRequest);
            return tokenResponse;
        }
    }
}
