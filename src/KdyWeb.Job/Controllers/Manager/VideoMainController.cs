using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 影片相关
    /// </summary>
    public class VideoMainController : BaseManagerController
    {
        private readonly IVideoMainService _videoMainService;
        private readonly IVideoMainMatchInfoService _videoMainMatchInfoService;

        public VideoMainController(IVideoMainService videoMainService,
            IVideoMainMatchInfoService videoMainMatchInfoService)
        {
            _videoMainService = videoMainService;
            _videoMainMatchInfoService = videoMainMatchInfoService;
        }

        /// <summary>
        /// 更新字段值
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateField")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateValueByFieldAsync(UpdateValueByFieldInput input)
        {
            var result = await _videoMainService.UpdateValueByFieldAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 删除影片
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        [Authorize(Policy = AuthorizationConst.NormalPolicyName.SuperAdminPolicy)]
        public async Task<IActionResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            var result = await _videoMainService.DeleteAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 匹配豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("matchDouBanInfo")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> BindDouBanInfoAsync(MatchDouBanInfoInput input)
        {
            var result = await _videoMainService.BindDouBanInfoAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 修改影片详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("modify")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ModifyVideoMainAsync(ModifyVideoMainInput input)
        {
            var result = await _videoMainService.ModifyVideoMainAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 强制同步影片主表
        /// </summary>
        /// <returns></returns>
        [HttpGet("forceSync/{mainId}")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ForceSyncVideoMainAsync(long mainId)
        {
            var result = await _videoMainService.ForceSyncVideoMainAsync(mainId);
            return Ok(result);
        }

        /// <summary>
        /// 上|下架影片
        /// </summary>
        /// <returns></returns>
        [HttpPost("upAndDown/{mainId}")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpAndDownVodAsync(long mainId)
        {
            var result = await _videoMainService.UpAndDownVodAsync(mainId);
            return Ok(result);
        }

        /// <summary>
        /// 通过豆瓣信息创建影片信息(新版)
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-by-douban")]
        public async Task<KdyResult> CreateForDouBanInfoNewAsync(CreateForDouBanInfoNewInput input)
        {
            var result = await _videoMainService.CreateForDouBanInfoNewAsync(input);
            return result;
        }

        /// <summary>
        /// 通过豆瓣信息更新影片信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("update-by-douban")]
        public async Task<KdyResult> UpdateVodForDouBanInfoAsync(UpdateVodForDouBanInfoInput input)
        {
            var result = await _videoMainService.UpdateVodForDouBanInfoAsync(input);
            return result;
        }

        /// <summary>
        /// 自动匹配并保存剧集
        /// </summary>
        /// <returns></returns>
        [HttpPost("auto-match-save")]
        public async Task<KdyResult<string>> AutoMatchSaveEpAsync(AutoMatchSaveEpInput input)
        {
            var result = await _videoMainMatchInfoService.AutoMatchSaveEpAsync(input);
            return result;
        }

        ///// <summary>
        ///// 批量匹配豆瓣信息 前端根据访问发起
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("batch-match-douban")]
        //public async Task<KdyResult> BatchMatchDouBanInfoAsync(List<BatchMatchDouBanInfoInput> input)
        //{
        //    if (input == null ||
        //        input.Any() == false)
        //    {
        //        return KdyResult.Error(KdyResultCode.ParError, "参数不能为空");
        //    }

        //    foreach (var item in input)
        //    {
        //        var matchInput = new AutoMatchDouBanInfoJobInput()
        //        {
        //            MainId = item.KeyId,
        //            VodTitle = item.VodTitle,
        //            VodYear = item.VodYear
        //        };
        //        BackgroundJob.Enqueue<AutoMatchDouBanInfoJobService>(a => a.ExecuteAsync(matchInput));
        //    }

        //    await Task.CompletedTask;
        //    return KdyResult.Success($"匹配数量：{input.Count},任务已提交");
        //}
    }
}
