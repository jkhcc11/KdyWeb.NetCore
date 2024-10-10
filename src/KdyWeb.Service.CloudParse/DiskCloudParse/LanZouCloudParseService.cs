using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.CloudParse.CloudParseEnum;
using KdyWeb.CloudParse.Extensions;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.CloudParse.DiskCloudParse;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 蓝奏优享 实现
    /// </summary>
    [CloudParseService(CloudParseCookieType.LanZouYouXiang, DownCachePrefix = CacheKeyConst.LanZouYouXiangCacheKey.DownCacheKey)]
    public class LanZouCloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut>,
        ILanZouCloudParseService
    {
        private const string DefaultRootId = "0";
        private const string AesKey = "lanZouY-disk-app";

        /// <summary>
        /// 仅用于清除缓存
        /// </summary>
        /// <param name="childUserId">子账号Id</param>
        public LanZouCloudParseService(long childUserId)
        {
            CloudConfig = new BaseConfigInput(childUserId);
        }

        public LanZouCloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://api.ilanzou.com")
            {
                TimeOut = 5000,
                Referer = "https://www.ilanzou.com/",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36 Edg/129.0.0.0",
            };
        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var result = new List<BaseResultOut>();
            //搜索结果用这个
            var dirArray = jObject["list"] as JArray;
            if (dirArray == null || dirArray.Any() == false)
            {
                return result;
            }

            foreach (JToken jToken in dirArray)
            {
                var fileName = jToken["fileName"];
                var fileType = CloudFileType.Dir;
                if (fileName == null)
                {
                    fileName = jToken["folderName"];
                }
                else
                {
                    fileType = fileName.ToString().FileNameToFileType();
                }

                if (fileName == null)
                {
                    continue;
                }

                long.TryParse($"{jToken["fileSize"]}", out var size);
                var model = new BaseResultOut
                {
                    //文件夹路径
                    ParentId = $"{jToken["folderId"]}",
                    //文件id 或者文件夹id
                    ResultId = $"{jToken["fileId"] ?? jToken["folderId"]}",
                    //名字
                    ResultName = fileName.ToString(),
                    FileSize = size * 1024,
                    FileType = fileType
                };

                result.Add(model);
            }

            return result;
        }

        public override async Task<KdyResult<List<BaseResultOut>>> QueryFileAsync(BaseQueryInput<string> input)
        {
            var resultList = new List<BaseResultOut>();
            if (CloudConfig.ParseCookie.IsEmptyExt())
            {
                return KdyResult.Success(resultList);
            }

            var userInfo = await GetUserInfoAsync();
            if (input.InputId.IsEmptyExt())
            {
                input.InputId = DefaultRootId;
            }

            if (input.Page <= 0)
            {
                input.Page = 1;
            }

            if (input.PageSize <= 0)
            {
                input.PageSize = 60;
            }

            var timestamp = DateTime.Now.ToMillisecondTimestamp()
                .ToString()
                .ToAesHexExt(AesKey)
                .ToLower();
            var reqUrl = $"/proved/record/file/list?uuid={userInfo.DevCode}&devType=6&devCode={userInfo.DevCode}&devModel=chrome&devVersion=129&appVersion=&timestamp={timestamp}&appToken={userInfo.AppToken}&extra=2&offset={input.Page}&limit={input.PageSize}&folderId={input.InputId}&type=0";
            if (input.KeyWord.IsEmptyExt() == false)
            {
                reqUrl = $"/proved/record/search/list?uuid={userInfo.DevCode}&devType=6&devCode={userInfo.DevCode}&devModel=chrome&devVersion=129&appVersion=&timestamp={timestamp}&appToken={userInfo.AppToken}&extra=2&offset=1&limit=60&search={input.KeyWord}&folderId=0&type=0";
            }

            KdyRequestCommonInput.SetGetRequest(reqUrl);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick},蓝奏优享搜索文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            //{"code":"10500","message":"server error","data":null}
            var jObject = JObject.Parse(reqResult.Data);
            if (jObject.GetValue("code")?.Value<int>() != 200)
            {
                KdyLog.LogWarning("{userNick},蓝奏优享搜索文件异常,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
                return KdyResult.Success(resultList);
            }

            var result = JArrayHandler(jObject);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 根据关键字获取文件信息
        /// </summary>
        /// <returns></returns>
        public override async Task<KdyResult<BaseResultOut>> GetFileInfoByKeyWordAsync(string keyWord)
        {
            var nameCacheKey = GetCacheKeyWithFileName();
            var nameDb = GetNameCacheDb();

            var fileNameMd5 = keyWord.Md5Ext();
            var fileNameInfo = await nameDb.GetHashSetAsync<BaseResultOut>(nameCacheKey, fileNameMd5);
            if (fileNameInfo != null)
            {
                return KdyResult.Success(fileNameInfo);
            }

            var searchList = await QueryFileAsync(new BaseQueryInput<string>()
            {
                KeyWord = keyWord,
            });

            var fileInfo = searchList.Data.FirstOrDefault(a => a.ResultName == keyWord);
            if (fileInfo == null)
            {
                KdyLog.LogWarning("{userNick}文件名:{fileName} 蓝奏优享未搜索到请留意", CloudConfig.ReqUserInfo, keyWord);
                return KdyResult.Error<BaseResultOut>(KdyResultCode.Error, $"未找到文件名：{keyWord}");
            }

            await nameDb.SetHashSetAsync(nameCacheKey, fileNameMd5, fileInfo);
            return KdyResult.Success(fileInfo);
        }

        public override async Task<KdyResult<string>> GetDownUrlForNoCacheAsync<TDownEntity>(BaseDownInput<TDownEntity> input)
        {
            var currentFlag = HttpContextAccessor.HttpContext?.TraceIdentifier;
            var fileId = input.FileId;
            if (input.DownUrlSearchType == DownUrlSearchType.Name)
            {
                //根据文件名获取文件Id
                var fileInfo = await GetFileInfoByKeyWordAsync(input.FileName);
                if (fileInfo.IsSuccess == false)
                {
                    return KdyResult.Error<string>(fileInfo.Code, fileInfo.Msg);
                }

                fileId = fileInfo.Data.ResultId;
            }

            var userInfo = await GetUserInfoAsync();
            var timeStr = DateTime.Now.ToMillisecondTimestamp().ToString();
            var downloadId = $"{fileId}|{userInfo.UserId}"
                .ToAesHexExt(AesKey)
                .ToLower();
            var auth = $"{fileId}|{timeStr}"
                .ToAesHexExt(AesKey)
                .ToLower();
            var timestamp = timeStr
                .ToAesHexExt(AesKey)
                .ToLower();
            var reqUrl = $"/unproved/file/redirect?uuid={userInfo.DevCode}&devType=6&devCode={userInfo.DevCode}&devModel=chrome&devVersion=129&appVersion=&timestamp={timestamp}&appToken={userInfo.AppToken}&enable=0&downloadId={downloadId}&auth={auth}";
            KdyRequestCommonInput.SetGetRequest(reqUrl);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.LocationUrl.IsEmptyExt())
            {
                KdyLog.LogWarning("{userNick},蓝奏优享文件下载异常,Flag:{flag},Req:{input},ErrInfo:{msg}",
                    CloudConfig.ReqUserInfo, currentFlag, input, reqResult.ErrMsg);
                throw new KdyCustomException(reqResult.ErrMsg);
            }

            await KdyRedisCache.GetCache()
                .SetStringAsync(input.CacheKey, reqResult.LocationUrl, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60 * 2)
                });

            return KdyResult.Success<string>(reqResult.LocationUrl);
        }

        /// <summary>
        /// 批量更改名称
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input)
        {
            var nameCacheKey = GetCacheKeyWithFileName();
            var nameCacheDb = GetNameCacheDb();
            var result = false;
            var userInfo = await GetUserInfoAsync();
            foreach (var inputItem in input)
            {
                if (string.IsNullOrEmpty(inputItem.FileId) ||
                    string.IsNullOrEmpty(inputItem.NewName) ||
                    string.IsNullOrEmpty(inputItem.OldName))
                {
                    continue;
                }

                var tempData = new
                {
                    fileId = inputItem.FileId,
                    fileName = inputItem.NewName,
                    fileDesc = string.Empty
                };

                var timestamp = DateTime.Now.ToMillisecondTimestamp()
                    .ToString()
                    .ToAesHexExt(AesKey)
                    .ToLower();
                var reqUrl =
                    $"/proved/file/edit?uuid={userInfo.DevCode}&devType=6&devCode={userInfo.DevCode}&devModel=chrome&devVersion=129&appVersion=&timestamp={timestamp}&appToken={userInfo.AppToken}&extra=2";
                KdyRequestCommonInput.SetPostData(reqUrl,
                    tempData.ToJsonStr(),
                    isAjax: true);
                var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
                if (reqResult.IsSuccess)
                {
                    var fileNameMd5 = inputItem.OldName.Md5Ext();
                    result = true;
                    await nameCacheDb.DeleteHashSetAsync(nameCacheKey, fileNameMd5);
                }

                await Task.Delay(100);
            }

            if (result)
            {
                return KdyResult.Success();
            }

            return KdyResult.Error(KdyResultCode.Error, "改名失败");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        internal async Task<LanZouYxUserInfoCache> GetUserInfoAsync()
        {
            var cacheKey = GetUserInfoCacheKey();
            var baseInfo = await KdyRedisCache.GetCache().GetValueAsync<LanZouYxUserInfoCache>(cacheKey);
            if (baseInfo != null)
            {
                return baseInfo;
            }

            //配置用户名和密码 直接登录获取token
            var tempArray = CloudConfig.ParseCookie.Split("|");
            var userName = tempArray.First();
            var userPwd = tempArray.Last();
            var timestamp = DateTime.Now.ToMillisecondTimestamp()
                .ToString()
                .ToAesHexExt(AesKey)
                .ToLower();
            var devCode = Guid.NewGuid().ToString("D");

            //1、先登录
            var loginUrl = $"/unproved/login?uuid={devCode}&devType=6&devCode={devCode}&devModel=chrome&devVersion=129&appVersion=&timestamp={timestamp}&extra=2";
            var tempData = new
            {
                loginName = userName,
                loginPwd = userPwd,
            };
            KdyRequestCommonInput.SetPostData(loginUrl,
                tempData.ToJsonStr(),
                isAjax: true);
            var loginResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (loginResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},蓝奏登录失败.{loginResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(loginResult.Data);
            var appToken = jObject["appToken"] + "";
            await Task.Delay(500);
            //2、获取token 然后获取用户id
            timestamp = DateTime.Now.ToMillisecondTimestamp()
                .ToString()
                .ToAesHexExt(AesKey)
                .ToLower();
            var reqUrl = $"/proved/user/account/map?uuid={devCode}&devType=6&devCode={devCode}&devModel=chrome&devVersion=129&appVersion=&timestamp={timestamp}&appToken={appToken}&extra=2";
            KdyRequestCommonInput.SetGetRequest(reqUrl);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},蓝奏Cookie失效.{reqResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            jObject = JObject.Parse(reqResult.Data);
            //用户信息
            var userInfo = new LanZouYxUserInfoCache()
            {
                DevCode = devCode,
                UserId = jObject.GetValueExt("map.userId"),
                AppToken = appToken
            };

            await KdyRedisCache.GetCache().SetValueAsync(cacheKey, userInfo, TimeSpan.FromDays(1));
            return userInfo;
        }

        private string GetUserInfoCacheKey()
        {
            return $"{CacheKeyConst.LanZouYouXiangCacheKey.UserInfoCache}:{CloudConfig.ReqUserInfo}";
        }
    }
}
