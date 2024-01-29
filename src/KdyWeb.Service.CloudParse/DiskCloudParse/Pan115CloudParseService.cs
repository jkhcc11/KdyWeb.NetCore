using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
    /// 115解析 实现
    /// </summary>
    [CloudParseService(CloudParseCookieType.Pan115, DownCachePrefix = CacheKeyConst.Pan115CacheKey.DownCacheKey)]
    public class Pan115CloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut>,
        IPan115CloudParseService
    {
        private const string DefaultRootId = "0";
        private const string WebApiHost = "https://webapi.115.com";
        private const string DownHost = "https://v.anxia.com";

        /// <summary>
        /// 仅用于清除缓存
        /// </summary>
        /// <param name="childUserId">子账号Id</param>
        public Pan115CloudParseService(long childUserId)
        {
            CloudConfig = new BaseConfigInput(childUserId);
        }

        public Pan115CloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://aps.115.com")
            {
                TimeOut = 5000,
                Cookie = cloudConfig.ParseCookie,
                Referer = "https://115.com/",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0",
            };
        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var result = new List<BaseResultOut>();

            //文件列表
            var fileArray = jObject["data"] as JArray;
            if (fileArray == null || fileArray.Any() == false)
            {
                return result;
            }

            foreach (JToken jToken in fileArray)
            {
                var fileName = jToken["n"];
                if (fileName == null)
                {
                    continue;
                }

                var fileType = fileName.ToString().FileNameToFileType();
                var icon = jToken.Value<string>("ico");
                if (string.IsNullOrEmpty(icon))
                {
                    fileType = CloudFileType.Dir;
                }

                var model = new BaseResultOut
                {
                    //用于下载
                    ParentId = $"{(fileType != CloudFileType.Dir ? jToken["pc"] : jToken["cid"])}",
                    //直接获取内容的（用于改名啥的）
                    ResultId = $"{(fileType != CloudFileType.Dir ? jToken["fid"] : jToken["cid"])}",
                    //名字
                    ResultName = fileName.ToString(),
                    FileSize = (fileType != CloudFileType.Dir ? long.Parse((jToken["s"] + "")) : 0),
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

            var startNumber = (input.Page - 1) * input.PageSize;

            var reqUrl = "/natsort/files.php";
            if (input.KeyWord.IsEmptyExt() == false)
            {
                //关键字搜索
                reqUrl = $"/files/search?offset={startNumber}&limit=50&search_value={input.KeyWord}&date=&aid=1&cid=0&pick_code=&type=&count_folders=1&source=&format=json";
                KdyRequestCommonInput.BaseHost = WebApiHost;
                KdyRequestCommonInput.SetGetRequest(reqUrl);
            }
            else
            {
                var reqPar = $"aid=1&cid={input.InputId}&o=file_name&asc=1&offset={startNumber}&show_dir=1&limit={input.PageSize}&code=&scid=&snap=0&natsort=1&record_open_time=1&count_folders=1&source=&format=json&fc_mix=0";
                KdyRequestCommonInput.SetGetRequest($"{reqUrl}?{reqPar}");
            }

            //KdyRequestCommonInput.SetPostData(reqUrl, postDataJson, isAjax: true);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick},115盘搜索文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            //{"code":"10500","message":"server error","data":null}
            var jObject = JObject.Parse(reqResult.Data);
            if (jObject.GetValue("state")?.Value<bool>() == false)
            {
                KdyLog.LogWarning("{userNick},115盘搜索文件异常,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
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
                KdyLog.LogWarning("{userNick}文件名:{fileName} 115未搜索到请留意", CloudConfig.ReqUserInfo, keyWord);
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

                fileId = fileInfo.Data.ParentId;
            }

            if (input.ExtData is StDownExtData stDownExt)
            {
                KdyRequestCommonInput.UserAgent = stDownExt.UserAgent;
            }

            KdyRequestCommonInput.BaseHost = string.Empty;
            KdyRequestCommonInput.SetGetRequest($"{DownHost}/site/api/video/m3u8/{fileId}.m3u8");

            //KdyRequestCommonInput.SetPostData("/orchestration/personalCloud/content/v1.2/getFlvOnlineAddr", tempData.ToJsonStr(), isAjax: true);
            var m3U8Result = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (m3U8Result.IsSuccess == false ||
                m3U8Result.HttpCode == HttpStatusCode.Forbidden)
            {
                KdyLog.LogWarning("{userNick}，115文件下载异常01.Flag:{flag}获取转码失败,Req:{input},ErrInfo:{msg}"
                    , CloudConfig.ReqUserInfo
                    , currentFlag
                    , input
                    , m3U8Result.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常,请稍后2分钟后重试");
            }

            var resultUrl = string.Empty;
            //优先1080p
            var index = m3U8Result.Data.IndexOf("UD", StringComparison.CurrentCultureIgnoreCase);
            if (index == -1)
            {
                index = m3U8Result.Data.IndexOf("HD", StringComparison.CurrentCultureIgnoreCase);
            }
            if (index > 0)
            {
                var tempM3U8Array = m3U8Result.Data.Split(new[] { "\r", "\n", "" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempM3U8Array.Length >= 3)
                {
                    resultUrl = tempM3U8Array[2];
                }
            }

            if (resultUrl.IsEmptyExt())
            {
                KdyLog.LogWarning("{userNick}11文件下载异常02.Flag:{flag},获取转码失败,Req:{input},ErrInfo:{msg}"
                    , CloudConfig.ReqUserInfo
                    , currentFlag
                    , input
                    , m3U8Result.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常,请稍后2分钟后重试");
            }

            //todo:待测试时间
            await KdyRedisCache.GetCache()
                .SetStringAsync(input.CacheKey, resultUrl, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2 * 60)
                });

            return KdyResult.Success<string>(resultUrl);
        }

        /// <summary>
        /// 批量更改名称
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input)
        {
            var nameCacheKey = GetCacheKeyWithFileName();
            var nameCacheDb = GetNameCacheDb();
            var postDic = new Dictionary<string, string>();
            foreach (var inputItem in input)
            {
                if (string.IsNullOrEmpty(inputItem.FileId) ||
                    string.IsNullOrEmpty(inputItem.NewName) ||
                    string.IsNullOrEmpty(inputItem.OldName))
                {
                    continue;
                }

                postDic.Add($"files_new_name%5B{inputItem.FileId}%5D", inputItem.NewName);

                //先清空文件名缓存
                var fileNameMd5 = inputItem.OldName.Md5Ext();
                await nameCacheDb.DeleteHashSetAsync(nameCacheKey, fileNameMd5);
            }

            var tempStr = string.Join("&", postDic
                .Select(a => $"{a.Key}={a.Value}"));
            KdyRequestCommonInput.BaseHost = WebApiHost;
            KdyRequestCommonInput.SetPostData("/files/batch_rename", $"{tempStr}&share_id=310005",
                isAjax: true,
                contentType: "application/x-www-form-urlencoded");
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess)
            {
                return KdyResult.Success();
            }

            return KdyResult.Error(KdyResultCode.Error, "改名失败");
        }
    }
}
