using System;
using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;
using KdyWeb.WebParse;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// CCTV解析 服务实现
    /// </summary>
    public class CctvWebParseService : BaseKdyWebParseService<CctvParseConfig, KdyWebParseInput, KdyWebParseOut>, ICctvWebParseService
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;

        public CctvWebParseService(IKdyRequestClientCommon kdyRequestClientCommon)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        public override async Task<KdyResult<KdyWebParseOut>> GetNoCacheResultAsync(KdyWebParseInput input)
        {
            var reqInput = new KdyRequestCommonInput(input.DetailUrl, HttpMethod.Get)
            {
                UserAgent = BaseConfig.UserAgent,
                TimeOut = 3000
            };
            var reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error<KdyWebParseOut>(KdyResultCode.Error, "解析失败，获取Guid失败");
            }

            //请求Api
            var guid = reqResult.Data.Replace(" ", "")
                .GetStrMathExt("guid=\"", "\";");
            var key = GetVc();
            var apiUrl = $"{BaseConfig.ApiHost}/api/getHttpVideoInfo.do?pid={guid}&client=flash&tsp={key.Item1}&vn={BaseConfig.Vn}&vc={key.Item2}&uid={GetOneKey()}&wlan=";
            reqInput = new KdyRequestCommonInput(apiUrl, HttpMethod.Get)
            {
                UserAgent = BaseConfig.UserAgent,
                TimeOut = 3000
            };
            reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error<KdyWebParseOut>(KdyResultCode.Error, "解析失败，获取Data失败");
            }

            //获取m3u8地址
            var tempJson = reqResult.Data.Replace(" ", "")
                .GetStrMathExt("'", "'");
            var jObject = JObject.Parse(tempJson);
            string url = jObject.GetValueExt("hls_url");
            if (string.IsNullOrEmpty(url))
            {
                return KdyResult.Error<KdyWebParseOut>(KdyResultCode.Error, "解析失败，获取hls地址失败");
            }

            //获取高画质地址 优先1080p
            reqInput = new KdyRequestCommonInput(url, HttpMethod.Get)
            {
                UserAgent = BaseConfig.UserAgent,
                TimeOut = 3000
            };
            reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error<KdyWebParseOut>(KdyResultCode.Error, "解析失败，获取内容失败");
            }

            var tempM3U8Array = reqResult.Data.Split(new[]
            {
                "\r", "\n", ""
            }, StringSplitOptions.RemoveEmptyEntries);
            if (tempM3U8Array.Length <= 0)
            {
                return KdyResult.Error<KdyWebParseOut>(KdyResultCode.Error, "解析失败，获取M3u8失败");
            }

            var host = new Uri(url).Host;
            var result = new KdyWebParseOut()
            {
                ResultUrl = $"//{host}{tempM3U8Array[^1]}",
                Type = WebParseType.M3u8
            };
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 获取唯一指纹
        /// </summary>
        /// <returns></returns>
        private string GetOneKey()
        {
            return DateTime.Now.ToString("yyyyMMddHH").Md5Ext();
        }

        /// <summary>
        /// 获取Vc值
        /// </summary>
        /// <returns>
        /// Item1时间戳
        /// Item2 vc值 
        /// </returns>
        private Tuple<string, string> GetVc()
        {
            string time = DateTime.Now.ToSecondTimestamp() + "";
            var vc = $"{time}{BaseConfig.Vn}{BaseConfig.StaticCheck}{GetOneKey()}".Md5Ext().ToUpper();
            return new Tuple<string, string>(time, vc);
        }

    }
}
