using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SequenceRecord;
using KdyWeb.IService.SequenceRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 接龙使用记录
    /// </summary>
    [CustomRoute("sequence-use-record")]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "ppv1")]
    public class SequenceUseRecordController : BaseApiController
    {
        private readonly ISequenceUseRecordService _sequenceUseRecordService;

        public SequenceUseRecordController(ISequenceUseRecordService sequenceUseRecordService)
        {
            _sequenceUseRecordService = sequenceUseRecordService;
        }

        /// <summary>
        /// 根据腾讯接龙记录创建接龙记录
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult<IList<QueryPageSequenceUseRecordDto>>> CreateSequenceUseRecordByTxDocAsync(CreateSequenceUseRecordByTxDocInput input)
        {
            var result = await _sequenceUseRecordService.CreateSequenceUseRecordByTxDocAsync(input);
            return result;
        }

        /// <summary>
        /// 分页获取接龙使用记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-sequence-use-record")]
        public async Task<KdyResult<PageList<QueryPageSequenceUseRecordDto>>> QueryPageSequenceUseRecordAsync([FromQuery] QueryPageSequenceUseRecordInput input)
        {
            var result = await _sequenceUseRecordService.QueryPageSequenceUseRecordAsync(input);
            return result;
        }

        /// <summary>
        /// 分页获取用户接龙列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-sequence-user-record")]
        public async Task<KdyResult<PageList<QueryPageSequenceUserRecordDto>>> QueryPageSequenceUserRecordAsync([FromQuery] QueryPageSequenceUserRecordInput input)
        {
            var result = await _sequenceUseRecordService.QueryPageSequenceUserRecordAsync(input);
            return result;
        }

    }
}
