using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyOptions;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 系统相关
    /// </summary>
    [CustomRoute("system")]
    public class SystemController : BaseApiController
    {
        private readonly IParseSystemService _parseSystemService;
        private readonly IOneApiService _oneApiService;
        private readonly ITxDocWebService _txDocWebService;
        private readonly TxDocRecordsOption _txDocRecordsOption;

        public SystemController(IParseSystemService parseSystemService, IOneApiService oneApiService,
            ITxDocWebService txDocWebService, IOptions<TxDocRecordsOption> options)
        {
            _parseSystemService = parseSystemService;
            _oneApiService = oneApiService;
            _txDocWebService = txDocWebService;
            _txDocRecordsOption = options.Value;
        }

        /// <summary>
        /// 影片发送入库
        /// </summary>
        /// <returns></returns>
        [HttpPost("vod-send")]
        public async Task<KdyResult> ParseVodSendAsync(ParseVodSendInput input)
        {
            var result = await _parseSystemService.ParseVodSendAsync(input);
            return result;
        }

        /// <summary>
        /// 批量创建Token
        /// </summary>
        /// <returns></returns>
        [HttpPost("batch-create-token")]
        public async Task<KdyResult> BatchCreateOneApiTokenAsync(BatchCreateOneApiTokenInput input)
        {
            var result = await _oneApiService.BatchCreateOneApiTokenAsync(input);
            return result;
        }

        /// <summary>
        /// 获取接龙记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-doc-records")]
        [AllowAnonymous]
        public async Task<KdyResult> CreateProxyAsync([FromQuery] string docUrl)
        {
            var input = new GetSequenceRecordsInput()
            {
                DocUrl = docUrl,
                UserLoginCookie = _txDocRecordsOption.UserLoginCookie
            };
            var result = await _txDocWebService.GetSequenceRecordsAsync(input);

            return result;
        }
    }
}
