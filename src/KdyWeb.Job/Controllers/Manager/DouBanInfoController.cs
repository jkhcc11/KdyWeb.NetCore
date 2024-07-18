using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.HttpApi.DouBan;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 豆瓣信息
    /// </summary>
    public class DouBanInfoController : BaseManagerController
    {
        private readonly IDouBanInfoService _douBanInfoService;

        public DouBanInfoController(IDouBanInfoService douBanInfoService)
        {
            _douBanInfoService = douBanInfoService;
        }

        /// <summary>
        /// 创建并获取豆瓣信息
        /// </summary>
        /// <param name="subject">豆瓣Id</param>
        /// <returns></returns>
        [HttpPost("createGet/{subject}")]
        [ProducesResponseType(typeof(KdyResult<CreateForSubjectIdDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateForSubjectIdAsync(string subject)
        {
            var result = await _douBanInfoService.CreateForSubjectIdAsync(subject);
            return Ok(result);
        }

        /// <summary>
        /// 查询豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryDouBanInfoDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryDouBanInfoAsync([FromQuery] QueryDouBanInfoInput input)
        {
            var result = await _douBanInfoService.QueryDouBanInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        [ProducesResponseType(typeof(KdyResult<GetDouBanInfoForIdDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDouBanInfoForIdAsync(int id)
        {
            var result = await _douBanInfoService.GetDouBanInfoForIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// 变更豆瓣信息状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("change")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeDouBanInfoAsync(ChangeDouBanInfoStatusInput input)
        {
            var result = await _douBanInfoService.ChangeDouBanInfoStatusAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 重试保存图片
        /// </summary>
        /// <returns></returns>
        [HttpPut("retrySaveImg/{id}")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RetrySaveImgAsync(int id)
        {
            var result = await _douBanInfoService.RetrySaveImgAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("batchDelete")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public async Task<IActionResult> BatchDeleteAsync(BatchDeleteForIntKeyInput input)
        {
            var result = await _douBanInfoService.DeleteAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 豆瓣关键字搜索
        /// </summary>
        /// <returns></returns>
        [HttpGet("keyword-search")]
        public async Task<KdyResult<PageList<SearchSuggestResponse>>> DouBanKeyWordSearchAsync([FromQuery] DouBanKeyWordSearchInput input)
        {
            var result = await _douBanInfoService.DouBanKeyWordSearchAsync(input);

            var pageResult = new PageList<SearchSuggestResponse>(input.Page, input.PageSize)
            {
                DataCount = result.Data.Count > 0 ? 20 : 0,
                Data = result.Data
            };
            return KdyResult.Success(pageResult);
        }

        /// <summary>
        /// 根据关键字自动创建豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("auto-create-by-keyword")]
        public async Task<KdyResult<CreateForSubjectIdDto>> CreateForKeyWordAsync(string keyWord, int year)
        {
            if (string.IsNullOrEmpty(keyWord) || year <= 1900)
            {
                return KdyResult.Error<CreateForSubjectIdDto>(KdyResultCode.ParError, "参数错误");
            }

            return await _douBanInfoService.CreateForKeyWordAsync(keyWord, year);
        }
    }
}
