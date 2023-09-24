using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.CloudParse;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 解析记录
    /// </summary>
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.NormalPolicy)]
    [CustomRoute("record-history")]
    public class ParseRecordHistoryController : BaseApiController
    {
        private readonly IParseRecordHistoryService _parseRecordHistoryService;

        public ParseRecordHistoryController(IParseRecordHistoryService parseRecordHistoryService)
        {
            _parseRecordHistoryService = parseRecordHistoryService;
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<PageList<QueryParseRecordHistoryDto>>> QueryParseRecordHistoryAsync(
            [FromQuery] QueryParseRecordHistoryInput input)
        {
            var result = await _parseRecordHistoryService.QueryParseRecordHistoryAsync(input);
            return result;
        }

        /// <summary>
        /// 获取访问前N条文件信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-top")]
        public async Task<KdyResult<List<GetTopFileInfoDto>>> GetTopFileInfoAsync([FromQuery] GetTopFileInfoInput input)
        {
            var result = await _parseRecordHistoryService.GetTopFileInfoAsync(input);
            return result;
        }

        /// <summary>
        /// 查询时间范围内记录按天汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-day-sum")]
        public async Task<KdyResult<List<QueryRecordDaySumByDateRangeDto>>> QueryRecordDaySumByDateRangeAsync([FromQuery] QueryRecordSumByDateRangeInput input)
        {
            var result = await _parseRecordHistoryService.QueryRecordDaySumByDateRangeAsync(input);
            return result;
        }
    }
}
