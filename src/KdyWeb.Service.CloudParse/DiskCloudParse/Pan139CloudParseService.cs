using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 139解析 实现
    /// </summary>
    [CloudParseService(CloudParseCookieType.Pan139, DownCachePrefix = CacheKeyConst.Pan139CacheKey.DownCacheKey)]
    public class Pan139CloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut>,
        IPan139CloudParseService
    {
        private readonly string _account;
        private int _accountType = 1;
        private const string DefaultRootId = "00019700101000000001";

        /// <summary>
        /// 仅用于清除缓存
        /// </summary>
        /// <param name="childUserId">子账号Id</param>
        public Pan139CloudParseService(long childUserId)
        {
            CloudConfig = new BaseConfigInput(childUserId);
        }

        public Pan139CloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://yun.139.com")
            {
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput()
                {
                    HeardDic = new Dictionary<string, string>()
                    {
                        { "authorization",  cloudConfig.ParseCookie }
                    }
                },
                Referer = "https://yun.139.com/file/index",
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
            };

            var tempAccount = cloudConfig.ParseCookie.Remove(0, 6)
                .Base64ToStrExt(Encoding.UTF8);
            _account = tempAccount.Split(':')[1];
        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var result = new List<BaseResultOut>();

            var dirArray = jObject["data"]["getDiskResult"]?["catalogList"] as JArray;
            if (dirArray != null && dirArray.Any())
            {
                #region 文件夹
                foreach (JToken jToken in dirArray)
                {
                    var fileName = jToken["catalogName"];
                    if (fileName == null)
                    {
                        continue;
                    }

                    var model = new BaseResultOut
                    {
                        //文件夹路径
                        ParentId = $"{jToken["parentCatalogId"]}",
                        //文件id 或者文件夹id
                        ResultId = $"{jToken["catalogID"]}",
                        //名字
                        ResultName = fileName.ToString()
                    };

                    model.FileType = CloudFileType.Dir;

                    result.Add(model);
                }
                #endregion
            }

            //文件列表
            var fileArray = jObject["data"]["getDiskResult"]?["contentList"] as JArray;
            if (fileArray == null || fileArray.Any() == false)
            {
                fileArray = jObject["data"]["rows"] as JArray;
                if (fileArray == null || fileArray.Any() == false)
                {
                    return result;
                }
            }

            foreach (JToken jToken in fileArray)
            {
                var fileName = jToken["contentName"] ?? jToken["contName"];
                if (fileName == null)
                {
                    continue;
                }

                var model = new BaseResultOut
                {
                    //文件夹路径
                    ParentId = $"{jToken["parentCatalogId"]}",
                    //文件id 或者文件夹id
                    ResultId = $"{jToken["contentID"] ?? jToken["contID"]}",
                    //名字
                    ResultName = fileName.ToString(),
                    FileSize = long.Parse((jToken["contentSize"] ?? jToken["contSize"]) + "")
                };

                model.FileType = ("." + (jToken["contentSuffix"] ?? jToken["contSuffix"])).FileNameToFileType();

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
                input.PageSize = 100;
            }

            var startNumber = (input.Page - 1) * input.PageSize + 1;
            var endNumber = input.Page * input.PageSize;

            var reqUrl = "/orchestration/personalCloud/catalog/v1.0/getDisk";
            var tempData = new
            {
                catalogID = input.InputId,
                sortDirection = 1,
                startNumber = startNumber,
                endNumber = endNumber,
                filterType = 0,
                catalogSortType = 1, //1名称排序
                contentSortType = 1,
                commonAccountInfo = new
                {
                    account = _account,
                    accountType = _accountType
                }
            };
            var postDataJson = tempData.ToJsonStr();
            if (input.KeyWord.IsEmptyExt() == false)
            {
                //关键字搜索
                reqUrl = "/orchestration/personalCloud/search/v1.0/fileSearch";
                var tempKeyWordData = new
                {
                    conditions = $"search_name:\"{input.KeyWord}\" and path:\"{DefaultRootId}\"",
                    showInfo = new
                    {
                        startNum = 1,
                        stopNum = 100,
                        sortInfos = new List<int>()
                    },
                    commonAccountInfo = new
                    {
                        account = _account,
                        accountType = _accountType
                    }
                };
                postDataJson = tempKeyWordData.ToJsonStr();
            }

            KdyRequestCommonInput.SetPostData(reqUrl, postDataJson, isAjax: true);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick},139盘搜索文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            //{"code":"10500","message":"server error","data":null}
            var jObject = JObject.Parse(reqResult.Data);
            if (jObject.GetValue("success")?.Value<bool>() == false)
            {
                KdyLog.LogWarning("{userNick},139盘搜索文件异常,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
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
                KdyLog.LogWarning("{userNick}文件名:{fileName} 胜天未搜索到请留意", CloudConfig.ReqUserInfo, keyWord);
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

            var tempData = new
            {
                getFlvOnlineAddrReq = new
                {
                    contentID = fileId,
                    commonAccountInfo = new
                    {
                        account = _account,
                        accountType = _accountType
                    }
                }
            };
            KdyRequestCommonInput.SetPostData("/orchestration/personalCloud/content/v1.2/getFlvOnlineAddr", tempData.ToJsonStr(), isAjax: true);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false ||
                reqResult.LocationUrl.IsEmptyExt() == false)
            {
                //有跳转说明失效了
                KdyLog.LogWarning("{userNick},139盘文件下载异常,Flag:{flag},Req:{input},ErrInfo:{msg}",
                    CloudConfig.ReqUserInfo, currentFlag, input, reqResult.ErrMsg);
                throw new KdyCustomException(reqResult.ErrMsg);
            }

            var tempResult = JObject.Parse(reqResult.Data);
            var downUrl = tempResult.GetValueExt("data.getFlvOnlineAddrRes.presentURL");
            if (downUrl.IsEmptyExt())
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "无效播放地址"); ;
            }

            downUrl = downUrl.Replace("http", "https");
            await KdyRedisCache.GetCache()
                .SetStringAsync(input.CacheKey, downUrl, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60 * 2)
                });

            return KdyResult.Success<string>(downUrl);
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

                var tempData = new
                {
                    contentID = inputItem.FileId,
                    contentName = inputItem.NewName,
                    commonAccountInfo = new
                    {
                        account = _account,
                        accountType = _accountType
                    }
                };

                KdyRequestCommonInput.SetPostData("/orchestration/personalCloud/content/v1.0/updateContentInfo",
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
    }
}
