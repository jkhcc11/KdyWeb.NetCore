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
using KdyWeb.IService.CloudParse;
using KdyWeb.IService.CloudParse.DiskCloudParse;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Service.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 光鸭云盘 实现
    /// </summary>
    [CloudParseService(CloudParseCookieType.GuangYa, DownCachePrefix = CacheKeyConst.GuangYaCacheKey.DownCacheKey)]
    public class GuangYaCloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut>,
        IGuangYaCloudParseService
    {
        private readonly string _refreshApiHost = "https://account.guangyapan.com";

        public GuangYaCloudParseService(long childUserId)
        {
            CloudConfig = new BaseConfigInput(childUserId);
        }

        public GuangYaCloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://api.guangyapan.com")
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36 Edg/149.0.0.0",
                TimeOut = 5000,
                Referer = "https://api.guangyapan.com",
                ExtData = new KdyRequestCommonExtInput(),
            };
        }


        /// <summary>
        /// 批量改名
        /// </summary>
        /// <remarks>
        ///  只要不是全部失败 都是True
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> BatchUpdateNameAsync(List<BatchUpdateNameInput> input)
        {
            var nameCacheKey = GetCacheKeyWithFileName();
            var nameCacheDb = GetNameCacheDb();
            var result = false;

            await SetAuthHeaderAsync();

            foreach (var inputItem in input)
            {
                if (string.IsNullOrEmpty(inputItem.FileId) ||
                    string.IsNullOrEmpty(inputItem.NewName) ||
                    string.IsNullOrEmpty(inputItem.OldName))
                {
                    continue;
                }

                KdyRequestCommonInput.SetPostData("/userres/v1/file/rename", BuildUpdateNameReqData(inputItem));
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

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var resultList = new List<BaseResultOut>();

            var dirArray = jObject?["data"]?["list"] as JArray;
            if (dirArray == null || dirArray.Any() == false)
            {
                return resultList;
            }

            foreach (JToken jToken in dirArray)
            {
                long.TryParse($"{jToken["fileSize"]}", out var size);
                var model = new BaseResultOut
                {
                    //ParentId = $"{jToken["parentId"]}",
                    //resourceId
                    ResultId = $"{jToken["fileId"]}",
                    //名字
                    ResultName = $"{jToken["fileName"]}",
                    FileSize = size
                };


                var dirType = jToken["fileType"] + "";
                model.FileType = string.IsNullOrEmpty(dirType) ? CloudFileType.Dir : model.ResultName.FileNameToFileType();
                resultList.Add(model);
            }

            return resultList;
        }

        public override async Task<KdyResult<List<BaseResultOut>>> QueryFileAsync(BaseQueryInput<string> input)
        {
            var resultList = new List<BaseResultOut>();
            if (input.Page <= 1)
            {
                input.Page = 0;
            }

            if (input.PageSize <= 0)
            {
                input.PageSize = 100;
            }

            if (input.PageSize > 200)
            {
                input.PageSize = 200;
            }

            if (string.IsNullOrEmpty(input.InputId))
            {
                //根目录
                input.InputId = string.Empty;
            }

            var postJsonDic = new Dictionary<string, object>()
            {
                {"pageSize",input.PageSize},
                {"orderBy",0},
                {"sortType",0},
                {"page",input.Page},
                {"parentId",input.InputId}
            };

            var reqUrl = "/userres/v1/file/get_file_list";
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                postJsonDic = new Dictionary<string, object>()
                {
                    {"pageSize",8},
                    {"page",0},
                    {"name",input.KeyWord}
                };
                reqUrl = "/userres/v1/file/search_files";
            }

            await SetAuthHeaderAsync();
            KdyRequestCommonInput.SetPostData(reqUrl, postJsonDic.ToJsonStr());
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

        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <remarks>
        /// 根据各网盘缓存情况删除
        /// </remarks>
        /// <returns></returns>
        public override async Task<bool> ClearCacheAsync()
        {
            //刷新Token
            var refreshCacheKey = GetCacheKeyWithRefreshToken();
            await KdyRedisCache.GetCache().RemoveAsync(refreshCacheKey);

            //请求token
            var reqCacheKey = $"{CacheKeyConst.GuangYaCacheKey.ReqToken}:{CloudConfig.ChildUserId}";
            await KdyRedisCache.GetCache().RemoveAsync(reqCacheKey);
            return await base.ClearCacheAsync();
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

            var reqData = new
            {
                fileId = fileId
            };
            var postJson = JsonConvert.SerializeObject(reqData);
            await SetAuthHeaderAsync();

            KdyRequestCommonInput.SetPostData("/userres/v1/get_res_download_url", postJson);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}获取文件下载异常,Flag:{flag},Req:{input},ErrInfo:{msg}",
                    CloudConfig.ReqUserInfo, currentFlag, input, reqResult.ErrMsg);
                return KdyResult.Error<string>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            var jObject = JObject.Parse(reqResult.Data);
            var downUrl = jObject.GetValueExt("data.signedURL");

            if (string.IsNullOrEmpty(downUrl))
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取下载地址失败,请稍后再试");
            }

            await KdyRedisCache.GetCache().SetStringAsync(input.CacheKey, downUrl, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120)
            });
            return KdyResult.Success<string>(downUrl);
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
                KdyLog.LogWarning("{userNick}文件名:{fileName} 光鸭未搜索到请留意", CloudConfig.ReqUserInfo, keyWord);
                return KdyResult.Error<BaseResultOut>(KdyResultCode.Error, $"未找到文件名：{keyWord}");
            }

            await nameDb.SetHashSetAsync(nameCacheKey, fileNameMd5, fileInfo);
            return KdyResult.Success(fileInfo);
        }

        #region 私有
        internal async Task<KdyResult<GuangYaCloudTokenCache>> GetLoginInfoAsync()
        {
            var cacheKey = $"{CacheKeyConst.GuangYaCacheKey.ReqToken}:{CloudConfig.ChildUserId}";
            var token = await KdyRedisCache.GetCache().GetValueAsync<GuangYaCloudTokenCache>(cacheKey);
            if (token != null)
            {
                return KdyResult.Success(token);
            }

            var refreshToken = await GetRefreshTokenAsync();
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},获取刷新Token为空");
            }

            var postData = new
            {
                client_id= "aMe-8VSlkrbQXpUR",
                refresh_token = refreshToken,
                grant_type = "refresh_token"
            };

            var reqInput = new KdyRequestCommonInput(_refreshApiHost);
            reqInput.SetPostData("/v1/auth/token", JsonConvert.SerializeObject(postData));
            var reqResult = await KdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},光鸭刷新Token失败.{reqResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            //获取访问Token
            token = JsonConvert.DeserializeObject<GuangYaCloudTokenCache>(reqResult.Data);
            await KdyRedisCache.GetCache().SetValueAsync(cacheKey, token, TimeSpan.FromMinutes(115));
            await SetNewRefreshTokenAsync(token.RefreshToken);
            return KdyResult.Success(token);
        }


        /// <summary>
        /// 设置请求头
        /// </summary>
        internal async Task SetAuthHeaderAsync()
        {
            var token = await GetLoginInfoAsync();
            //清空
            KdyRequestCommonInput.ExtData.HeardDic = new Dictionary<string, string>()
            {
                {"Authorization",$"Bearer {token.Data.Token}"},
            };
        }

        /// <summary>
        /// 获取刷新Token
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetRefreshTokenAsync()
        {
            //先读cache 如果有就不读取db配置
            var refreshCacheKey = GetCacheKeyWithRefreshToken();
            var refreshToken = await KdyRedisCache.GetCache().GetStringAsync(refreshCacheKey);
            if (string.IsNullOrEmpty(refreshToken) == false)
            {
                return refreshToken;
            }

            //为空时直接读配置
            return CloudConfig.ParseCookie;
        }

        /// <summary>
        /// 设置最新刷新token
        /// </summary>
        /// <returns></returns>
        private async Task SetNewRefreshTokenAsync(string newRefreshToken)
        {
            var refreshCacheKey = GetCacheKeyWithRefreshToken();
            await KdyRedisCache.GetCache().SetStringAsync(refreshCacheKey, newRefreshToken);

            //更新db
            var subAccountService = KdyBaseServiceProvider.HttpContextServiceProvide
                .GetRequiredService<ISubAccountService>();
            await subAccountService.UpdateSubAccountInfoAsync(CloudConfig.ChildUserId, newRefreshToken);
        }

        /// <summary>
        /// 生成改名Json数据
        /// </summary>
        /// <returns></returns>
        internal string BuildUpdateNameReqData(BatchUpdateNameInput updateItem)
        {
            var reqData = new
            {
                fileId = updateItem.FileId,
                newName = updateItem.NewName
            };
            return JsonConvert.SerializeObject(reqData);
        }

        /// <summary>
        /// 获取刷新Token缓存Key
        /// </summary>
        /// <returns></returns>
        internal string GetCacheKeyWithRefreshToken()
        {
            return $"{CacheKeyConst.GuangYaCacheKey.RefreshToken}:{CloudConfig.ChildUserId}";
        }
        #endregion

    }
}
