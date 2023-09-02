using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
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
    /// 统一播放器
    /// </summary>
    [Route("player-v2")]
    public class CloudParsePlayerController : BaseController
    {
        private readonly ISubAccountService _subAccountService;
        private readonly IServerCookieService _serverCookieService;
        private readonly IConfiguration _configuration;

        public CloudParsePlayerController(ISubAccountService subAccountService,
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
        public async Task<ActionResult> StCloudParseAsync(CloudParsePlayerInput input)
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
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "账号异常,请联系管理员.code:-1");
                return View("Index");
            }

            var subAccount = await _subAccountService.GetSubAccountCacheAsync(subAccountId.Value);
            if (subAccount == null)
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "账号异常,请联系管理员.code:-2");
                return View("Index");
            }

            var cloudParseService =
                new StCloudParseService(new BaseConfigInput(subAccount.ShowName, subAccount.CookieInfo, subAccount.Id));
            var parseResult = await cloudParseService.GetDownUrlAsync(reqInput);
            parseResult.Data = parseResult.Data.ToStrConfuse(7);

            ViewBag.ParseResult = parseResult;
            return View("Index");
        }

        /// <summary>
        /// 阿里
        /// </summary>
        /// <returns></returns>
        [HttpGet("ali/{userInfo}/{fileInfo}")]
        public async Task<ActionResult> AliCloudParseAsync(CloudParsePlayerInput input)
        {
            var downInfo = await GetDownInfoByInputAsync(input, CacheKeyConst.AliYunCacheKey.DownCacheKey);
            if (downInfo.subAccountCacheItem == null)
            {
                return View("Index");
            }

            var cloudConfig = new BaseConfigInput(downInfo.subAccountCacheItem.ShowName,
                downInfo.subAccountCacheItem.CookieInfo,
                downInfo.subAccountCacheItem.Id);

            var cloudParseService = new AliYunCloudParseService(cloudConfig);
            downInfo.reqInput.IsTs = true;
            var parseResult = await cloudParseService.GetDownUrlAsync(downInfo.reqInput);
            parseResult.Data = parseResult.Data.ToStrConfuse(7);

            ViewBag.ParseResult = parseResult;
            return View("Index");
        }

        /// <summary>
        /// 天翼云 个人
        /// </summary>
        /// <returns></returns>
        [HttpGet("ty-person/{userInfo}/{fileInfo}")]
        public async Task<ActionResult> TyPersonCloudParseAsync(CloudParsePlayerInput input)
        {
            var downInfo = await GetDownInfoByInputAsync(input, CacheKeyConst.TyCacheKey.DownCacheKey);
            if (downInfo.subAccountCacheItem == null)
            {
                return View("Index");
            }

            var serverCookieCache =
                await _serverCookieService.GetServerCookieCacheAsync(GetServerIp(), downInfo.subAccountCacheItem.Id);
            if (serverCookieCache == null)
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "账号信息未配置");
                return View("Index");
            }

            var cloudConfig = new BaseConfigInput(input.UserInfo, serverCookieCache.CookieInfo,
                downInfo.subAccountCacheItem.Id);

            var cloudParseService = new TyPersonCloudParseService(cloudConfig);
            var parseResult = await cloudParseService.GetDownUrlAsync(downInfo.reqInput);
            parseResult.Data = parseResult.Data.ToStrConfuse(7);
            ViewBag.ParseResult = parseResult;
            return View("Index");
        }

        /// <summary>
        /// 天翼云 企业
        /// </summary>
        /// <returns></returns>
        [HttpGet("ty-crop/{userInfo}/{fileInfo}")]
        public async Task<ActionResult> TyCropCloudParseAsync(CloudParsePlayerInput input)
        {

            var downInfo = await GetDownInfoByInputAsync(input, CacheKeyConst.TyCacheKey.DownCacheKey);
            if (downInfo.subAccountCacheItem == null)
            {
                return View("Index");
            }

            var serverCookieCache =
                await _serverCookieService.GetServerCookieCacheAsync(GetServerIp(), downInfo.subAccountCacheItem.Id);
            if (serverCookieCache == null)
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "账号信息未配置");
                return View("Index");
            }

            var cloudConfig = new BaseConfigInput(input.UserInfo, serverCookieCache.CookieInfo,
                downInfo.subAccountCacheItem.Id);

            var cloudParseService = new TyCropCloudParseService(cloudConfig);
            var parseResult = await cloudParseService.GetDownUrlAsync(downInfo.reqInput);
            parseResult.Data = parseResult.Data.ToStrConfuse(7);
            ViewBag.ParseResult = parseResult;
            return View("Index");
        }

        /// <summary>
        /// 盘139
        /// </summary>
        /// <returns></returns>
        [HttpGet("hc/{userInfo}/{fileInfo}")]
        public async Task<ActionResult> Pan139CloudParseAsync(CloudParsePlayerInput input)
        {
            var downInfo = await GetDownInfoByInputAsync(input, CacheKeyConst.Pan139CacheKey.DownCacheKey);
            if (downInfo.subAccountCacheItem == null)
            {
                return View("Index");
            }

            var cloudConfig = new BaseConfigInput(downInfo.subAccountCacheItem.ShowName,
                downInfo.subAccountCacheItem.CookieInfo,
                downInfo.subAccountCacheItem.Id);

            var cloudParseService = new Pan139CloudParseService(cloudConfig);
            downInfo.reqInput.IsTs = true;
            var parseResult = await cloudParseService.GetDownUrlAsync(downInfo.reqInput);
            parseResult.Data = parseResult.Data.ToStrConfuse(7);

            ViewBag.ParseResult = parseResult;
            return View("Index");
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
            BaseDownInput<string> reqInput, string downCacheKey)>
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
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "账号异常,请联系管理员.code:-1");
                return (null, null, null);
            }

            var subAccount = await _subAccountService.GetSubAccountCacheAsync(subAccountId.Value);
            if (subAccount == null)
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "账号异常,请联系管理员.code:-2");
                return (null, null, null);
            }

            if (reqInput.ExtData.IsEmptyExt() &&
                subAccount.BusinessId.IsEmptyExt() == false)
            {
                //跨盘解析 丢失的问题
                //todo：个人没有分组Id，同子账号更换为一个有分组Id时，不换外链，直接填写
                reqInput.ExtData = subAccount.BusinessId;
            }

            //防盗校验
            var userInfo = await _subAccountService.GetUserInfoCacheAsync(subAccount.UserId);
            if (userInfo == null)
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "无效账号,请联系管理员");
                return (null, null, null);
            }

            if (userInfo.IsHoldLink == false)
            {
                return (subAccount, reqInput, cacheKey);
            }

            #region 来源检测
            var referrer = HttpContext.Request.Headers["Referer"] + "";
            if (string.IsNullOrEmpty(referrer))
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "来源未知,请联系管理员");
                return (null, null, null);
            }

            var url = new Uri(referrer);
            if (userInfo.HoldLinkHost.Any(a => a == url.Host) == false)
            {
                ViewBag.ParseResult = KdyResult.Error(KdyResultCode.Error, "来源异常,请联系管理员");
                return (null, null, null);
            }

            return (subAccount, reqInput, cacheKey);
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
