using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
    /// 腾讯云分享解析 实现
    /// </summary>
    [CloudParseService(CloudParseCookieType.TxShare, DownCachePrefix = CacheKeyConst.TxShareCacheKey.DownCacheKey)]
    public class TShareCloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut>,
        ITShareCloudParseService
    {
        public TShareCloudParseService(long childUserId)
        {
            CloudConfig = new BaseConfigInput(childUserId);
        }

        public TShareCloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://api.vs.tencent.com")
            {
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput()
                {
                    HeardDic = new Dictionary<string, string>()
                    {
                        {"x-weapp-pub-id","437840001"},
                        {"Origin","https://app.v.tencent.com"},
                        {"xweb_xhr","1"}
                    }
                },
                Cookie = cloudConfig.ParseCookie,
                Referer = "https://servicewechat.com/wxb1dd1d968a7269a3/177/page-frame.html",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 MicroMessenger/7.0.20.1781(0x6700143B) NetType/WIFI MiniProgramEnv/Windows WindowsWechat/WMPF XWEB/8447"
            };
        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var resultList = new List<BaseResultOut>();
            var dirArray = jObject?["Data"]["ResourceInfoSet"] as JArray;
            if (dirArray == null || dirArray.Count <= 0)
            {
                return resultList;
            }

            foreach (JToken jToken in dirArray)
            {
                //long.TryParse($"{jToken["ClassInfo"]["AttachmentInfo"]["Size"]}", out var size);
                var fileTypeStr = jToken["Type"] + "";
                var fileType = fileTypeStr == "CLASS" ? CloudFileType.Dir : CloudFileType.Video;
                BaseResultOut model;
                if (fileType == CloudFileType.Dir)
                {
                    var size = jToken["ClassInfo"]?["AttachmentInfo"]?.Value<long?>("Size") ?? 0;
                    model = new BaseResultOut
                    {
                        ResultId = $"{jToken["ClassInfo"]?["ClassId"]}",
                        //名字
                        ResultName = $"{jToken["ClassInfo"]?["Name"]}",
                        FileSize = size,
                        FileType = fileType
                    };
                }
                else
                {
                    var size = jToken["MaterialInfo"]?["MediaInfo"]?.Value<long?>("Size") ?? 0;
                    model = new BaseResultOut
                    {
                        ResultId = $"{jToken["MaterialInfo"]?["BasicInfo"]?["MaterialId"]}",
                        //名字
                        ResultName = $"{jToken["MaterialInfo"]?["BasicInfo"]?["Name"]}",
                        FileSize = size,
                    };
                    model.FileType = model.ResultName.FileNameToFileType();
                }

                resultList.Add(model);
            }

            return resultList;
        }

        public override async Task<KdyResult<List<BaseResultOut>>> QueryFileAsync(BaseQueryInput<string> input)
        {
            var currentFlag = HttpContextAccessor.HttpContext?.TraceIdentifier;
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
                input.PageSize = 20;
            }

            var reqUrl = "/PaaS/Material/SearchResource";
            var isSearchOneDepth = true;
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                input.InputId = baseInfo.RootDir;
                reqUrl = "/PaaS/Material/SearchResource";
                isSearchOneDepth = false;
            }

            var postJsonData = new
            {
                Offset = (input.Page - 1) * input.PageSize,
                Limit = input.PageSize,
                Sort = new
                {
                    Field = "Name",
                    Order = "Asc"
                },
                SearchScopes = new List<object>
                {
                    new
                    {
                        ClassId=int.Parse(input.InputId),
                        SearchOneDepth=isSearchOneDepth,
                        Owner=new
                        {
                            Type="PERSON",
                            Id=baseInfo.UserId
                        }
                    }
                },
                Text = input.KeyWord
            };

            KdyRequestCommonInput.SetPostData(reqUrl, postJsonData.ToJsonStr());
            SetReqAuth();
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}搜索文件异常,TxShare,Flag:{flag},Req:{input},ErrInfo:{msg}",
                    CloudConfig.ReqUserInfo, currentFlag, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            //{"code":"10500","message":"server error","data":null}
            var jObject = JObject.Parse(reqResult.Data);
            if (jObject.Value<string>("Code") != "Success")
            {
                KdyLog.LogWarning("{userNick}搜索文件异常,TxShare,Flag:{flag},Req:{input},Response:{msg}",
                    CloudConfig.ReqUserInfo, currentFlag, input, reqResult.Data);
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
                KdyLog.LogWarning("{userNick}文件名:{fileName} TxShare未搜索到请留意", CloudConfig.ReqUserInfo, keyWord);
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

            var postJsonData = new
            {
                MaterialIds = new List<string>()
                {
                    fileId
                }
            };
            KdyRequestCommonInput.SetPostData("/PaaS/Material/DescribeMaterials",
                postJsonData.ToJsonStr());
            SetReqAuth();
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}TxShare文件下载异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                throw new KdyCustomException(reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(reqResult.Data);
            if (jObject.Value<string>("Code") != "Success")
            {
                KdyLog.LogWarning("{userNick}TxShare下载异常,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
                return KdyResult.Error<string>(KdyResultCode.Error, "登录已过期或转码异常");
            }

            var mainM3U8Url = jObject["Data"]?["MaterialInfoSet"]?[0]?["BasicInfo"]?.Value<string>("MediaUrl");
            if (string.IsNullOrEmpty(mainM3U8Url))
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "未知地址");
            }

            await KdyRedisCache.GetCache()
                .SetStringAsync(input.CacheKey, mainM3U8Url, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
                });

            return KdyResult.Success<string>(mainM3U8Url);
        }

        /// <summary>
        /// 批量更改名称
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input)
        {
            var baseInfo = await GetRootDirAsync();
            if (baseInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "用户信息失效");
            }

            var nameCacheKey = GetCacheKeyWithFileName();
            var nameCacheDb = GetNameCacheDb();
            var result = false;
            var successCount = 0;
            foreach (var inputItem in input)
            {
                if (string.IsNullOrEmpty(inputItem.FileId) ||
                    string.IsNullOrEmpty(inputItem.NewName) ||
                    string.IsNullOrEmpty(inputItem.OldName))
                {
                    continue;
                }

                var postJsonData = new
                {
                    Owner = new
                    {
                        Type = "PERSON",
                        Id = baseInfo.UserId,
                    },
                    MaterialId = inputItem.FileId,
                    Name = inputItem.NewName
                };
                KdyRequestCommonInput.SetPostData("/PaaS/Material/ModifyMaterial",
                    postJsonData.ToJsonStr());
                SetReqAuth();

                var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
                if (reqResult.IsSuccess)
                {
                    var fileNameMd5 = inputItem.OldName.Md5Ext();
                    result = true;
                    await nameCacheDb.DeleteHashSetAsync(nameCacheKey, fileNameMd5);
                    successCount++;
                }

                await Task.Delay(100);
            }

            if (result)
            {
                return KdyResult.Success("修改成功,数量：" + successCount);
            }

            return KdyResult.Error(KdyResultCode.Error, "修改失败");
        }

        /// <summary>
        /// 同步名称和Id映射
        /// </summary>
        /// <remarks>
        /// 没有搜索功能,只能映射
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> SyncNameIdMapAsync(List<BatchUpdateNameInput> input)
        {
            var nameCacheKey = GetCacheKeyWithFileName();
            var nameDb = GetNameCacheDb();

            var result = new List<bool>();
            foreach (var inputItem in input)
            {
                var fileNameMd5 = inputItem.OldName.Md5Ext();
                var setResult = await nameDb.SetHashSetAsync<BaseResultOut>(nameCacheKey, fileNameMd5,
                    new BaseResultOut()
                    {
                        ResultId = inputItem.FileId,
                        ResultName = inputItem.OldName
                    });
                result.Add(setResult);
            }

            return KdyResult.Success($"成功数量：{result.Count(a => a)}");
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

            KdyRequestCommonInput.SetPostData("/SaaS/Account/DescribeAccount", "{\"CheckSubscribeOffiAccount\":true}");
            SetReqAuth();
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},TShare获取用户失效.{reqResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(reqResult.Data);
            //用户信息
            var userInfo = new StUserInfoCache()
            {
                UserId = jObject["Data"]?.Value<string>("TfUid"),
                Mobile = jObject["Data"]?.Value<string>("Phone"),
                RootDir = "9"
            };

            await KdyRedisCache.GetCache().SetValueAsync(cacheKey, userInfo, TimeSpan.FromMinutes(60));
            return userInfo;
        }

        private string GetRootDirCacheKey()
        {
            return $"{CacheKeyConst.TxShareCacheKey.UserInfoCache}:{CloudConfig.ReqUserInfo}";
        }

        /// <summary>
        /// Auth
        /// </summary>
        private void SetReqAuth()
        {
            KdyRequestCommonInput.Url += $"?auth={KdyRequestCommonInput.Cookie}";
            KdyRequestCommonInput.Cookie = string.Empty;
        }
    }
}
