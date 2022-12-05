using System;
using System.Collections.Generic;
using System.Dynamic;
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
using KdyWeb.IService;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.HttpCapture.KdyCloudParse;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 阿里云盘解析 实现
    /// </summary>
    public class AliYunCloudParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut, DownTypeExtData>, IAliYunCloudParseService
    {
        /// <summary>
        /// 刷新Token Api old：https://websv.aliyundrive.com/token/refresh
        /// </summary>
        private readonly string _refreshApiHost = "https://auth.aliyundrive.com";

        public AliYunCloudParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://api.aliyundrive.com")
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.81 Safari/537.36 SE 2.X MetaSr 1.0",
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput()
            };
        }

        public async Task<KdyResult<AilYunCloudTokenCache>> GetLoginInfoAsync()
        {
            var cacheKey = $"{CacheKeyConst.AliYunCacheKey.AliReqToken}:{CloudConfig.ChildUserId}";
            var token = await KdyRedisCache.GetCache().GetValueAsync<AilYunCloudTokenCache>(cacheKey);
            if (token != null)
            {
                return KdyResult.Success(token);
            }

            var refreshToken = await GetRefreshTokenAsync();
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},获取刷新Token为空");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.Error, "");
            }

            //https://github.com/Xhofe/alist/pull/68 改进
            var postData = new
            {
                refresh_token = refreshToken,
                grant_type = "refresh_token"
            };

            var reqInput = new KdyRequestCommonInput(_refreshApiHost);
            reqInput.SetPostData("/v2/account/token", JsonConvert.SerializeObject(postData));
            var reqResult = await KdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                throw new KdyCustomException($"{CloudConfig.ReqUserInfo},阿里云刷新Token失败.{reqResult.ErrMsg}");
                //return KdyResult.Error<AilYunCloudTokenCache>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            //获取访问Token
            token = JsonConvert.DeserializeObject<AilYunCloudTokenCache>(reqResult.Data);
            await KdyRedisCache.GetCache().SetValueAsync(cacheKey, token, TimeSpan.FromMinutes(115));
            await SetNewRefreshTokenAsync(token.RefreshToken);
            return KdyResult.Success(token);
        }

        public async Task<KdyResult<BaseResultOut>> GetFileInfoByFileNameAsync(string fileName)
        {
            var fileNameMd5 = fileName.Md5Ext();
            var cacheKey = GetCacheKeyWithFileName();
            var fileNameInfo = await KdyRedisCache.GetDb().GetHashSetAsync<BaseResultOut>(cacheKey, fileNameMd5);
            if (fileNameInfo != null)
            {
                return KdyResult.Success(fileNameInfo);
            }

            var token = await GetLoginInfoAsync();
            var reqData = new
            {
                drive_id = token.Data.DriveId,
                limit = 100,
                all = false,
                image_thumbnail_process = "image/resize,w_400/format,jpeg",
                image_url_process = "image/resize,w_1920/format,jpeg",
                video_thumbnail_process = "video/snapshot,t_0,f_jpg,ar_auto,w_300",
                order_by = "updated_at DESC",
                query = $"name match \"{fileName}\""
            };
            var postJson = JsonConvert.SerializeObject(reqData);

            await SetAuthHeaderAsync();

            //搜索文件名
            var searchUrl = "/adrive/v3/file/search";
            KdyRequestCommonInput.SetPostData(searchUrl, postJson);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning($"{CloudConfig.ReqUserInfo}文件名:{fileName} 搜索异常。服务器返回：{reqResult.ErrMsg}");
                return KdyResult.Error<BaseResultOut>(KdyResultCode.HttpError, reqResult.ErrMsg);
            }

            //结果处理
            var jObject = JObject.Parse(reqResult.Data);
            var searchList = JArrayHandler(jObject);

            var fileInfo = searchList.FirstOrDefault(a => a.ResultName == fileName);
            if (fileInfo == null)
            {
                KdyLog.LogWarning("{userNick}文件名:{fileName} 未搜索到请留意", CloudConfig.ReqUserInfo, fileName);
                return KdyResult.Error<BaseResultOut>(KdyResultCode.Error, $"未找到文件名：{fileName}");
            }

            await KdyRedisCache.GetDb().SetHashSetAsync(cacheKey, fileNameMd5, fileInfo);
            return KdyResult.Success(fileInfo);
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
            var cacheKey = GetCacheKeyWithFileName();
            var token = await GetLoginInfoAsync();
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

                KdyRequestCommonInput.SetPostData("/v3/file/update", BuildUpdateNameReqData(inputItem, token.Data.DriveId));
                var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
                if (reqResult.IsSuccess)
                {
                    var fileNameMd5 = inputItem.OldName.Md5Ext();
                    result = true;
                    await KdyRedisCache.GetDb().DeleteHashSetAsync(cacheKey, fileNameMd5);
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

            var dirArray = jObject?["items"] as JArray;
            if (dirArray == null || dirArray.Any() == false)
            {
                return resultList;
            }

            foreach (JToken jToken in dirArray)
            {
                long.TryParse($"{jToken["size"]}", out var size);
                var model = new BaseResultOut
                {
                    ParentId = $"{jToken["drive_id"]}",
                    //resourceId
                    ResultId = $"{jToken["file_id"]}",
                    //名字
                    ResultName = $"{jToken["name"]}",
                    FileSize = size
                };


                var dirType = jToken["type"] + "";
                model.FileType = dirType == "folder" ? CloudFileType.Dir : model.ResultName.FileNameToFileType();
                resultList.Add(model);
            }

            return resultList;
        }

        public override async Task<KdyResult<List<BaseResultOut>>> QueryFileAsync(BaseQueryInput<string> input)
        {
            var token = await GetLoginInfoAsync();
            var resultList = new List<BaseResultOut>();
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

            if (input.PageSize > 200)
            {
                input.PageSize = 200;
            }

            if (string.IsNullOrEmpty(input.InputId))
            {
                //根目录
                input.InputId = "root";
            }

            var postJson = BuildRequestJsonByFileId(token.Data.DriveId, input.InputId, input.PageSize, nextMark);
            var reqUrl = "/adrive/v3/file/list";

            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                postJson = BuildRequestJsonByFileName(token.Data.DriveId, input.KeyWord);
                reqUrl = "/adrive/v3/file/search";
            }

            await SetAuthHeaderAsync();
            KdyRequestCommonInput.SetPostData(reqUrl, postJson);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}搜索文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return KdyResult.Success(resultList);
            }

            var jObject = JObject.Parse(reqResult.Data);
            //  nextMark = JsonHelper.GetTokenBySelectPath(rd.DataInfo, "next_marker");
            nextMark = jObject.GetValueExt("next_marker");
            if (string.IsNullOrEmpty(nextMark) == false)
            {
                //有翻页
                await SetNextMarkAsync(input.InputId, input.Page + 1, nextMark);
            }

            var result = JArrayHandler(jObject);
            return KdyResult.Success(result);
        }

        public override async Task<KdyResult<string>> GetDownUrlForNoCacheAsync(BaseDownInput<DownTypeExtData> input)
        {
            var token = await GetLoginInfoAsync();

            //是否切片
            var isTs = false;
            if (input.ExtData == DownTypeExtData.FileName)
            {
                //根据文件名获取文件Id
                var fileInfo = await GetFileInfoByFileNameAsync(input.FileName);
                if (fileInfo.IsSuccess == false)
                {
                    return KdyResult.Error<string>(fileInfo.Code, fileInfo.Msg);
                }

                input.FileId = fileInfo.Data.ResultId;
                if (fileInfo.Data.FileType == CloudFileType.VideoTs)
                {
                    isTs = true;
                }
            }

            var ts = TimeSpan.FromMinutes(110);
            string downUrl;
            if (isTs)
            {
                ts = TimeSpan.FromMinutes(13);
                downUrl = await GetTsUrlByFileIdAsync(input.FileId, token.Data.DriveId, "FHD");
            }
            else
            {
                #region 直链Mp4
                var reqData = new
                {
                    drive_id = token.Data.DriveId,
                    file_id = input.FileId,
                    expire_sec = 7200
                };
                var postJson = JsonConvert.SerializeObject(reqData);
                await SetAuthHeaderAsync();

                //https://api.aliyundrive.com/v2/databox/get_video_play_info  获取转码地址
                KdyRequestCommonInput.SetPostData("/v2/file/get_download_url", postJson);
                var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
                if (reqResult.IsSuccess == false)
                {
                    KdyLog.LogWarning("{userNick}获取文件下载异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                    return KdyResult.Error<string>(KdyResultCode.HttpError, reqResult.ErrMsg);
                }

                var jObject = JObject.Parse(reqResult.Data);
                downUrl = jObject.GetValueExt("url");

                #endregion
            }

            if (string.IsNullOrEmpty(downUrl))
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取下载地址失败,请稍后再试");
            }

            await KdyRedisCache.GetCache().SetStringAsync(input.CacheKey, downUrl, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = ts
            });
            return KdyResult.Success<string>(downUrl);
        }

        #region 私有

        /// <summary>
        /// 设置请求头
        /// </summary>
        internal async Task SetAuthHeaderAsync()
        {
            var token = await GetLoginInfoAsync();
            KdyRequestCommonInput.ExtData.HeardDic = new Dictionary<string, string>()
            {
                {"Authorization",$"Bearer {token.Data.Token}"}
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
        }

        /// <summary>
        /// 生成根据文件名筛选的 Json数据
        /// </summary>
        /// <param name="driveId">网盘Id</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        internal string BuildRequestJsonByFileName(string driveId, string fileName)
        {
            var reqDataByFileName = new
            {
                drive_id = driveId,
                limit = 100,
                all = false,
                image_thumbnail_process = "image/resize,w_400/format,jpeg",
                image_url_process = "image/resize,w_1920/format,jpeg",
                video_thumbnail_process = "video/snapshot,t_0,f_jpg,ar_auto,w_300",
                order_by = "name ASC",
                query = $"name match \"{fileName}\""
            };
            return JsonConvert.SerializeObject(reqDataByFileName);
        }

        /// <summary>
        /// 生成普通请求列表的Json数据
        /// </summary>
        /// <returns></returns>
        internal string BuildRequestJsonByFileId(string driveId, string fileId, int pageSize, string nextMark)
        {
            dynamic reqData = new ExpandoObject();
            ((IDictionary<string, object>)reqData).Add("drive_id", driveId);
            ((IDictionary<string, object>)reqData).Add("parent_file_id", fileId);
            ((IDictionary<string, object>)reqData).Add("limit", pageSize);
            ((IDictionary<string, object>)reqData).Add("all", false);
            ((IDictionary<string, object>)reqData).Add("url_expire_sec", 1600);

            ((IDictionary<string, object>)reqData).Add("image_thumbnail_process", "image/resize,w_400/format,jpeg");
            ((IDictionary<string, object>)reqData).Add("image_url_process", "image/resize,w_1920/format,jpeg");
            ((IDictionary<string, object>)reqData).Add("video_thumbnail_process", "video/snapshot,t_0,f_jpg,ar_auto,w_300");

            ((IDictionary<string, object>)reqData).Add("fields", "*");
            ((IDictionary<string, object>)reqData).Add("order_by", "name");
            ((IDictionary<string, object>)reqData).Add("order_direction", "ASC");//ASC|DESC

            if (string.IsNullOrEmpty(nextMark) == false)
            {
                ((IDictionary<string, object>)reqData).Add("marker", nextMark);
            }

            return JsonConvert.SerializeObject(reqData);
        }

        /// <summary>
        /// 获取翻页标记缓存
        /// </summary>
        /// <returns></returns>
        internal async Task<string> GetNextMarkAsync(string parentFileId, int page)
        {
            var nextMarkCacheKey = $"{CacheKeyConst.AliYunCacheKey.AliPageCacheKey}:{CloudConfig.ReqUserInfo}:{parentFileId}:{page}";
            return await KdyRedisCache.GetCache().GetStringAsync(nextMarkCacheKey);
        }

        /// <summary>
        /// 设置翻页标记缓存
        /// </summary>
        /// <returns></returns>
        internal async Task SetNextMarkAsync(string parentFileId, int page, string nextMark)
        {
            var nextMarkCacheKey = $"{CacheKeyConst.AliYunCacheKey.AliPageCacheKey}:{CloudConfig.ReqUserInfo}:{parentFileId}:{page}";
            await KdyRedisCache.GetCache()
                .SetStringAsync(nextMarkCacheKey, nextMark, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });
        }

        /// <summary>
        /// 根据文件Id获取切片地址 
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <param name="driveId">网盘Id</param>
        /// <param name="templateId">转码模板Id 只要FHD|HD 优先FHD</param>
        /// <returns></returns>
        internal async Task<string> GetTsUrlByFileIdAsync(string fileId, string driveId, string templateId)
        {
            var reqData = new
            {
                drive_id = driveId,
                file_id = fileId,
                category = "live_transcoding",
                template_id = templateId
            };
            var postJson = JsonConvert.SerializeObject(reqData);
            await SetAuthHeaderAsync();
            KdyRequestCommonInput.SetPostData("/v2/file/get_video_preview_play_info", postJson);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick}获取转码地址异常,FileId:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, fileId, reqResult.ErrMsg);
                return string.Empty;
            }

            var jObject = JObject.Parse(reqResult.Data);
            var downUrl = jObject.GetValueExt("video_preview_play_info.live_transcoding_task_list[0].url");
            if (string.IsNullOrEmpty(downUrl) &&
                templateId != "HD")
            {
                //只循环两次
                return await GetTsUrlByFileIdAsync(fileId, driveId, "HD");
            }

            return downUrl;
        }

        /// <summary>
        /// 生成改名Json数据
        /// </summary>
        /// <returns></returns>
        internal string BuildUpdateNameReqData(BatchUpdateNameInput updateItem, string driveId)
        {
            var reqData = new
            {
                drive_id = driveId,
                file_id = updateItem.FileId,
                name = updateItem.NewName,
                check_name_mode = "refuse"
            };
            return JsonConvert.SerializeObject(reqData);
        }

        /// <summary>
        /// 获取文件名缓存Key
        /// </summary>
        /// <remarks>
        ///  文件名->文件Id对应关系得HashSet Key
        /// </remarks>
        /// <returns></returns>
        internal string GetCacheKeyWithFileName()
        {
            return $"{CacheKeyConst.AliYunCacheKey.FileNameCache}:{CloudConfig.ReqUserInfo}";
        }

        /// <summary>
        /// 获取刷新Token缓存Key
        /// </summary>
        /// <returns></returns>
        internal string GetCacheKeyWithRefreshToken()
        {
            return $"{CacheKeyConst.AliYunCacheKey.AliRefreshToken}:{CloudConfig.ReqUserInfo}";
        }
        #endregion

    }
}
