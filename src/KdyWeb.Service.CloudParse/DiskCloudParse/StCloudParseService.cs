using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.CloudParse.DiskCloudParse;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 盛天网盘解析 实现
    /// </summary>
    public class StCloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut>,
        IStCloudParseService
    {
        public StCloudParseService(long childUserId)
        {
            CloudConfig = new BaseConfigInput(childUserId);
        }

        public StCloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://pan.bitqiu.com")
            {
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput(),
                Cookie = cloudConfig.ParseCookie
            };
        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var resultList = new List<BaseResultOut>();
            var dirArray = jObject?["data"]["data"] as JArray;
            if (dirArray == null || dirArray.Count <= 0)
            {
                return resultList;
            }

            foreach (JToken jToken in dirArray)
            {
                long.TryParse($"{jToken["size"]}", out var size);
                var model = new BaseResultOut
                {
                    ParentId = $"{jToken["parentId"]}",
                    //resourceId
                    ResultId = $"{jToken["resourceId"]}",
                    //名字
                    ResultName = $"{jToken["name"]}",
                    FileSize = size
                };

                var dirType = jToken["dirType"] + "";
                model.FileType = string.IsNullOrEmpty(dirType) == false ? CloudFileType.Dir : model.ResultName.FileNameToFileType();
                resultList.Add(model);
            }

            return resultList;
        }

        public override async Task<KdyResult<List<BaseResultOut>>> QueryFileAsync(BaseQueryInput<string> input)
        {
            var resultList = new List<BaseResultOut>();
            if (CloudConfig.ParseCookie.IsEmptyExt())
            {
                return KdyResult.Success(resultList);
            }

            var baseInfo = await GetRootDirAsync();
            if (baseInfo == null)
            {
                return KdyResult.Success(resultList);
            }

            if (input.InputId.IsEmptyExt())
            {
                input.InputId = baseInfo.RootDir;
            }

            if (input.Page <= 0)
            {
                input.Page = 1;
            }

            if (input.PageSize <= 0)
            {
                input.PageSize = 24;
            }

            var reqUrl = "/apiToken/cfi/fs/resources/pages";
            var postData = $"parentId={input.InputId}&page={input.Page}&limit={input.PageSize}&orderType=name&desc=0&model=1&userId={baseInfo.UserId}&name=&org_channel=default%7Cdefault%7Cdefault";
            if (input.KeyWord.IsEmptyExt() == false)
            {
                //关键字搜索
                reqUrl = "/apiToken/cfi/fs/search/name";
                postData = $"parentId={baseInfo.RootDir}&page={input.Page}&limit={input.PageSize}&orderType=name&desc=0&model=1&userId={baseInfo.UserId}&name={input.KeyWord}&org_channel=default%7Cdefault%7Cdefault";
            }

            KdyRequestCommonInput.SetPostData(reqUrl, postData, "application/x-www-form-urlencoded", true);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}搜索文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            //{"code":"10500","message":"server error","data":null}
            var jObject = JObject.Parse(reqResult.Data);
            if (jObject["code"] + "" != "10200")
            {
                KdyLog.LogWarning("{userNick}搜索文件异常,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
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
                Page = 1,
                PageSize = 50
            });

            var fileInfo = searchList.Data.FirstOrDefault(a => a.ResultName == keyWord);
            if (fileInfo == null)
            {
                KdyLog.LogWarning("{userNick}文件名:{fileName} 胜天未搜索到请留意", CloudConfig.ReqUserInfo, keyWord);
                return KdyResult.Error<BaseResultOut>(KdyResultCode.Error, $"未找到文件名：{keyWord}");
            }

            await nameDb.SetHashSetAsync(nameCacheKey, fileNameMd5, fileInfo);
            return KdyResult.Success(fileInfo);
        }

        public override async Task<KdyResult<string>> GetDownUrlForNoCacheAsync<TDownEntity>(BaseDownInput<TDownEntity> input)
        {
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

            var baseInfo = await GetRootDirAsync();
            KdyRequestCommonInput.SetPostData("/apiToken/videoTranscode/getVideoP1080", $"userId={baseInfo.UserId}&fileUid={fileId}&fileId={fileId}&org_channel=default%7Cdefault%7Cdefault", "application/x-www-form-urlencoded", true);

            if (input.ExtData is StDownExtData stDownExt)
            {
                KdyRequestCommonInput.UserAgent = stDownExt.UserAgent;
            }

            KdyRequestCommonInput.Referer = $"{KdyRequestCommonInput.BaseHost}/video?fileUid={fileId}&resourceId={fileId}";
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}胜天文件下载异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                throw new KdyCustomException(reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(reqResult.Data);
            if (jObject["code"] + "" != "10200")
            {
                KdyLog.LogWarning("{userNick}胜天文件下载异常,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
                return KdyResult.Error<string>(KdyResultCode.Error, "登录已过期或转码异常");
            }

            var mainM3U8Url = jObject["data"]?["m3u8Url"] + "";
            if (mainM3U8Url.IsEmptyExt())
            {
                KdyLog.LogWarning("{userNick}胜天文件下载失败可能正在转码,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
                return KdyResult.Error<string>(KdyResultCode.Error, "视频正在转码,请稍后2分钟后重试");
            }

            //根据主文件获取转码后的地址
            KdyRequestCommonInput.SetGetRequest(mainM3U8Url);
            var m3U8Result = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (m3U8Result.IsSuccess == false ||
                m3U8Result.HttpCode == HttpStatusCode.Forbidden)
            {
                KdyLog.LogWarning("{userNick}胜天文件下载异常01,获取转码失败,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常,请稍后2分钟后重试");
            }

            var resultUrl = string.Empty;
            //优先1080p
            var index = m3U8Result.Data.IndexOf("原画", StringComparison.CurrentCultureIgnoreCase);
            if (index == -1)
            {
                index = m3U8Result.Data.IndexOf("蓝光", StringComparison.CurrentCultureIgnoreCase);
            }
            if (index == -1)
            {
                index = m3U8Result.Data.IndexOf("超清", StringComparison.CurrentCultureIgnoreCase);
            }
            if (index == -1)
            {
                index = m3U8Result.Data.IndexOf("高清", StringComparison.CurrentCultureIgnoreCase);
            }
            if (index > 0)
            {
                var tempM3U8Array = m3U8Result.Data.Split(new[] { "\r", "\n", "" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempM3U8Array.Length >= 4)
                {
                    resultUrl = tempM3U8Array[3];
                }
            }

            if (resultUrl.IsEmptyExt())
            {
                KdyLog.LogWarning("{userNick}胜天文件下载异常02,获取转码失败,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常,请稍后2分钟后重试");
            }

            await KdyRedisCache.GetCache()
                .SetStringAsync(input.CacheKey, resultUrl, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            return KdyResult.Success<string>(resultUrl);
        }

        public async Task<KdyResult<string>> AddCloudDownloadAsync(string url)
        {
            var baseInfo = await GetRootDirAsync();
            KdyRequestCommonInput.SetPostData("/cloudDownload/addTasks", $"dirId=&userId={baseInfo.UserId}&downloadUrls=%5B%22{url.ToUrlCodeExt().ToUrlCodeExt()}%22%5D&org_channel=default%7Cdefault%7Cdefault", "application/x-www-form-urlencoded", true);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}胜天网盘离线下载异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, url, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "离线异常");
            }

            var jObject = JObject.Parse(reqResult.Data);
            if ((jObject["code"] + "") != "10200")
            {
                return KdyResult.Error<string>(KdyResultCode.Error, $"盛天网盘离线返回:{jObject["message"]}");
            }

            return KdyResult.Success("", "操作成功");
        }

        public async Task<KdyResult<List<StCloudDownloadListOut>>> QueryCloudDownloadAsync(BaseQueryInput<string> input)
        {
            var resultList = new List<StCloudDownloadListOut>();
            var baseInfo = await GetRootDirAsync();
            if (baseInfo == null)
            {
                return KdyResult.Success(resultList);
            }

            if (input.InputId.IsEmptyExt())
            {
                input.InputId = baseInfo.RootDir;
            }

            if (input.Page <= 0)
            {
                input.Page = 1;
            }

            if (input.PageSize <= 0)
            {
                input.PageSize = 20;
            }

            var reqUrl = "/cloudDownload/getUserTaskList";
            var postData = $"currentPage={input.Page}&limit={input.PageSize}&userId={baseInfo.UserId}&status=0&task=&org_channel=default%7Cdefault%7Cdefault";

            KdyRequestCommonInput.SetPostData(reqUrl, postData, "application/x-www-form-urlencoded", true);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}获取离线文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            //结果格式化
            var jObject = JObject.Parse(reqResult.Data);
            var dirArray = jObject["data"]?["data"] as JArray;
            if (dirArray == null || dirArray.Count <= 0)
            {
                return KdyResult.Success(resultList);
            }

            var jsonStr = jObject["data"]["data"] + "";
            resultList = JsonConvert.DeserializeObject<List<StCloudDownloadListOut>>(jsonStr);
            return KdyResult.Success(resultList);
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
            foreach (var inputItem in input)
            {
                if (string.IsNullOrEmpty(inputItem.FileId) ||
                    string.IsNullOrEmpty(inputItem.NewName) ||
                    string.IsNullOrEmpty(inputItem.OldName))
                {
                    continue;
                }

                KdyRequestCommonInput.SetPostData("/resource/rename", $"resourceId={inputItem.FileId}&name={inputItem.NewName}&type=2&org_channel=default%7Cdefault%7Cdefault", "application/x-www-form-urlencoded", true);
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
        /// 清空缓存
        /// </summary>
        /// <remarks>
        /// 根据各网盘缓存情况删除
        /// </remarks>
        /// <returns></returns>
        public override async Task<bool> ClearCacheAsync()
        {
            var cacheKey = GetRootDirCacheKey();
            await KdyRedisCache.GetCache().RemoveAsync(cacheKey);
            return await base.ClearCacheAsync();
        }

        /// <summary>
        /// 获取根目录即用户信息
        /// </summary>
        internal async Task<StUserInfoCache> GetRootDirAsync()
        {
            var cacheKey = GetRootDirCacheKey();
            var baseInfo = await KdyRedisCache.GetCache().GetValueAsync<StUserInfoCache>(cacheKey);
            if (baseInfo != null)
            {
                return baseInfo;
            }

            KdyRequestCommonInput.SetPostData("/user/getInfo", "org_channel=default%7Cdefault%7Cstpan", "application/x-www-form-urlencoded", true);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},胜天Cookie失效.{reqResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(reqResult.Data);
            //用户信息
            var userInfo = JsonConvert.DeserializeObject<StUserInfoCache>(jObject["data"] + "");

            await KdyRedisCache.GetCache().SetValueAsync(cacheKey, userInfo, TimeSpan.FromMinutes(60));
            return userInfo;
        }

        private string GetRootDirCacheKey()
        {
            return $"{CacheKeyConst.StCacheKey.UserInfoCache}:{CloudConfig.ReqUserInfo}";
        }
    }
}
