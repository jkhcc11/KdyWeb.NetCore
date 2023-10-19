using System.Net.Http.Json;
using System.Text;
using System.Web;
using KdyWeb.IService.Selenium;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;
using KdyWeb.Dto.Selenium;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace KdyWeb.Service.Selenium
{
    /// <summary>
    /// 通过二维码登录
    /// </summary>
    public class LoginByQrService : ILoginByQrService
    {
        private const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36";
        private const string AliQrLoginBaseUrl = "https://passport.aliyundrive.com";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LoginByQrService> _logger;
        private const string RefUrl =
           "https://passport.aliyundrive.com/mini_login.htm?lang=zh_cn&appName=aliyun_drive&appEntrance=&styleType=auto&bizParams=&notLoadSsoView=false&notKeepLogin=false&isMobile=false&ad__pass__q__rememberLogin=true&ad__pass__q__rememberLoginDefaultValue=true&ad__pass__q__forgotPassword=true&ad__pass__q__licenseMargin=true&ad__pass__q__loginType=normal&hidePhoneCode=true&rnd=0.{time}";

        public LoginByQrService(IHttpClientFactory httpClientFactory, ILogger<LoginByQrService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// 阿里云盘Qr
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QrLoginInitWithAliOut>> QrLoginInitWithAliAsync()
        {
            var client = _httpClientFactory.CreateClient(Guid.NewGuid().ToString());
            client.BaseAddress = new Uri(AliQrLoginBaseUrl);
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            var urlBuild = new UriBuilder(AliQrLoginBaseUrl + "/newlogin/qrcode/generate.do");
            var reqParams = HttpUtility.ParseQueryString(urlBuild.Query);
            reqParams.Add("appName", "aliyun_drive");
            reqParams.Add("fromSite", "52");
            reqParams.Add("appEntrance", "web");
            reqParams.Add("isMobile", "false");

            reqParams.Add("lang", "zh_CN");
            reqParams.Add("returnUrl", "");
            reqParams.Add("bizParams", "");
            urlBuild.Query = reqParams.ToString();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, urlBuild.Uri.PathAndQuery);
            requestMessage.Headers.Referrer = new Uri(RefUrl.Replace("{time}", DateTime.Now.ToMillisecondTimestamp() + ""));

            var response = await client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode == false)
            {
                _logger.LogWarning("获取二维码异常,{msg}", await response.Content.ReadAsStringAsync());
                return KdyResult.Error<QrLoginInitWithAliOut>(KdyResultCode.Error, "获取异常");
            }


            var resultJsonStr = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(resultJsonStr);
            if (jObject["content"]?["success"] + "" != "True")
            {
                return KdyResult.Error<QrLoginInitWithAliOut>(KdyResultCode.Error, "获取参数异常");
            }

            var resultDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jObject["content"]?["data"] + "");
            if (resultDic == null)
            {
                return KdyResult.Error<QrLoginInitWithAliOut>(KdyResultCode.Error, "获取参数异常 01");
            }

            var result = new QrLoginInitWithAliOut()
            {
                Time = resultDic["t"] + "",
                CKey = resultDic["ck"] + "",
                CodeContent = resultDic["codeContent"] + "",
            };

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 阿里云盘qr获取token
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<string>> QrLoginGetTokenWithAliAsync(QrLoginGetTokenInput input)
        {
            var client = _httpClientFactory.CreateClient(Guid.NewGuid().ToString());
            client.BaseAddress = new Uri(AliQrLoginBaseUrl);
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            var postData = new Dictionary<string, string>
            {
                { "t", input.AliTime+"" },
                { "ck", input.AliCKey },
                { "ua", "" },
                { "appName", "aliyun_drive" },
                { "appEntrance", "web" },
                { "_csrf_token", "" },
                { "umidToken", "" },
                { "isMobile", "false" },
                { "lang", "zh_CN" },
                { "returnUrl", "" },
                { "hsiz", "" },
                { "fromSite", "52" },
                { "bizParams", "" },
                { "navlanguage", "zh-CN" },
                { "navUserAgent", "Mozilla%2F5.0%20%28Windows%20NT%2010.0%3B%20Win64%3B%20x64%29%20AppleWebKit%2F537.36%20%28KHTML%2C%20like%20Gecko%29%20Chrome%2F102.0.0.0%20Safari%2F537.36" },
                { "navPlatform", "Win32" },
                { "deviceId", "" },
                { "pageTraceId", "" }
            };

            var urlBuild = new UriBuilder(AliQrLoginBaseUrl + "/newlogin/qrcode/query.do");
            var reqParams = HttpUtility.ParseQueryString(urlBuild.Query);
            reqParams.Add("appName", "aliyun_drive");
            reqParams.Add("fromSite", "52");
            reqParams.Add("_bx-v", "2.0.31");
            urlBuild.Query = reqParams.ToString();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, urlBuild.Uri.PathAndQuery);
            requestMessage.Headers.Referrer = new Uri(RefUrl.Replace("{time}", DateTime.Now.ToMillisecondTimestamp() + ""));
            requestMessage.Content = new FormUrlEncodedContent(postData);

            var response = await client.SendAsync(requestMessage);
            var responseStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode == false)
            {
                _logger.LogWarning("获取Token异常,{msg}", responseStr);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取异常");
            }

            _logger.LogInformation("获取Qr结果,返回：{c}", responseStr);
            var jObject = JObject.Parse(responseStr);
            if (jObject["content"]?["data"]?["loginResult"] + "" != "success")
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取失败");
            }

            var base64Result = jObject["content"]?["data"]?["bizExt"] + "";
            if (string.IsNullOrEmpty(base64Result))
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取失败01");
            }

            var base64ToJson = base64Result.Base64ToStrExt(Encoding.UTF8);
            var result = JObject.Parse(base64ToJson);
            return KdyResult.Success<string>(result["pds_login_result"]?["refreshToken"] + "");
        }
    }
}
