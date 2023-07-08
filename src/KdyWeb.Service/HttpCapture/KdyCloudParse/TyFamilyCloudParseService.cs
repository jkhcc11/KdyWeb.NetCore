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
    /// 天翼家庭网盘解析 实现
    /// </summary>
    public class TyFamilyCloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut, string>, ITyFamilyCloudParseService
    {
        public TyFamilyCloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://api.cloud.189.cn")
            {
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput(),
                Cookie = cloudConfig.ParseCookie,
                UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1",
                Referer = "https://h5.cloud.189.cn/main.html"
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

            var reqUrl = $"/open/family/file/listFiles.action?pageNum={input.Page}&pageSize={input.PageSize}&familyId={input.ExtData}&folderId=&iconOption=5&orderBy=1&descending=false";
            if (string.IsNullOrEmpty(input.InputId) == false)
            {
                reqUrl = $"/open/family/file/listFiles.action?pageNum={input.Page}&pageSize={input.PageSize}&familyId={input.ExtData}&folderId={input.InputId}&iconOption=5&orderBy=1&descending=false";
            }

            var time = DateTime.Now.ToMillisecondTimestamp() + "";
            var url = $"AccessToken={CloudConfig.ParseCookie}&Timestamp={time}&descending=false&familyId={input.ExtData}&folderId={input.InputId}&iconOption=5&orderBy=1&pageNum={input.Page}&pageSize={input.PageSize}";
            BuilderReqHeadSign(url, time);

            KdyRequestCommonInput.SetGetRequest(reqUrl);
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

        public async Task<Dictionary<string, string>> GetFamilyListAsync()
        {
            var result = new Dictionary<string, string>();
            var cacheKey = $"{CacheKeyConst.TyCacheKey.UserFamilyInfoCache}:{CloudConfig.ReqUserInfo}";
            var cacheV = await KdyRedisCache.GetCache().GetValueAsync<Dictionary<string, string>>(cacheKey);
            if (cacheV != null)
            {
                return cacheV;
            }

            KdyRequestCommonInput.SetGetRequest("/open/family/manage/getFamilyList.action");
            var time = DateTime.Now.ToMillisecondTimestamp() + "";
            BuilderReqHeadSign($"AccessToken={CloudConfig.ParseCookie}&Timestamp={time}", time);

            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},天翼获取家庭云列表异常.{reqResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(reqResult.Data);
            var jArray = jObject?["familyInfoResp"] as JArray;
            if (jArray == null || jArray.Count <= 0)
            {
                return result;
            }

            foreach (JToken item in jArray)
            {
                result.Add($"{item["remarkName"]}", $"{item["familyId"]}");
            }

            await KdyRedisCache.GetCache().SetValueAsync(cacheKey, result, TimeSpan.FromDays(1));
            return result;
        }

        public override async Task<KdyResult<string>> GetDownUrlForNoCacheAsync(BaseDownInput<string> input)
        {
            //1、获取下载
            KdyRequestCommonInput.SetGetRequest($"/open/family/file/getFileDownloadUrl.action?fileId={input.FileId}&familyId={input.ExtData}");
            var time = DateTime.Now.ToMillisecondTimestamp() + "";
            BuilderReqHeadSign($"AccessToken={CloudConfig.ParseCookie}&Timestamp={time}&familyId={input.ExtData}&fileId={input.FileId}", time);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},天翼获取家庭云列表异常.{reqResult.ErrMsg}");
            }

            //2、获取最终地址
            var jObject = JObject.Parse(reqResult.Data);
            var tempUrl = jObject.GetValueExt("fileDownloadUrl");
            if (string.IsNullOrEmpty(tempUrl))
            {
                KdyLog.LogWarning("{userNick}天翼家庭云获取下载地址第一步异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常01,请稍等1-2分钟后重试");
            }

            KdyRequestCommonInput.ExtData.HeardDic = null;
            tempUrl = tempUrl.Replace("&amp;", "&");
            KdyRequestCommonInput.SetGetRequest(tempUrl);
            KdyRequestCommonInput.Cookie = "COOKIE_CTWAP_LOGOUT=COOKIE_CTWAP_LOGOUT; COOKIE_LOGIN_USER=null";
            reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false &&
                reqResult.LocationUrl.IsEmptyExt())
            {
                KdyLog.LogWarning("{userNick}天翼家庭云获取下载地址第二步异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常02,请稍等1-2分钟后重试");
            }

            var ts = GetExpiresByUrl(reqResult.LocationUrl, "Expires");
            //最后下载地址转为https的 这样移动网络可以不卡顿
            //2021-10  改为2小时
            var resultUrl = reqResult.LocationUrl.Replace("http:", "https:");
            await KdyRedisCache.GetCache().SetStringAsync(input.CacheKey, resultUrl, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = ts
            });

            return KdyResult.Success<string>(resultUrl);
        }

        /// <summary>
        /// 生成授权请求头
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timestamp"></param>
        internal void BuilderReqHeadSign(string url, string timestamp)
        {
            var sign = url.Md5Ext("");
            KdyRequestCommonInput.ExtData.HeardDic = new Dictionary<string, string>()
            {
                { "Accept", "application/json;charset=utf-8" },
                { "accesstoken", CloudConfig.ParseCookie },
                { "sign-type", "1" },
                { "signature", sign },
                { "timestamp", timestamp }
            };
        }
    }
}
