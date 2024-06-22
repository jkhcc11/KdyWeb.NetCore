using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Mvc;

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

        public SystemController(IParseSystemService parseSystemService, IOneApiService oneApiService)
        {
            _parseSystemService = parseSystemService;
            _oneApiService = oneApiService;
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
    }
}
