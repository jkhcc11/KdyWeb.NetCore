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
        [AllowAnonymous]
        public async Task<KdyResult<IList<QueryPageSequenceUseRecordDto>>> CreateSequenceUseRecordByTxDocAsync(CreateSequenceUseRecordByTxDocInput input)
        {
            var result = await _sequenceUseRecordService.CreateSequenceUseRecordByTxDocAsync(input);
            return result;
        }

        /// <summary>
        /// 更新接龙使用记录
        /// </summary>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<KdyResult> UpdateSequenceUseRecordAsync(UpdateSequenceUseRecordInput input)
        {
            var result = await _sequenceUseRecordService.UpdateSequenceUseRecordAsync(input);
            return result;
        }

        /// <summary>
        /// 分页获取接龙使用记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-sequence-use-record")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<KdyResult<PageList<QueryPageSequenceUserRecordDto>>> QueryPageSequenceUserRecordAsync([FromQuery] QueryPageSequenceUserRecordInput input)
        {
            var result = await _sequenceUseRecordService.QueryPageSequenceUserRecordAsync(input);
            return result;
        }

        /// <summary>
        /// 获取所有用户接龙记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-user-record")]
        [AllowAnonymous]
        public async Task<KdyResult<List<SelectedItemOut>>> GetAllUserRecordAsync()
        {
            var result = await _sequenceUseRecordService.GetAllUserRecordAsync();
            return result;
        }

        /// <summary>
        /// 根据场馆获取腾讯文档缓存地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-today-url")]
        [AllowAnonymous]
        public async Task<KdyResult<string>> GetTodayTxDocUrlCacheAsync(string placeTxt)
        {
            var result = await _sequenceUseRecordService.GetTodayTxDocUrlCacheAsync(placeTxt);
            return result;
        }
    }
}
