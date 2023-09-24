using System.Linq;
using KdyWeb.BaseInterface.Service;
using KdyWeb.IService.CloudParse;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.CloudParse.Input;
using KdyWeb.Dto.CloudParse.CacheItem;
using Microsoft.Extensions.Configuration;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Utility;
using System;
using System.Collections.Generic;
using KdyWeb.BaseInterface;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using Microsoft.AspNetCore.Http;
using static KdyWeb.IService.CloudParse.CacheKeyConst;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 解析 服务实现
    /// </summary>
    public class DiskParseService : BaseKdyService, IDiskParseService
    {
        private readonly ISubAccountService _subAccountService;
        private readonly IServerCookieService _serverCookieService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKdyRepository<ParseRecordHistory, long> _parseRecordHistoryRepository;

        public DiskParseService(IUnitOfWork unitOfWork, ISubAccountService subAccountService,
            IServerCookieService serverCookieService, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IKdyRepository<ParseRecordHistory, long> parseRecordHistoryRepository) : base(unitOfWork)
        {
            _subAccountService = subAccountService;
            _serverCookieService = serverCookieService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _parseRecordHistoryRepository = parseRecordHistoryRepository;
        }

        /// <summary>
        /// 通用解析
        /// </summary>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/></param>
        /// <param name="userInfo">用户信息  新版 subAccountId  旧版 xxx_id</param>
        /// <param name="fileInfo">文件信息 文件Id或文件名</param>
        /// <param name="isTs">是否切片（如果支持）</param>
        /// <param name="isName">文件信息是否为名称</param>
        /// <returns></returns>
        public async Task<KdyResult<CommonParseDto>> CommonParseAsync(string cachePrefix, string cloudParseType,
            object userInfo, string fileInfo, bool isTs, bool isName)
        {
            var subAccountCache = await GetSubAccountInfoAsync(userInfo);
            var reqInput = BuildReqInfo(fileInfo, isName, cachePrefix, subAccountCache);
            var downUrlResult = await GetDownUrlAsync(cloudParseType, isTs, subAccountCache, reqInput);
            if (downUrlResult.IsSuccess == false)
            {
                return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, downUrlResult.Msg);
            }

            return KdyResult.Success(new CommonParseDto(downUrlResult.Data));
        }

        /// <summary>
        /// 通用解析(校验信息)
        /// </summary>
        /// <param name="verifyInfo">来源ref 或者 api token</param>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/></param>
        /// <param name="userInfo">用户信息  新版 subAccountId  旧版 xxx_id</param>
        /// <param name="fileInfo">文件信息 文件Id或文件名</param>
        /// <param name="isTs">是否切片（如果支持）</param>
        /// <param name="isName">文件信息是否为名称</param>
        /// <returns></returns>
        public async Task<KdyResult<CommonParseDto>> CommonParseAsync(string cachePrefix, string verifyInfo,
            string cloudParseType, object userInfo, string fileInfo, bool isTs, bool isName)
        {
            var subAccountCache = await GetSubAccountInfoAsync(userInfo);
            var userInfoCache = await GetUserInfoCacheAsync(subAccountCache.UserId);
            if (userInfoCache.IsHoldLink &&
                LoginUserInfo.IsLogin == false)
            {
                #region 未登录防盗校验

                if (string.IsNullOrEmpty(verifyInfo))
                {
                    return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "来源无效,请联系管理员");
                }

                if (verifyInfo.StartsWith("http"))
                {
                    var url = new Uri(verifyInfo);
                    if (userInfoCache.HoldLinkHost.Any(a => a == url.Host) == false)
                    {
                        return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "来源异常,请联系管理员");
                    }
                }
                else if (userInfoCache.ApiToken != verifyInfo)
                {
                    return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "无效Token");
                }
                #endregion
            }

            if (userInfoCache.ExpirationDateTime.HasValue &&
                userInfoCache.ExpirationDateTime.Value.AddDays(CloudParseUser.OverDays) < DateTime.Now)
            {
                //超过7天 g
                return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "已过期,停止使用");
            }

            var reqInput = BuildReqInfo(fileInfo, isName, cachePrefix, subAccountCache);
            var downUrlResult = await GetDownUrlAsync(cloudParseType, isTs, subAccountCache, reqInput);
            if (downUrlResult.IsSuccess == false)
            {
                return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, downUrlResult.Msg);
            }

            return KdyResult.Success(new CommonParseDto(downUrlResult.Data));
        }

        /// <summary>
        /// 通用解析 服务器Cookie
        /// </summary>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/></param>
        /// <param name="userInfo">用户信息  新版 subAccountId  旧版 xxx_id</param>
        /// <param name="fileInfo">文件信息 文件Id或文件名</param>
        /// <param name="isTs">是否切片（如果支持）</param>
        /// <param name="isName">文件信息是否为名称</param>
        /// <returns></returns>
        public async Task<KdyResult<CommonParseDto>> CommonParseWithServerCookieAsync(string cachePrefix, string cloudParseType,
            object userInfo, string fileInfo,
            bool isTs, bool isName)
        {
            var subAccountCache = await GetSubAccountInfoAsync(userInfo);
            var serverCookieCache = await GetServerCookieCacheAsync(subAccountCache.Id);
            if (serverCookieCache == null)
            {
                return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "账号信息未配置");
            }

            //这里需要替换为服务器cookie
            subAccountCache.CookieInfo = serverCookieCache.CookieInfo;
            var reqInput = BuildReqInfo(fileInfo, isName, cachePrefix, subAccountCache);
            var downUrlResult = await GetDownUrlAsync(cloudParseType, isTs, subAccountCache, reqInput);
            if (downUrlResult.IsSuccess == false)
            {
                return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, downUrlResult.Msg);
            }

            return KdyResult.Success(new CommonParseDto(downUrlResult.Data));
        }

        /// <summary>
        /// 通用解析 服务器Cookie  (校验信息)
        /// </summary>
        /// <param name="verifyInfo">来源ref 或者 api token</param>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/></param>
        /// <param name="userInfo">用户信息  新版 subAccountId  旧版 xxx_id</param>
        /// <param name="fileInfo">文件信息 文件Id或文件名</param>
        /// <param name="isTs">是否切片（如果支持）</param>
        /// <param name="isName">文件信息是否为名称</param>
        /// <returns></returns>
        public async Task<KdyResult<CommonParseDto>> CommonParseWithServerCookieAsync(string cachePrefix, string verifyInfo,
            string cloudParseType, object userInfo, string fileInfo,
           bool isTs, bool isName)
        {
            var subAccountCache = await GetSubAccountInfoAsync(userInfo);
            var userInfoCache = await GetUserInfoCacheAsync(subAccountCache.UserId);
            if (userInfoCache.IsHoldLink &&
                LoginUserInfo.IsLogin == false)
            {
                #region 未登录防盗校验

                if (verifyInfo.IsEmptyExt())
                {
                    return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "未知来源");
                }

                if (verifyInfo.StartsWith("http"))
                {
                    var url = new Uri(verifyInfo);
                    if (userInfoCache.HoldLinkHost.Any(a => a == url.Host) == false)
                    {
                        return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "来源异常,请联系管理员");
                    }
                }
                else if (userInfoCache.ApiToken != verifyInfo)
                {
                    return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "无效Token");
                }
                #endregion
            }

            if (userInfoCache.ExpirationDateTime.HasValue &&
                userInfoCache.ExpirationDateTime.Value.AddDays(CloudParseUser.OverDays) < DateTime.Now)
            {
                //超过7天 g
                return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, "已过期,停止使用");
            }

            var serverCookieCache = await GetServerCookieCacheAsync(subAccountCache.Id);
            //这里需要替换为服务器cookie
            subAccountCache.CookieInfo = serverCookieCache.CookieInfo;

            var reqInput = BuildReqInfo(fileInfo, isName, cachePrefix, subAccountCache);
            var downUrlResult = await GetDownUrlAsync(cloudParseType, isTs, subAccountCache, reqInput);
            if (downUrlResult.IsSuccess == false)
            {
                return KdyResult.Error<CommonParseDto>(KdyResultCode.Error, downUrlResult.Msg);
            }

            return KdyResult.Success(new CommonParseDto(downUrlResult.Data));
        }

        #region 私有
        /// <summary>
        /// 获取子账号缓存
        /// </summary>
        /// <returns></returns>
        private async Task<CloudParseUserChildrenCacheItem> GetSubAccountInfoAsync(object subAccountInfo)
        {
            CloudParseUserChildrenCacheItem subAccountCacheItem;
            if (long.TryParse(subAccountInfo + "", out long subAccountId))
            {
                subAccountCacheItem = await _subAccountService.GetSubAccountCacheAsync(subAccountId);
            }
            else
            {
                subAccountCacheItem = await _subAccountService.GetSubAccountCacheAsync(subAccountInfo + "");
            }

            if (subAccountCacheItem == null)
            {
                throw new KdyCustomException("账号异常,请联系管理员.code:-2");
            }

            return subAccountCacheItem;
        }

        /// <summary>
        /// 获取用户信息缓存
        /// </summary>
        /// <returns></returns>
        private async Task<GetParseUserInfoDto> GetUserInfoCacheAsync(long userId)
        {
            var userInfoCache = await _subAccountService.GetUserInfoCacheAsync(userId);
            if (userInfoCache == null)
            {
                throw new KdyCustomException("账号异常,请联系管理员.code:-1");
            }

            return userInfoCache;
        }

        /// <summary>
        /// 获取服务器Cookie
        /// </summary>
        /// <returns></returns>
        private async Task<QueryServerCookieDto> GetServerCookieCacheAsync(long subAccountId)
        {
            var userInfoCache = await _serverCookieService.GetServerCookieCacheAsync(GetServerIp(), subAccountId);
            if (userInfoCache == null)
            {
                throw new KdyCustomException("账号信息未配置");
            }

            return userInfoCache;
        }

        /// <summary>
        /// 生成DownInput
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="isName">是否为名称</param>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="subAccountCache">子账号信息</param>
        /// <returns></returns>
        private BaseDownInput<string> BuildReqInfo(string fileInfo, bool isName,
            string cachePrefix, CloudParseUserChildrenCacheItem subAccountCache)
        {
            //文件信息
            //todo:兼容旧版 如果旧版tyPerson没有
            string fileId;
            var fileName = string.Empty;
            var tempStr = string.Empty;
            string[] tempArray = { tempStr };
            var downUrlSearchType = DownUrlSearchType.FileId;
            if (long.TryParse(fileInfo, out long tyFileId))
            {
                fileId = tyFileId + "";
                tempStr = fileInfo;
            }
            else
            {
                var tryDesJie = fileInfo.DesHexToStr(KdyCloudParseConst.TyCorpDesKey);
                if (tryDesJie == fileInfo)
                {
                    //todo:des解析失败(旧版兼容)，使用新版
                    tempStr = fileInfo.HexToStr();
                    tempArray = tempStr.Split('|');
                }
                else
                {
                    //todo:旧版 businessId,fileId
                    tempStr = tryDesJie;
                    var oldTempArray = tryDesJie.Split(',');
                    tempArray = new[] { oldTempArray[1], oldTempArray[0] };
                }

                fileId = tempArray.First();
            }

            if (isName)
            {
                fileName = tempArray.First();
                downUrlSearchType = DownUrlSearchType.Name;
            }

            var cacheKey = $"{cachePrefix}:{subAccountCache.Id}:{tempStr.Md5Ext()}";
            var reqInput = new BaseDownInput<string>(cacheKey, fileId, downUrlSearchType)
            {
                FileName = fileName,
            };
            if (tempArray.Count() == 2)
            {
                //多模式 xxx|extId（特殊Id）
                reqInput.ExtData = tempArray.Last();
            }

            if (subAccountCache.BusinessId.IsEmptyExt() == false)
            {
                //跨盘解析 配置优先
                //todo：个人没有分组Id，同子账号更换为一个有分组Id时，不换外链，直接填写
                reqInput.ExtData = subAccountCache.BusinessId;
            }

            return reqInput;
        }

        /// <summary>
        /// 获取下载地址
        /// </summary>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/> </param>
        /// <param name="isTs">是否切片</param>
        /// <param name="subAccountCacheItem">子账号缓存</param>
        /// <param name="downReqInput">DownInput</param>
        /// <returns></returns>
        private async Task<KdyResult<string>> GetDownUrlAsync(string cloudParseType, bool isTs,
            CloudParseUserChildrenCacheItem subAccountCacheItem,
            BaseDownInput<string> downReqInput)
        {
            var cloudConfig = new BaseConfigInput(subAccountCacheItem.ShowName,
                subAccountCacheItem.CookieInfo, subAccountCacheItem.Id);

            downReqInput.IsTs = isTs;
            var cloudParseService = DiskCloudParseFactory.CreateKdyCloudParseService(cloudParseType, cloudConfig);

            KdyResult<string> downResult = null;
            //特殊下载参数
            switch (cloudParseType)
            {
                case CloudParseCookieType.Ali:
                    {
                        //强制切片
                        downReqInput.IsTs = true;
                        break;
                    }
                case CloudParseCookieType.BitQiu:
                    {
                        #region st
                        var httpRequest = _httpContextAccessor.HttpContext.Request;
                        var userAgent = httpRequest.Headers["User-Agent"].ToString();

                        var tempCacheKey = $"{downReqInput.CacheKey}:{userAgent.Md5Ext()}";
                        var tempDownReqInput = new BaseDownInput<StDownExtData>(tempCacheKey,
                            downReqInput.FileId, downReqInput.DownUrlSearchType)
                        {
                            FileName = downReqInput.FileName,
                            ExtData = new StDownExtData()
                            {
                                UserAgent = userAgent
                            }
                        };


                        #endregion

                        downResult = await cloudParseService.GetDownUrlAsync(tempDownReqInput);
                        break;
                    }
            }

            //为空则通用请求
            downResult ??= await cloudParseService.GetDownUrlAsync(downReqInput);

            #region 历史记录
            ParseRecordHistory historyEntity;
            if (downResult.Msg == CloudParseUser.CacheMsg)
            {
                //缓存
                historyEntity = new ParseRecordHistory(RecordHistoryType.Visit,
                    subAccountCacheItem.UserId,
                    subAccountCacheItem.Id);
            }
            else
            {
                historyEntity = new ParseRecordHistory(RecordHistoryType.Request,
                    subAccountCacheItem.UserId,
                    subAccountCacheItem.Id);
            }

            historyEntity.Alias = subAccountCacheItem.Alias;
            historyEntity.FileIdOrFileName =
                string.IsNullOrEmpty(downReqInput.FileName) ?
                    downReqInput.FileId : downReqInput.FileName;

            await _parseRecordHistoryRepository.CreateAsync(historyEntity);
            await UnitOfWork.SaveChangesAsync();
            #endregion

            return downResult;
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
