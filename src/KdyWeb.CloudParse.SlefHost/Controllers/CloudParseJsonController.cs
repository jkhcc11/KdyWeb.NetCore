using System.Linq;
using System.Threading.Tasks;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.SelfHost.Models;
using KdyWeb.Dto.CloudParse.CacheItem;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Service.CloudParse.DiskCloudParse;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.CloudParse.SelfHost.Controllers
{
    /// <summary>
    /// ApiJson格式
    /// </summary>
    [Route("api-v2")]
    [ApiController]
    public class CloudParseJsonController : BaseController
    {
        private readonly ISubAccountService _subAccountService;
        private readonly IServerCookieService _serverCookieService;
        private readonly IConfiguration _configuration;

        public CloudParseJsonController(ISubAccountService subAccountService,
            IServerCookieService serverCookieService, IConfiguration configuration)
        {
            _subAccountService = subAccountService;
            _serverCookieService = serverCookieService;
            _configuration = configuration;
        }

        /// <summary>
        /// 胜天
        /// </summary>
        /// <returns></returns>
        [HttpGet("st/{userInfo}/{fileInfo}")]
        public async Task<JsonParseDto> StCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            var tempStr = input.FileInfo.HexToStr();
            var tempArray = tempStr.Split('|');

            var fileId = tempArray[0];
            var parentId = tempArray[1];
            var fileName = string.Empty;
            var downUrlSearchType = DownUrlSearchType.FileId;
            if (input.ParseModel == ParseModel.Name)
            {
                fileName = tempArray[0];
                downUrlSearchType = DownUrlSearchType.Name;
            }

            var cacheKey = $"{CacheKeyConst.StCacheKey.DownCacheKey}:{input.UserInfo}:st:{userAgent.Md5Ext()}:{tempStr.Md5Ext()}";
            var reqInput = new BaseDownInput<StDownExtData>(cacheKey, fileId, downUrlSearchType)
            {
                FileName = fileName,
                ExtData = new StDownExtData()
                {
                    ParentId = parentId,
                    UserAgent = userAgent
                }
            };

            var subAccountId = await GetSubAccountIdAsync(input);
            if (subAccountId.HasValue == false)
            {
                return JsonParseDto.SetFail("账号异常,请联系管理员.code:-1");
            }

            var subAccount = await _subAccountService.GetSubAccountCacheAsync(subAccountId.Value);
            if (subAccount == null)
            {
                return JsonParseDto.SetFail("账号异常,请联系管理员.code:-2");
            }

            var cloudParseService =
                new StCloudParseService(new BaseConfigInput(subAccount.ShowName, subAccount.CookieInfo, subAccount.Id));
            var parseResult = await cloudParseService.GetDownUrlAsync(reqInput);
            if (parseResult.IsSuccess == false)
            {
                return JsonParseDto.SetFail(parseResult.Msg);
            }

            return JsonParseDto.SetSuccess(parseResult.Data);
        }

        /// <summary>
        /// 阿里
        /// </summary>
        /// <returns></returns>
        [HttpGet("ali/{userInfo}/{fileInfo}")]
        public async Task<JsonParseDto> AliCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        {
            var downInfo = await GetDownInfoByInputAsync(input, CacheKeyConst.AliYunCacheKey.DownCacheKey);
            if (downInfo.subAccountCacheItem == null)
            {
                return JsonParseDto.SetFail(downInfo.message);
            }

            var cloudConfig = new BaseConfigInput(downInfo.subAccountCacheItem.ShowName,
                downInfo.subAccountCacheItem.CookieInfo,
                downInfo.subAccountCacheItem.Id);

            var cloudParseService = new AliYunCloudParseService(cloudConfig);
            downInfo.reqInput.IsTs = true;
            var parseResult = await cloudParseService.GetDownUrlAsync(downInfo.reqInput);
            if (parseResult.IsSuccess == false)
            {
                return JsonParseDto.SetFail(parseResult.Msg);
            }

            return JsonParseDto.SetSuccess(parseResult.Data);
        }

        /// <summary>
        /// 天翼云 个人
        /// </summary>
        /// <returns></returns>
        [HttpGet("ty-person/{userInfo}/{fileInfo}")]
        public async Task<JsonParseDto> TyPersonCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        {
            var downInfo = await GetDownInfoByInputAsync(input, CacheKeyConst.TyCacheKey.DownCacheKey);
            if (downInfo.subAccountCacheItem == null)
            {
                return JsonParseDto.SetFail(downInfo.message);
            }

            var serverCookieCache =
                await _serverCookieService.GetServerCookieCacheAsync(GetServerIp(), downInfo.subAccountCacheItem.Id);
            if (serverCookieCache == null)
            {
                return JsonParseDto.SetFail("账号信息未配置");
            }

            var cloudConfig = new BaseConfigInput(input.UserInfo, serverCookieCache.CookieInfo,
                downInfo.subAccountCacheItem.Id);

            var cloudParseService = new TyPersonCloudParseService(cloudConfig);
            var parseResult = await cloudParseService.GetDownUrlAsync(downInfo.reqInput);
            if (parseResult.IsSuccess == false)
            {
                return JsonParseDto.SetFail(parseResult.Msg);
            }

            return JsonParseDto.SetSuccess(parseResult.Data);
        }

        /// <summary>
        /// 天翼云 企业
        /// </summary>
        /// <returns></returns>
        [HttpGet("ty-crop/{userInfo}/{fileInfo}")]
        public async Task<JsonParseDto> TyCropCloudParseAsync([FromRoute] CloudParsePlayerInput input)
        {
            var downInfo = await GetDownInfoByInputAsync(input, CacheKeyConst.TyCacheKey.DownCacheKey);
            if (downInfo.subAccountCacheItem == null)
            {
                return JsonParseDto.SetFail(downInfo.message);
            }

            var serverCookieCache =
                await _serverCookieService.GetServerCookieCacheAsync(GetServerIp(), downInfo.subAccountCacheItem.Id);
            if (serverCookieCache == null)
            {
                return JsonParseDto.SetFail("账号信息未配置");
            }

            var cloudConfig = new BaseConfigInput(input.UserInfo, serverCookieCache.CookieInfo,
                downInfo.subAccountCacheItem.Id);

            var cloudParseService = new TyCropCloudParseService(cloudConfig);
            var parseResult = await cloudParseService.GetDownUrlAsync(downInfo.reqInput);
            if (parseResult.IsSuccess == false)
            {
                return JsonParseDto.SetFail(parseResult.Msg);
            }

            return JsonParseDto.SetSuccess(parseResult.Data);
        }

        /// <summary>
        /// 盘139
        /// </summary>
        /// <returns></returns>
        [HttpGet("hc/{userInfo}/{fileInfo}")]
        public async Task<JsonParseDto> Pan139CloudParseAsync([FromRoute] CloudParsePlayerInput input)
        {
            var downInfo = await GetDownInfoByInputAsync(input, CacheKeyConst.Pan139CacheKey.DownCacheKey);
            if (downInfo.subAccountCacheItem == null)
            {
                return JsonParseDto.SetFail(downInfo.message);
            }

            var cloudConfig = new BaseConfigInput(downInfo.subAccountCacheItem.ShowName,
                downInfo.subAccountCacheItem.CookieInfo,
                downInfo.subAccountCacheItem.Id);

            var cloudParseService = new Pan139CloudParseService(cloudConfig);
            downInfo.reqInput.IsTs = true;
            var parseResult = await cloudParseService.GetDownUrlAsync(downInfo.reqInput);
            if (parseResult.IsSuccess == false)
            {
                return JsonParseDto.SetFail(parseResult.Msg);
            }

            return JsonParseDto.SetSuccess(parseResult.Data);
        }

        #region 私有

        /// <summary>
        /// 获取子账号ID
        /// </summary>
        /// <returns></returns>
        private async Task<long?> GetSubAccountIdAsync(CloudParsePlayerInput input)
        {
            long.TryParse(input.UserInfo, out long subAccountId);
            if (input.IsOldUserInfo == false)
            {
                return subAccountId;
            }

            var subAccountCache = await _subAccountService.GetSubAccountCacheAsync(input.UserInfo);
            if (subAccountCache == null)
            {
                return default;
            }

            subAccountId = subAccountCache.Id;
            return subAccountId;
        }

        /// <summary>
        /// 获取通用下载信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cachePrefix">下载缓存前缀</param>
        /// <returns></returns>
        private async Task<(CloudParseUserChildrenCacheItem subAccountCacheItem,
            BaseDownInput<string> reqInput,
            string downCacheKey,
            string message)>
            GetDownInfoByInputAsync(CloudParsePlayerInput input, string cachePrefix)
        {
            var tempStr = input.FileInfo.HexToStr();
            var tempArray = tempStr.Split('|');

            var fileId = tempArray.First();
            var fileName = string.Empty;
            var downUrlSearchType = DownUrlSearchType.FileId;
            if (input.ParseModel == ParseModel.Name)
            {
                fileName = tempArray.First();
                downUrlSearchType = DownUrlSearchType.Name;
            }

            var cacheKey = $"{cachePrefix}:{input.UserInfo}:{tempStr.Md5Ext()}";
            var reqInput = new BaseDownInput<string>(cacheKey, fileId, downUrlSearchType)
            {
                FileName = fileName,
            };
            if (tempArray.Count() == 2)
            {
                //多模式 xxx|extId（特殊Id）
                reqInput.ExtData = tempArray.Last();
            }

            var subAccountId = await GetSubAccountIdAsync(input);
            if (subAccountId.HasValue == false)
            {
                return (null, null, null, "账号异常,请联系管理员.code:-1");
            }

            var subAccount = await _subAccountService.GetSubAccountCacheAsync(subAccountId.Value);
            if (subAccount == null)
            {
                return (null, null, null, "账号异常,请联系管理员.code:-2");
            }

            //防盗校验
            var userInfo = await _subAccountService.GetUserInfoCacheAsync(subAccount.UserId);
            if (userInfo == null)
            {
                return (null, null, null, "无效账号,请联系管理员");
            }

            if (userInfo.IsHoldLink == false)
            {
                return (subAccount, reqInput, cacheKey, "");
            }

            #region 来源Token校验
            if (userInfo.ApiToken != input.Token)
            {
                return (null, null, null, "无效Token");
            }

            return (subAccount, reqInput, cacheKey, "");
            #endregion
        }

        /// <summary>
        /// 获取服务器Ip
        /// </summary>
        /// <returns></returns>
        private string GetServerIp()
        {
            return _configuration.GetValue<string>("CloudParseConfig:ServerIp");
        } 
        #endregion
    }
}
