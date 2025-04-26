using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.CloudParse.Input;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.HttpCapture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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
        private readonly IKdyRedisCache _redisCache;

        public SystemController(IParseSystemService parseSystemService, IOneApiService oneApiService,
            IKdyRedisCache redisCache)
        {
            _parseSystemService = parseSystemService;
            _oneApiService = oneApiService;
            _redisCache = redisCache;
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
        /// 创建代理缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet("create-proxy")]
        public async Task<KdyResult> CreateProxyAsync([FromQuery] long userId, [FromQuery] string proxyInfo)
        {
            //todo:临时redis获取 加管理功能后移除
            await _redisCache.GetCache().SetStringAsync($"proxy:{userId}", proxyInfo);

            var getTemp = await _redisCache.GetCache().GetStringAsync($"proxy:{userId}");
            return KdyResult.Success($"操作成功：{getTemp}");
        }
    }
}
