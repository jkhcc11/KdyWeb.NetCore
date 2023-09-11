using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.CloudParse;
using KdyWeb.IService.CloudParse;
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

        public SystemController(IParseSystemService parseSystemService)
        {
            _parseSystemService = parseSystemService;
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
    }
}
