using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.HttpCapture;
using KdyWeb.WebParse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 通用站点解析 服务实现
    /// </summary>
    public class NormalWebParseService : BaseKdyWebParseService<NormalParseConfig, KdyWebParseInput, KdyWebParseOut>, INormalWebParseService
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;

        public NormalWebParseService(IKdyRequestClientCommon kdyRequestClientCommon)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        public override async Task<KdyResult<KdyWebParseOut>> GetNoCacheResultAsync(KdyWebParseInput input)
        {
            var url = $"{BaseConfig.ApiHost}/api/Parse/kdyParse?url={input.DetailUrl}";
            var reqInput = new KdyRequestCommonInput(url, HttpMethod.Get)
            {
                TimeOut = 10000
            };
            var reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error<KdyWebParseOut>(KdyResultCode.Error, "解析失败");
            }

            var jObject = (JObject)JsonConvert.DeserializeObject(reqResult.Data);
            if (jObject == null || jObject["code"] + "" != "200")
            {
                return KdyResult.Error<KdyWebParseOut>(KdyResultCode.Error, jObject?["message"] + "");
            }

            var result = new KdyWebParseOut()
            {
                ResultUrl = jObject["data"] + "",
                Type = WebParseType.M3u8
            };
            result.ResultUrl = result.ResultUrl.Replace("http:", "")
                .Replace("https:", "");

            return KdyResult.Success(result);
        }
    }
}
