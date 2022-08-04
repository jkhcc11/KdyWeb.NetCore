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
using KdyWeb.Dto.HttpCapture.KdyCloudParse.Cache;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.HttpCapture.KdyCloudParse;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 天翼个人网盘解析 实现
    /// </summary>
    public class TyPersonCloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut, string>, ITyPersonCloudParseService
    {
        public TyPersonCloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://cloud.189.cn")
            {
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput(),
                Cookie = cloudConfig.ParseCookie,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.81 Safari/537.36 SE 2.X MetaSr 1.0"
            };
            KdyRequestCommonInput.ExtData.HeardDic = new Dictionary<string, string>()
            {
                { "Accept", "application/json;charset=utf-8" }
            };
        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var resultList = new List<BaseResultOut>();

            //fileListAO 普通列表  fileList 搜索结果
            var fileArray = jObject?["fileListAO"]?["fileList"] as JArray ?? jObject?["fileList"] as JArray;

            var dirArray = jObject?["fileListAO"]?["folderList"] as JArray ?? jObject?["folderList"] as JArray;

            if (dirArray != null && dirArray.Any())
            {
                //文件夹处理
                foreach (JToken jToken in dirArray)
                {
                    var model = new BaseResultOut
                    {
                        ResultId = jToken["id"] + "",
                        ResultName = jToken["name"] + "",
                        FileType = CloudFileType.Dir
                    };
                    resultList.Add(model);
                }
            }

            if (fileArray != null && fileArray.Any())
            {
                //文件处理
                foreach (JToken jToken in fileArray)
                {
                    var model = new BaseResultOut
                    {
                        ResultId = jToken["id"] + "",
                        ResultName = jToken["name"] + "",
                        FileSize = Convert.ToInt64(jToken["size"])
                    };
                    model.FileType = model.ResultName.FileNameToFileType();
                    resultList.Add(model);
                }
            }

            return resultList;
        }

        public override async Task<KdyResult<List<BaseResultOut>>> QueryFileAsync(BaseQueryInput<string> input)
        {
            var resultList = new List<BaseResultOut>();
            if (input.Page <= 0)
            {
                input.Page = 1;
            }

            if (input.PageSize <= 0)
            {
                input.PageSize = 60;
            }

            if (string.IsNullOrEmpty(input.InputId))
            {
                //根目录
                input.InputId = "-11";
            }

            var reqUrl = $"/api/open/file/listFiles.action?noCache=0.{DateTime.Now.ToSecondTimestamp()}&pageSize={input.PageSize}&pageNum={input.Page}&mediaType=0&folderId={input.InputId}&iconOption=5&orderBy=filename&descending=false";
            if (input.KeyWord.IsEmptyExt() == false)
            {
                //关键字搜索
                reqUrl = $"/api/open/file/searchFiles.action?noCache=0.{DateTime.Now.ToSecondTimestamp()}&folderId=-11&pageSize=60&pageNum=1&filename={input.KeyWord}&recursive=1&iconOption=5&orderBy=lastOpTime&descending=true";
            }

            KdyRequestCommonInput.SetGetRequest(reqUrl);
            KdyRequestCommonInput.Referer = $"{KdyRequestCommonInput.BaseHost}/web/main/file/folder/{input.InputId}";
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}搜索文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            var jObject = JObject.Parse(reqResult.Data);
            var result = JArrayHandler(jObject);
            return KdyResult.Success(result);
        }

        public async Task<TyLoginInfoCache> GetLoginInfoAsync()
        {
            var cacheKey = $"{CacheKeyConst.TyCacheKey.UserInfoCache}:{CloudConfig.ReqUserInfo}";
            var cacheV = await KdyRedisCache.GetCache().GetValueAsync<TyLoginInfoCache>(cacheKey);
            if (cacheV != null)
            {
                return cacheV;
            }

            KdyRequestCommonInput.SetGetRequest($"/api/open/user/getUserInfoForPortal.action?noCache=0.{DateTime.Now.ToSecondTimestamp()}");
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},天翼Cookie失效.{reqResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(reqResult.Data);
            var userInfo = new TyLoginInfoCache()
            {
                UserAccount = jObject["userExtResp"]?["domainSpaceAccount"] + "",
                Email = jObject["userExtResp"]?["email"] + "",
                Nick = jObject["userExtResp"]?["nickName"] + "",
            };

            await KdyRedisCache.GetCache().SetValueAsync(cacheKey, userInfo, TimeSpan.FromMinutes(60));
            return userInfo;
        }

        public override async Task<KdyResult<string>> GetDownUrlForNoCacheAsync(BaseDownInput<string> input)
        {
            //1、获取实际下载地址
            KdyRequestCommonInput.SetGetRequest($"/api/open/file/getFileDownloadUrl.action?noCache=0.{DateTime.Now.ToSecondTimestamp()}&fileId={input.FileId}");
            KdyRequestCommonInput.Referer = $"{KdyRequestCommonInput.BaseHost}/web/main/file/folder/-11";
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}天翼云获取下载地址第一步异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常01,请稍等1-2分钟后重试");
            }

            //2、downUrl 有效5分钟
            var jObject = JObject.Parse(reqResult.Data);
            var firstDownUrl = jObject.GetValueExt("fileDownloadUrl");
            if (firstDownUrl.StartsWith("//"))
            {
                firstDownUrl = "https:" + firstDownUrl;
            }

            //3、获取最终地址
            KdyRequestCommonInput.ExtData.HeardDic = null;
            KdyRequestCommonInput.SetGetRequest(firstDownUrl);
            reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false &&
                reqResult.LocationUrl.IsEmptyExt())
            {
                KdyLog.LogWarning("{userNick}天翼云获取下载地址第二步异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常02,请稍等1-2分钟后重试");
            }

            //最后下载地址转为https的 这样移动网络可以不卡顿
            //2021-10 改为1小时45分钟
            var resultUrl = reqResult.LocationUrl.Replace("http:", "https:");
            await KdyRedisCache.GetCache().SetStringAsync(input.CacheKey, resultUrl, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(105)
            });

            return KdyResult.Success<string>(resultUrl);
        }
    }
}
