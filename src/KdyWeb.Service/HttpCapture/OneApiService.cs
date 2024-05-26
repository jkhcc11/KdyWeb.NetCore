using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.HttpCapture;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// OneApi 服务实现
    /// </summary>
    public class OneApiService : BaseKdyService, IOneApiService
    {
        private const int BaseRemainQuota = 500000;
        private const string BaseHost = "https://pro-proxy-web.gpt-666.com";
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        /// <summary>
        /// 
        /// </summary>
        public OneApiService(IUnitOfWork unitOfWork, IKdyRequestClientCommon kdyRequestClientCommon) : base(unitOfWork)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        /// <summary>
        /// 批量创建Token
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<BatchCreateOneApiTokenOut>>> BatchCreateOneApiTokenAsync(BatchCreateOneApiTokenInput input)
        {
            var errMsg = new StringBuilder();
            var result = new List<BatchCreateOneApiTokenOut>();
            for (var i = 1; i <= input.TokenCount; i++)
            {
                input.TokenName = $"{input.TokenNamePrefix}_{i}";
                var send = await SendHttpAsync(input);
                if (send.IsSuccess == false)
                {
                    errMsg.AppendLine(send.Msg);
                    KdyLog.LogWarning($"创建Token失败,{send.Msg}");
                    continue;
                }

                await Task.Delay(200);
                result.Add(send.Data);
            }

            if (result.Any() == false)
            {
                return KdyResult.Error<List<BatchCreateOneApiTokenOut>>(KdyResultCode.Error, errMsg.ToString());
            }

            return KdyResult.Success(result);
        }

        private async Task<KdyResult<BatchCreateOneApiTokenOut>> SendHttpAsync(BatchCreateOneApiTokenInput input)
        {
            var baseHost = KdyConfiguration.GetValue<string>("OneApiHost") ??
                           BaseHost;
            var req = new KdyRequestCommonInput(baseHost)
            {
                ExtData = new KdyRequestCommonExtInput()
                {
                    HeardDic = new Dictionary<string, string>()
                    {
                        {"Authorization",$"Bearer {input.ApiToken}"}
                    }
                }
            };
            input.ExpiredTimeExt = input.ExpiredTime.ToSecondTimestamp();
            input.RemainQuota *= BaseRemainQuota;
            var reqJson = input.ToJsonStr();
            req.SetPostData("/api/token/", reqJson);

            var response = await _kdyRequestClientCommon.SendAsync(req);
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<BatchCreateOneApiTokenOut>(KdyResultCode.HttpError, response.ErrMsg);
            }

            var jObject = JObject.Parse(response.Data);
            if (jObject.GetValue("success")?.Value<bool>() == false)
            {
                return KdyResult.Error<BatchCreateOneApiTokenOut>(KdyResultCode.Error, jObject.GetValue("message")?.Value<string>());
            }

            return KdyResult.Success(new BatchCreateOneApiTokenOut()
            {
                Key = jObject["data"]?["key"] + ""
            });
        }
    }
}
