using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.CloudParse.CloudParseEnum;
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
        private const string DefaultRootId = "/";

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
            KdyRequestCommonInput = new KdyRequestCommonInput("https://personal-kd-njs.yun.139.com")
            {
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput()
                {
                    HeardDic = new Dictionary<string, string>()
                    {
                        {"Authorization",  cloudConfig.ParseCookie },
                        {"X-Yun-Api-Version","v1"},
                        {"X-Yun-App-Channel","10000034"},
                        {"X-Yun-Channel-Source","10000034"},
                        {"X-Yun-Client-Info","||9|7.13.5|edge||d3c93139b2021a3b00cfb503f0dbe16d||windows 10||zh-CN|||ZWRnZQ==||"},
                        {"X-Yun-Module-Type","100"},
                        {"X-Yun-Svc-Type","1"},
                    }
                },
                Referer = "https://yun.139.com/",
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
            };

            var tempAccount = cloudConfig.ParseCookie.Remove(0, 6)
                .Base64ToStrExt(Encoding.UTF8);
            _account = tempAccount.Split(':')[1];
        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var result = new List<BaseResultOut>();
            //搜索结果用这个
            var dirArray = jObject["rows"] as JArray;
            if (jObject["data"] is JObject tempData)
            {
                dirArray = tempData["items"] as JArray;
            }

            if (dirArray == null || dirArray.Any() == false)
            {
                return result;
            }

            foreach (JToken jToken in dirArray)
            {
                var fileName = jToken["name"];
                if (fileName == null)
                {
                    continue;
                }

                long.TryParse($"{jToken["size"]}", out var size);
                var model = new BaseResultOut
                {
                    //文件夹路径
                    ParentId = $"{jToken["parentFileId"]}",
                    //文件id 或者文件夹id
                    ResultId = $"{jToken["fileId"]}",
                    //名字
                    ResultName = fileName.ToString(),
                    FileSize = size
                };

                var category = jToken["category"] + "";
                switch (category)
                {
                    case "folder":
                        {
                            model.FileType = CloudFileType.Dir;
                            break;
                        }
                    case "image":
                        {
                            model.FileType = CloudFileType.Image;
                            break;
                        }
                    case "3":
                    case "video":
                        {
                            model.FileType = CloudFileType.Video;
                            break;
                        }
                    default:
                        {
                            model.FileType = CloudFileType.File;
                            break;
                        }
                }

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

            var nextMark = string.Empty;
            if (input.Page > 1)
            {
                nextMark = await GetNextMarkAsync(input.InputId, input.Page);
                if (string.IsNullOrEmpty(nextMark))
                {
                    return KdyResult.Success(resultList);
                }
            }

            if (input.PageSize <= 0)
            {
                input.PageSize = 100;
            }

            var reqUrl = "/hcy/file/list";
            var tempData = new
            {
                parentFileId = input.InputId,
                //名称排序
                orderBy = "name",
                orderDirection = "ASC",
                imageThumbnailStyleList = new[] { "Small", "Large" },
                pageInfo = new
                {
                    pageSize = 100,
                    //翻页标识
                    pageCursor = nextMark,
                }
            };

            var postDataJson = tempData.ToJsonStr();
            if (input.KeyWord.IsEmptyExt() == false)
            {
                //关键字搜索
                reqUrl = "https://search-njs.yun.139.com/search/SearchFile";
                var tempKeyWordData = new
                {
                    conditions = new
                    {
                        keyword = input.KeyWord,
                        owner = _account,
                        type = _accountType,
                    },
                    showInfo = new
                    {
                        returnTotalCountFlag = true,
                        startNum = 1,
                        stopNum = 100,
                        sortInfos = new List<int>()
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

            nextMark = jObject.GetValueExt("data.nextPageCursor");
            if (string.IsNullOrEmpty(nextMark) == false)
            {
                //有翻页
                await SetNextMarkAsync(input.InputId, input.Page + 1, nextMark);
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
                category = "video",
                fileId,
            };
            KdyRequestCommonInput.SetPostData("/hcy/videoPreview/getPreviewInfo", tempData.ToJsonStr(), isAjax: true);
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
            var downUrl = tempResult.GetValueExt("data.previewInfo.url");
            if (downUrl.IsEmptyExt())
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "无效播放地址");
            }

            if (downUrl.StartsWith("http:"))
            {
                downUrl = downUrl.Replace("http", "https");
            }

            //类似这样
            //xxxxxx/v2/hls/1471112557543558912/playlist.m3u8?ci=FtEfgbCZOWK2h0TqhDR9AiKcKYcN-zlOt&fileSize=232430401&isNew=1
            //直接拼接1080p地址
            //xxxx/v2/hls/1471112557543558912/single/video/0/1080/index.m3u8
            //todo:这里后面可能会调整 待定
            var playListIndex = downUrl.IndexOf("playlist", StringComparison.Ordinal);
            if (playListIndex == -1)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "无效播放地址02");
            }

            var prefix = downUrl.Substring(0, playListIndex);
            var checkList = new[]
            {
                "1080",
                "720",
                "480"
            };
            downUrl = string.Empty;

            foreach (var item in checkList)
            {
                var tempUrl = $"{prefix}single/video/0/{item}/index.m3u8";
                var checkInput = new KdyRequestCommonInput(tempUrl, HttpMethod.Get)
                {
                    Referer = "https://yun.139.com/",
                    UserAgent =
                        "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
                };
                var checkResult = await KdyRequestClientCommon.SendAsync(checkInput);
                if (checkResult.HttpCode == HttpStatusCode.NotFound ||
                    checkResult.HttpCode == HttpStatusCode.InternalServerError)
                {
                    //404 下一个检查
                    continue;
                }

                downUrl = tempUrl;
            }

            if (string.IsNullOrEmpty(downUrl))
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "waiting code 03");
            }

            await KdyRedisCache.GetCache()
                .SetStringAsync(input.CacheKey, downUrl, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60 * 12)
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
                    fileId = inputItem.FileId,
                    name = inputItem.NewName,
                };

                KdyRequestCommonInput.SetPostData("/hcy/file/update",
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
        /// 获取翻页标记缓存
        /// </summary>
        /// <returns></returns>
        internal async Task<string> GetNextMarkAsync(string parentFileId, int page)
        {
            var nextMarkCacheKey = $"{CacheKeyConst.Pan139CacheKey.AliPageCacheKey}:{CloudConfig.ReqUserInfo}:{parentFileId}:{page}";
            return await KdyRedisCache.GetCache().GetStringAsync(nextMarkCacheKey);
        }

        /// <summary>
        /// 设置翻页标记缓存
        /// </summary>
        /// <returns></returns>
        internal async Task SetNextMarkAsync(string parentFileId, int page, string nextMark)
        {
            var nextMarkCacheKey = $"{CacheKeyConst.Pan139CacheKey.AliPageCacheKey}:{CloudConfig.ReqUserInfo}:{parentFileId}:{page}";
            await KdyRedisCache.GetCache()
                .SetStringAsync(nextMarkCacheKey, nextMark, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });
        }
    }
}
