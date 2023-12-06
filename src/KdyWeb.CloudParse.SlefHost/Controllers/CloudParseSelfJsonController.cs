using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Filter;
using KdyWeb.CloudParse.SelfHost.Models;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Service.CloudParse;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KdyWeb.CloudParse.SelfHost.Controllers
{
    /// <summary>
    /// SelfApiJson格式
    /// </summary>
    [Route("self-api-v2")]
    [ApiController]
    [AddPowerByHeaderFilter]
    public class CloudParseSelfJsonController : BaseController
    {
        private readonly IDiskParseService _diskParseService;
        private readonly IConfiguration _configuration;
        private readonly ISubAccountService _subAccountService;
        private readonly ILogger<CloudParseSelfJsonController> _logger;
        public CloudParseSelfJsonController(IDiskParseService diskParseService,
            IConfiguration configuration, ISubAccountService subAccountService, 
            ILogger<CloudParseSelfJsonController> logger)
        {
            _diskParseService = diskParseService;
            _configuration = configuration;
            _subAccountService = subAccountService;
            _logger = logger;
        }

        /// <summary>
        /// 自用站解析
        /// </summary>
        /// <returns></returns>
        [HttpPost("parse")]
        public async Task<JsonParseDto> SelfParseAsync([FromForm]SelfParseInput input)
        {
            var desKey = _configuration.GetValue<string>(KdyWebServiceConst.DesKey, "hcc11com");
            var decodeUrl = input.EncodeUrl.DesHexToStr(desKey);
            var tempArray = decodeUrl.Split('/', StringSplitOptions.RemoveEmptyEntries);
            //旧版6个   新版4个
            // /Cloud/Down/Index/tytest_11/5134712334405615/2
            // /Cloud/Down/AliCloud/tytest_698/32303139E788B1E5B094E585B0E4BABA2E5468652E49726973686D616E2E6D7034/6
            // /api-v2/common/1698015082103050240/6465736B746F702E696E697C3333333237313236
            switch (tempArray.Length)
            {
                case 6:
                    {
                        var cloudType = tempArray[2]; //天翼个人、阿里、天翼家庭
                        var cloudUser = tempArray[3]; //用户昵称 xx_xx
                        var cloudIdOrName = tempArray[4]; //id或name hex
                        var cloudModel = tempArray.Last(); // 2 id 6|7 name 

                        return await SelfParseOldAsync(cloudType, cloudUser, cloudIdOrName, cloudModel);
                    }
                case 4:
                    {
                        // var cloudType = tempArray[1]; //common
                        var cloudUserId = tempArray[2]; //用户昵称 xx_xx
                        var cloudIdOrName = tempArray[3]; //name hex
                        return await SelfParseNewAsync(cloudUserId, cloudIdOrName, true, true);
                    }
            }

            return JsonParseDto.SetFail("无效Url");
        }

        /// <summary>
        /// 旧解析
        /// </summary>
        /// <param name="cloudType">盘类</param>
        /// <param name="cloudUser">用户 xxx_111</param>
        /// <param name="cloudIdOrName">id或name</param>
        /// <param name="cloudModel">2 id  6 name</param>
        /// <returns></returns>
        private async Task<JsonParseDto> SelfParseOldAsync(string cloudType, string cloudUser,
            string cloudIdOrName, string cloudModel)
        {
            _logger.LogInformation("OldParse,{cloudType},{cloudUser},{cloudIdOrName},{cloudModel}",
                cloudType,
                cloudUser,
                cloudIdOrName,
                cloudModel);
            var businessFlag = CacheKeyConst.ToBusinessFlag(cloudType);
            var cachePrefix = $"{DiskCloudParseFactory.BusinessFlagToDownCachePrefix(businessFlag)}:";
            KdyResult<CommonParseDto> parseResult;
            var isName = cloudModel != "2";
            if (CloudParseCookieType.IsNeedServerCookie(businessFlag))
            {
                parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix, businessFlag,
                    cloudUser, cloudIdOrName, false, isName);
            }
            else
            {
                parseResult = await _diskParseService.CommonParseAsync(cachePrefix, businessFlag,
                    cloudUser, cloudIdOrName, false, isName);
            }

            if (parseResult.IsSuccess)
            {
                return JsonParseDto.SetSuccess(parseResult.Data
                    .DownLink.ToStrConfuse(7));
            }

            return JsonParseDto.SetFail(parseResult.Msg);
        }

        /// <summary>
        /// 新解析
        /// </summary>
        /// <param name="cloudUserId">子账号Id</param>
        /// <param name="cloudIdOrName">id或name</param>
        /// <returns></returns>
        private async Task<JsonParseDto> SelfParseNewAsync(string cloudUserId, string cloudIdOrName,
            bool isTs, bool isName)
        {
            var isOldUserInfo = cloudUserId.Contains("_");
            var businessFlag = await _subAccountService.GetBusinessFlagByUserIdAsync(cloudUserId
                , isOldUserInfo == false);
            if (string.IsNullOrEmpty(businessFlag))
            {
                return JsonParseDto.SetFail("未知用户信息");
            }

            var cachePrefix = $"{DiskCloudParseFactory.BusinessFlagToDownCachePrefix(businessFlag)}:";
            KdyResult<CommonParseDto> parseResult;
            if (CloudParseCookieType.IsNeedServerCookie(businessFlag))
            {
                parseResult = await _diskParseService.CommonParseWithServerCookieAsync(cachePrefix,
                    businessFlag, cloudUserId, cloudIdOrName, isTs, isName);
            }
            else
            {
                parseResult = await _diskParseService.CommonParseAsync(cachePrefix, businessFlag,
                    cloudUserId, cloudIdOrName, isTs, isName);
            }

            if (parseResult.IsSuccess)
            {
                return JsonParseDto.SetSuccess(parseResult.Data
                    .DownLink.ToStrConfuse(7));
            }

            return JsonParseDto.SetFail(parseResult.Msg);
        }
    }
}
