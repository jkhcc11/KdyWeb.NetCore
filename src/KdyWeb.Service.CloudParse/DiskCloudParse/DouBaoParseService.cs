using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.CloudParse.CloudParseEnum;
using KdyWeb.CloudParse.Extensions;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.Out;
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
    /// 豆包 实现
    /// </summary>
    [CloudParseService(CloudParseCookieType.DouBao, DownCachePrefix = CacheKeyConst.DouBaoCacheKey.DownCacheKey)]
    public class DouBaoParseService : BaseKdyCloudParseService<BaseConfigInput, string, BaseResultOut>,
        IDouBaoParseService
    {
        private const string DeviceId = "7623775783081772578";
        private const string DeviceVersion = "3.8.0";
        /// <summary>
        /// 文档Id
        /// </summary>
        private string DocId { get; set; }

        /// <summary>
        /// 仅用于清除缓存
        /// </summary>
        /// <param name="childUserId">子账号Id</param>
        public DouBaoParseService(long childUserId)
        {
            CloudConfig = new BaseConfigInput(childUserId);
        }

        public DouBaoParseService(BaseConfigInput cloudConfig) : base(cloudConfig)
        {
            KdyRequestCommonInput = new KdyRequestCommonInput("https://www.doubao.com")
            {
                TimeOut = 5000,
                ExtData = new KdyRequestCommonExtInput(),
                Referer = "https://www.doubao.com",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36 Edg/129.0.0.0",
            };
        }

        protected override List<BaseResultOut> JArrayHandler(JObject jObject)
        {
            var result = new List<BaseResultOut>();
            //data=>block_map=>字典类型=>data=>type =="file"
            // 获取 block_map
            var blockMap = jObject["data"]?["block_map"] as JObject;

            if (blockMap == null)
            {
                return result;
            }

            // 遍历 block_map 中的所有 block
            foreach (var property in blockMap.Properties())
            {
                //var blockId = property.Name;
                var block = property.Value as JObject;

                // 获取 data 对象
                var data = block?["data"] as JObject;
                if (data == null)
                {
                    continue;
                }

                // 检查 type 是否为 "file"
                var type = data["type"]?.ToString();
                if (type != "file")
                {
                    continue;
                }

                // 提取 file 对象中的信息
                var fileObj = data["file"] as JObject;
                if (fileObj == null || (fileObj["mimeType"] + "").IsEmptyExt())
                {
                    continue;
                }

                var fileName = fileObj["name"] + "";
                var fileType = fileName.FileNameToFileType();

                long.TryParse($"{fileObj["fileSize"]}", out var size);
                var model = new BaseResultOut
                {
                    //写死的doc 扩展需要
                    ParentId = DocId,
                    //文件id 或者文件夹id
                    ResultId = fileObj["token"] + "",
                    //名字
                    ResultName = fileObj["name"] + "",
                    FileSize = size * 1024,
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

            DocId = input.ExtData;

            if (input.Page <= 0)
            {
                input.Page = 1;
            }

            if (input.PageSize <= 0)
            {
                input.PageSize = 60;
            }

            if (input.InputId.IsEmptyExt())
            {
                resultList = GetDefaultFileList(input);
            }
            else
            {
                //需要远程搜索了
                resultList = await QueryFileListByRemoteAsync(input);
            }

            if (input.KeyWord.IsEmptyExt() == false)
            {
                resultList = resultList.Where(a => a.ResultName.Contains(input.KeyWord)).ToList();
            }

            var result = resultList
                .Take(input.PageSize)
                .Skip((input.Page - 1) * input.PageSize)
                .ToList();
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
                ExtData = DocId,
                InputId = DocId,
                KeyWord = keyWord,
            });

            var fileInfo = searchList.Data.FirstOrDefault(a => a.ResultName == keyWord);
            if (fileInfo == null)
            {
                KdyLog.LogWarning("{userNick}文件名:{fileName} 豆包未搜索到请留意", CloudConfig.ReqUserInfo, keyWord);
                return KdyResult.Error<BaseResultOut>(KdyResultCode.Error, $"未找到文件名：{keyWord}");
            }

            await nameDb.SetHashSetAsync(nameCacheKey, fileNameMd5, fileInfo);
            return KdyResult.Success(fileInfo);
        }

        public override async Task<KdyResult<string>> GetDownUrlForNoCacheAsync<TDownEntity>(BaseDownInput<TDownEntity> input)
        {
            if (input.ExtData is string tempId)
            {
                DocId = tempId;
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

            var reqUrl = $"/samantha/otter/doc/attachment/mget?version_code=20800&language=zh-CN&device_platform=web&aid=497858&real_aid=497858&pkg_type=release_version&device_id={DeviceId}&pc_version={DeviceVersion}";
            var reqBody = new
            {
                page_token = DocId,
                keys = new[] { fileId }
            };
            KdyRequestCommonInput.SetPostData(reqUrl, reqBody.ToJsonStr());
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);

            var jObject = JObject.Parse(reqResult.Data);
            if (jObject.GetValue("code")?.Value<int>() != 0)
            {
                KdyLog.LogWarning("{userNick},豆包下载异常,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
                return KdyResult.Error<string>(KdyResultCode.Error, reqResult.ErrMsg);
            }

            // 定义清晰度优先级
            string[] priorities = { "1080p", "720p", "540p", "480p", "360p" };
            var bestUrl = GetPlayUrlBest(reqResult.Data, priorities);
            if (bestUrl.IsEmptyExt())
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取地址异常");
            }

            await KdyRedisCache.GetCache()
                .SetStringAsync(input.CacheKey, bestUrl, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
                });

            return KdyResult.Success<string>(bestUrl);
        }

        /// <summary>
        /// 首次获取时获取默认文件夹列表
        /// </summary>
        /// <returns></returns>
        internal List<BaseResultOut> GetDefaultFileList(BaseQueryInput<string> input)
        {
            //特殊这里的Cookie是文件名和文件Url   名称,文件Id
            //测试文件夹,HABrfCnQqdWtlncecqzcyLjvnKe
            var tempFileList = CloudConfig.ParseCookie.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a =>
                {
                    var tempArray = a.Split(',');
                    return new BaseResultOut()
                    {
                        IsRoot = true,
                        ParentId = tempArray.Last(),
                        ResultId = tempArray.Last(),
                        ResultName = tempArray.First(),
                        FileType = CloudFileType.Dir
                    };
                })
                .ToList();

            return tempFileList;
        }

        /// <summary>
        /// 需要远程加载
        /// </summary>
        /// <returns></returns>
        internal async Task<List<BaseResultOut>> QueryFileListByRemoteAsync(BaseQueryInput<string> input)
        {
            var resultList = new List<BaseResultOut>();
            var reqUrl = $"/samantha/otter/doc/pages/client_vars/?version_code=20800&language=zh-CN&device_platform=web&pkg_type=release_version&device_id={DeviceId}&pc_version{DeviceVersion}&id={input.InputId}";
            KdyRequestCommonInput.SetGetRequest(reqUrl);
            var reqResult = await KdyRequestClientCommon.SendAsync(KdyRequestCommonInput);
            if (reqResult.IsSuccess == false)
            {
                KdyLog.LogWarning("{userNick},豆包搜索文件异常,Req:{input},ErrInfo:{msg}", CloudConfig.ReqUserInfo, input, reqResult.ErrMsg);
                return resultList;
            }

            //{"code":"10500","message":"server error","data":null}
            var jObject = JObject.Parse(reqResult.Data);
            if (jObject.GetValue("code")?.Value<int>() != 0)
            {
                KdyLog.LogWarning("{userNick},豆包搜索文件异常,Req:{input},Response:{msg}", CloudConfig.ReqUserInfo, input, reqResult.Data);
                return resultList;
            }

            return JArrayHandler(jObject);
        }

        /// <summary>
        /// 根据Json字符串获取清晰度优先级
        /// </summary>
        /// <returns></returns>
        internal string GetPlayUrlBest(string json, string[] priorities)
        {
            var obj = JObject.Parse(json);

            // 获取所有 media_info
            var mediaInfos = obj.SelectTokens("$.data.attachments[*].play_info.media_info[*]").ToList();

            // 按优先级顺序查找
            foreach (var priority in priorities)
            {
                var matched = mediaInfos.FirstOrDefault(m => m["meta"]?["definition"]?.ToString() == priority);
                if (matched != null)
                {
                    return matched["main_url"]?.ToString();
                }
            }

            // 如果没有匹配，返回第一个可用的 main_url
            return mediaInfos.FirstOrDefault()?["main_url"]?.ToString();
        }
    }
}
