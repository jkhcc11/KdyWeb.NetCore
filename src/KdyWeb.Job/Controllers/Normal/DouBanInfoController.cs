using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 豆瓣信息
    /// </summary>
    public class DouBanInfoController : BaseNormalController
    {
        private readonly IDouBanInfoService _douBanInfoService;

        public DouBanInfoController(IDouBanInfoService douBanInfoService)
        {
            _douBanInfoService = douBanInfoService;
        }

        /// <summary>
        /// 获取最新Top信息
        /// </summary>
        /// <param name="top">最新几条</param>
        /// <returns></returns>
        [HttpGet("getTop/{top}")]
        [ProducesResponseType(typeof(KdyResult<List<GetTop50DouBanInfoDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTopDouBanInfoAsync(int top)
        {
            var result = await _douBanInfoService.GetTopDouBanInfoAsync(top);
            return Ok(result);
        }

        /// <summary>
        /// 根据关键字自动创建豆瓣信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("auto-create-by-keyword")]
        public async Task<KdyResult<dynamic>> CreateForKeyWordAsync(string keyWord, int year)
        {
            if (string.IsNullOrEmpty(keyWord) || year <= 1900)
            {
                return KdyResult.Error<dynamic>(KdyResultCode.ParError, "参数错误");
            }

            var result = await _douBanInfoService.CreateForKeyWordAsync(keyWord, year);
            if (result.IsSuccess == false)
            {
                return KdyResult.Error<dynamic>(KdyResultCode.Error, result.Msg);
            }

            //用户端搜索只返回这些
            var newResult = new
            {
                DbDouBanId = result.Data.Id,
                VodTitle = result.Data.VideoTitle,
                VodYear = result.Data.VideoYear,
                VodImg = result.Data.VideoImg,
                Subject = result.Data.VideoDetailId,
                DetailUrl = $"//movie.douban.com/subject/{result.Data.VideoDetailId}/"
            };
            return KdyResult.Success<dynamic>(newResult);
        }
    }
}
