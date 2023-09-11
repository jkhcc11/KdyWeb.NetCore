using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.CloudParse.SelfHost.Models;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParse.SelfHost.Controllers
{
    /// <summary>
    /// ApiJson格式
    /// </summary>
    [Route("api-v2")]
    [ApiController]
    public class CloudParseJsonController : BaseController
    {
        private readonly IDiskParseService _diskParseService;
        private readonly ISubAccountService _subAccountService;
        public CloudParseJsonController(IDiskParseService diskParseService,
            ISubAccountService subAccountService)
        {
            _diskParseService = diskParseService;
            _subAccountService = subAccountService;
        }

        /// <summary>
        /// 通用
        /// </summary>
        /// <returns></returns>
        [HttpGet("common/{userInfo}/{fileInfo}")]
        public async Task<JsonParseDto> CommonCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        {
            var newBusinessFlag = await _subAccountService.GetBusinessFlagByUserIdAsync(input.UserInfo
                , input.IsOldUserInfo == false);
            if (string.IsNullOrEmpty(newBusinessFlag))
            {
                return JsonParseDto.SetFail("未知用户信息");
            }

            var cachePrefix = $"{CacheKeyConst.BusinessFlagToDownCachePrefix(newBusinessFlag)}:";
            var isNeedSerCookie = CloudParseCookieType.IsNeedServerCookie(newBusinessFlag);
            KdyResult<CommonParseDto> parseResult;
            if (isNeedSerCookie)
            {
                parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
                    input.Token, newBusinessFlag,
                    input.UserInfo, input.FileInfo, false,
                    input.ParseModel == ParseModel.Name);
            }
            else
            {
                parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
                   input.Token, newBusinessFlag,
                   input.UserInfo, input.FileInfo, false,
                   input.ParseModel == ParseModel.Name);
            }


            return ToJsonParseDto(parseResult);
        }

        #region remove
        ///// <summary>
        ///// 胜天
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("st/{userInfo}/{fileInfo}")]
        //public async Task<JsonParseDto> StCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        //{
        //    var cachePrefix = $"{CacheKeyConst.StCacheKey.DownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
        //        input.Token, CloudParseCookieType.BitQiu,
        //        input.UserInfo, input.FileInfo, false,
        //        input.ParseModel == ParseModel.Name);
        //    return ToJsonParseDto(parseResult);
        //}

        ///// <summary>
        ///// 阿里
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("ali/{userInfo}/{fileInfo}")]
        //public async Task<JsonParseDto> AliCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        //{
        //    var cachePrefix = $"{CacheKeyConst.AliYunCacheKey.DownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
        //        input.Token, CloudParseCookieType.Ali,
        //        input.UserInfo, input.FileInfo, true,
        //        input.ParseModel == ParseModel.Name);
        //    return ToJsonParseDto(parseResult);
        //}

        ///// <summary>
        ///// 天翼云 个人
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("ty-person/{userInfo}/{fileInfo}")]
        //public async Task<JsonParseDto> TyPersonCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        //{
        //    var cachePrefix = $"{CacheKeyConst.TyCacheKey.DownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
        //        input.Token, CloudParseCookieType.TyPerson,
        //        input.UserInfo, input.FileInfo, false,
        //        input.ParseModel == ParseModel.Name);
        //    return ToJsonParseDto(parseResult);
        //}

        ///// <summary>
        ///// 天翼云 家庭
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("ty-family/{userInfo}/{fileInfo}")]
        //public async Task<JsonParseDto> TyFamilyCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        //{
        //    var cachePrefix = $"{CacheKeyConst.TyCacheKey.FamilyDownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
        //        input.Token, CloudParseCookieType.TyFamily,
        //        input.UserInfo, input.FileInfo, false,
        //        input.ParseModel == ParseModel.Name);
        //    return ToJsonParseDto(parseResult);
        //}

        /// <summary>
        /// 天翼云 企业
        /// </summary>
        /// <returns></returns>
        [HttpGet("ty-crop/{userInfo}/{fileInfo}")]
        [Obsolete("待移除")]
        public async Task<JsonParseDto> TyCropCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        {
            var cachePrefix = $"{CacheKeyConst.TyCacheKey.CropDownCacheKey}:";
            var parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
                input.Token, CloudParseCookieType.TyCrop,
                input.UserInfo, input.FileInfo, false,
                input.ParseModel == ParseModel.Name);
            return ToJsonParseDto(parseResult);
        }

        ///// <summary>
        ///// 盘139
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("hc/{userInfo}/{fileInfo}")]
        //public async Task<JsonParseDto> Pan139CloudParseAsync([FromRoute] CloudParsePlayerInput input)
        //{
        //    var cachePrefix = $"{CacheKeyConst.Pan139CacheKey.DownCacheKey}:";
        //    var parseResult = await _diskParseService.CommonParseAsync(cachePrefix,
        //        input.Token, CloudParseCookieType.Pan139,
        //        input.UserInfo, input.FileInfo, true,
        //        input.ParseModel == ParseModel.Name);
        //    return ToJsonParseDto(parseResult);
        //} 
        #endregion

        private JsonParseDto ToJsonParseDto(KdyResult<CommonParseDto> parseResult)
        {
            if (parseResult.IsSuccess == false)
            {
                return JsonParseDto.SetFail(parseResult.Msg);
            }

            return JsonParseDto.SetSuccess(parseResult.Data.DownLink);
        }
    }
}
