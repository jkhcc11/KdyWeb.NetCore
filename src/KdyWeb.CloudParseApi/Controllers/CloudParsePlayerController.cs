using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 统一播放器
    /// </summary>
    [CustomRoute("preview")]
    public class CloudParsePlayerController : BaseApiController
    {
        private readonly IDiskParseService _diskParseService;
        private readonly ISubAccountService _subAccountService;
        public CloudParsePlayerController(IDiskParseService diskParseService,
            ISubAccountService subAccountService)
        {
            _diskParseService = diskParseService;
            _subAccountService = subAccountService;
        }

        /// <summary>
        /// 通用
        /// </summary>
        /// <returns></returns>
        [HttpGet("common")]
        public async Task<KdyResult<string>> CommonParseAsync(string filePath)
        {
            // /player-v2/common/subAccount/xxxxxxxx
            if (filePath.IsEmptyExt())
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "异常");
            }

            var tempArray = filePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            //var cloudType = tempArray[1];
            var cloudIdOrName = tempArray.Last();
            var subAccountInfo = tempArray[2];

            var newBusinessFlag = await _subAccountService.GetBusinessFlagByUserIdAsync(subAccountInfo, true);
            if (string.IsNullOrEmpty(newBusinessFlag))
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "未知用户信息");
            }

            var cachePrefix = $"{CacheKeyConst.BusinessFlagToDownCachePrefix(newBusinessFlag)}:";
            //var isNeedSerCookie = CloudParseCookieType.IsNeedServerCookie(newBusinessFlag);
            //播放器 不需要服务器cookie
            var parseResult = await _diskParseService.CommonParseAsync(cachePrefix, newBusinessFlag,
                subAccountInfo, cloudIdOrName, false, true);

            if (parseResult.IsSuccess)
            {
                return KdyResult.Success(parseResult.Data.DownLink, "success");
            }

            return KdyResult.Error<string>(parseResult.Code, parseResult.Msg);
        }
    }
}
