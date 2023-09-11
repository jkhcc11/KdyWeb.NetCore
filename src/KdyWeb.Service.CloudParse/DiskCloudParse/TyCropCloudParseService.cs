using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.CloudParse.CloudParseEnum;
using KdyWeb.CloudParse.Extensions;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.Dto.HttpCapture.KdyCloudParse.Cache;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.CloudParse.DiskCloudParse;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 天翼企业云网盘解析 实现
    /// </summary>
    public class TyCropCloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut>,
        ITyCropCloudParseService
    {

        /// <summary>
        /// 企业Id
        /// </summary>
        private string CorpId { get; set; }
        public TyCropCloudParseService(long childUserId)
        {
            CloudConfig = new BaseConfigInput(childUserId);
        }

        public TyCropCloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://b.cloud.189.cn")
            {
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput()
                {
                    IsAjax = true
                },
                Cookie = cloudConfig.ParseCookie,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36 Edg/94.0.992.38",
                Referer = "https://b.cloud.189.cn/main.action"
            };

        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var resultList = new List<BaseResultOut>();
            if (jObject?["corpFolder"] is JObject corpFiles)
            {
                //公司列表 通用
                var model = new BaseResultOut
                {
                    ResultId = corpFiles["fileId"] + "",
                    ResultName = corpFiles["fileName"] + "",
                    FileType = CloudFileType.Dir
                };
                resultList.Add(model);
            }

            var dirArray = jObject?["data"] as JArray;
            if (dirArray == null || dirArray.Count <= 0)
            {
                return resultList;
            }

            foreach (JToken jToken in dirArray)
            {
                var model = new BaseResultOut
                {
                    ResultId = jToken["fileId"] + "",
                    ResultName = jToken["fileName"] + "",
                    FileSize = Convert.ToInt64(jToken["fileSize"])
                };

                var dirType = jToken["isFolder"] + "";
                if (dirType.ToLower() == "true" || dirType.ToLower() == "1")
                {
                    model.FileType = CloudFileType.Dir;
                }
                else
                {
                    model.FileType = model.ResultName.FileNameToFileType();
                }

                resultList.Add(model);
            }

            return resultList;
        }

        public override async Task<KdyResult<List<BaseResultOut>>> QueryFileAsync(BaseQueryInput<string> input)
        {
            var resultList = new List<BaseResultOut>();
            CorpId = input.ExtData;
            if (string.IsNullOrEmpty(CorpId))
            {
                //初始化
                var initCropList = await GetCropListAsync();
                resultList = initCropList.Select(a => new BaseResultOut()
                {
                    ResultName = a.Key,
                    IsRoot = true,
                    ParentId = a.Value.Split('|').First(),
                    ResultId = a.Value.Split('|').Last(),
                    FileType = CloudFileType.Dir
                }).ToList();
                return KdyResult.Success(resultList);
            }

            if (input.Page <= 0)
            {
                input.Page = 1;
            }

            if (input.PageSize <= 0)
            {
                input.PageSize = 80;
            }

            var reqUrl = $"/user/listDepartmentFolders.action?corpId={input.ExtData}&pageNum=1&pageSize=80&getUserName=true&getTotalNum=true&noCache=0.{DateTime.Now.ToMillisecondTimestamp()}";
            if (string.IsNullOrEmpty(input.InputId) == false)
            {
                reqUrl = $"/user/listCompanyFiles.action?corpId={input.ExtData}&fileId={input.InputId}&fileNameLike=&mediaType=&orderBy=1&order=ASC&pageNum={input.Page}&pageSize={input.PageSize}&recursive=false&noCache=0.{DateTime.Now.ToMillisecondTimestamp()}";
            }

            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                //关键字搜索
                reqUrl = $"/user/getFullSearchList.action?corpId={input.ExtData}&corpFileSort=0&keyWord={input.KeyWord}&mediaType=&pageSize=20&searchDate=&searchId=&searchScore=&isSearchContent=1&noCache=0.{DateTime.Now.ToMillisecondTimestamp()}";
            }

            KdyRequestCommonInput.SetGetRequest(reqUrl);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}企业搜索文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            var jObject = JObject.Parse(reqResult.Data);
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
                ExtData = CorpId
            });

            var fileInfo = searchList.Data.FirstOrDefault(a => a.ResultName == keyWord);
            if (fileInfo == null)
            {
                KdyLog.LogWarning("{userNick}文件名:{fileName} 天翼企业盘,未搜索到请留意", CloudConfig.ReqUserInfo, keyWord);
                return KdyResult.Error<BaseResultOut>(KdyResultCode.Error, $"未找到文件名：{keyWord}");
            }

            await nameDb.SetHashSetAsync(nameCacheKey, fileNameMd5, fileInfo);
            return KdyResult.Success(fileInfo);
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
            var cacheKey = GetUserCropInfoCacheKey();
            await KdyRedisCache.GetCache().RemoveAsync(cacheKey);
            return await base.ClearCacheAsync();
        }

        public async Task<Dictionary<string, string>> GetCropListAsync()
        {
            var result = new Dictionary<string, string>();
            var cacheKey = GetUserCropInfoCacheKey();
            var cacheV = await KdyRedisCache.GetCache().GetValueAsync<Dictionary<string, string>>(cacheKey);
            if (cacheV != null)
            {
                return cacheV;
            }

            KdyRequestCommonInput.SetGetRequest($"/user/listCorp.action?noCache=0.{DateTime.Now.ToMillisecondTimestamp()}");
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},天翼获取家庭云列表异常.{reqResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(reqResult.Data);
            var jArray = jObject["data"] as JArray;
            if (jArray == null || jArray.Count <= 0)
            {
                return result;
            }

            foreach (JToken item in jArray)
            {
                result.Add($"{item["corpName"]}", $"{item["corpId"]}|{item["corpFolderId"]}");
            }

            await KdyRedisCache.GetCache()
                .SetValueAsync(cacheKey, result, TimeSpan.FromDays(1));
            return result;
        }

        /// <summary>
        /// 批量改名
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input)
        {
            var nameCacheKey = GetCacheKeyWithFileName();
            var nameCacheDb = GetNameCacheDb();
            var result = false;
            var corpId = input.First().ExtId;

            foreach (var inputItem in input)
            {
                if (string.IsNullOrEmpty(inputItem.FileId) ||
                    string.IsNullOrEmpty(inputItem.NewName) ||
                    string.IsNullOrEmpty(inputItem.OldName))
                {
                    continue;
                }

                //todo:待测试
                KdyRequestCommonInput
                    .SetGetRequest($"/user/renameCompanyFile.action?fileId={inputItem.FileId}&fileName={inputItem.NewName}&corpId={corpId}&noCache=0.{DateTime.Now.ToMillisecondTimestamp()}");
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

        public override async Task<KdyResult<string>> GetDownUrlForNoCacheAsync<TDownEntity>(BaseDownInput<TDownEntity> input)
        {
            if (input.ExtData is string corpId)
            {
                CorpId = corpId;
            }

            var fileId = input.FileId;
            if (input.DownUrlSearchType == DownUrlSearchType.Name)
            {
                //根据文件名获取文件Id
                var nameFileInfo = await GetFileInfoByKeyWordAsync(input.FileName);
                if (nameFileInfo.IsSuccess == false)
                {
                    return KdyResult.Error<string>(nameFileInfo.Code, nameFileInfo.Msg);
                }

                fileId = nameFileInfo.Data.ResultId;
            }

            var fileInfo = await GetFileInfoAsync(fileId, CorpId);

            //1、获取下载
            var reqInput = new KdyRequestCommonInput(fileInfo.TempDownUrl, HttpMethod.Get)
            {
                Cookie = KdyRequestCommonInput.Cookie,
                Referer = KdyRequestCommonInput.Referer,
                ExtData = new KdyRequestCommonExtInput()
            };
            var reqResult = await KdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false &&
                reqResult.LocationUrl.IsEmptyExt())
            {
                KdyLog.LogWarning("{userNick}天翼获取企业云下载第一步异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常01,请稍等1-2分钟后重试");
            }

            //2、获取最终地址
            reqInput.SetGetRequest(reqResult.LocationUrl);
            reqResult = await KdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false &&
                reqResult.LocationUrl.IsEmptyExt())
            {
                KdyLog.LogWarning("{userNick}天翼获取企业云下载第二步异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常02,请稍等1-2分钟后重试");
            }

            var ts = GetExpiresByUrl(reqResult.LocationUrl, "Expires");
            //最后下载地址转为https的 这样移动网络可以不卡顿
            //2021-10 改为2小时
            var resultUrl = reqResult.LocationUrl.Replace("http:", "https:");
            await KdyRedisCache.GetCache()
                .SetStringAsync(input.CacheKey, resultUrl, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = ts
                });

            return KdyResult.Success<string>(resultUrl);
        }

        /// <summary>
        /// 根据文件Id和企业云Id获取文件信息
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <param name="corpId">企业云Id</param>
        /// <returns></returns>
        internal async Task<TyCropFileInfoCache> GetFileInfoAsync(string fileId, string corpId)
        {
            var cacheKey = $"{CacheKeyConst.TyCacheKey.UserCropFileInfoCache}_{corpId}:{fileId}";
            var cacheV = await KdyRedisCache.GetCache().GetValueAsync<TyCropFileInfoCache>(cacheKey);
            if (cacheV != null)
            {
                return cacheV;
            }

            KdyRequestCommonInput.SetGetRequest($"/user/listHisVersion.action?corpId={corpId}&curFileId={fileId}&noCache=0.{DateTime.Now.ToMillisecondTimestamp()}");
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},天翼企业云获取文件异常.{reqResult.ErrMsg}");
            }

            var jObject = JObject.Parse(reqResult.Data);
            var fileModel = new TyCropFileInfoCache
            {
                FileId = fileId,
                ResultName = jObject.GetValueExt("currentFile.fileName"),
                TempDownUrl = jObject.GetValueExt("currentFile.downloadURL"),
            };

            if (string.IsNullOrEmpty(fileModel.TempDownUrl))
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},天翼企业云获取文件异常01");
            }

            if (fileModel.TempDownUrl.StartsWith("//"))
            {
                fileModel.TempDownUrl = $"https:{fileModel.TempDownUrl}";
            }

            //大概有效期是1天左右 2019-03-26新增  这个地址获取的下载有效期长
            await KdyRedisCache.GetCache().SetValueAsync(cacheKey, fileModel, TimeSpan.FromDays(1));
            return fileModel;
        }


        /// <summary>
        /// 获取用户企业信息缓存Key
        /// </summary>
        /// <returns></returns>
        private string GetUserCropInfoCacheKey()
        {
            return $"{CacheKeyConst.TyCacheKey.UserCropInfoCache}:{CloudConfig.ReqUserInfo}";
        }
    }
}
