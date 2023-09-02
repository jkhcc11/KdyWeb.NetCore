using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Dto.KdyFile;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.FileStore;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.FileStore
{
    /// <summary>
    /// 微博上传 实现
    /// </summary>
    [Obsolete("2023已弃用")]
    public class WeiBoFileService : BaseKdyFileService<WeiBoFileInput>, IWeiBoFileService
    {
        private readonly string _postHost = "https://picupload.weibo.com";
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        private readonly IKdyRedisCache _kdyRedisCache;
        public WeiBoFileService(IHttpClientFactory httpClientFactory, IKdyRequestClientCommon kdyRequestClientCommon, IKdyRedisCache kdyRedisCache) : base(httpClientFactory)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
            _kdyRedisCache = kdyRedisCache;
        }

        public override async Task<KdyResult<KdyFileDto>> PostFileByBytes(WeiBoFileInput input)
        {
            var url =
                $"{_postHost}/interface/pic_upload.php?cb=https%3A%2F%2Fweibo.com%2Faj%2Fstatic%2Fupimgback.html%3F_wv%3D5%26callback%3DSTK_ijax_157495212677343&mime=image%2Fjpeg&data=base64&url=weibo.com%2Fu%2F2483430532&markpos=1&logo=1&nick=&marks=0&app=miniblog&s=rdxt&pri=null&file_source=1";
            var result = KdyResult.Error<KdyFileDto>(KdyResultCode.Error, "上传失败");
            var base64Img = ByteToBase64(input.FileBytes);
            var cookie = await GetLoginCookie();
            if (cookie.IsEmptyExt())
            {
                result.Msg = "微博获取登录Cookie失败";
                return result;
            }

            var httpInput = new KdyRequestCommonInput(url, HttpMethod.Post)
            {
                Cookie = cookie,
                Referer = "https://weibo.com",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36 Edg/85.0.564.44",
                ExtData = new KdyRequestCommonExtInput()
                {
                    PostData = $"b64_data={base64Img.ToUrlCodeExt()}"
                }
            };
            var httpResult = await _kdyRequestClientCommon.SendAsync(httpInput);
            if (httpResult.IsSuccess == false && httpResult.HttpCode != HttpStatusCode.Found)
            {
                result.Msg = httpResult.ErrMsg;
                KdyLog.LogWarning("微博上传失败.input:{input},result:{result}", input, result);
                return result;
            }

            var newStr = httpResult.LocationUrl.TrimEnd(';') + ';';
            if (newStr.Contains("pid") == false)
            {
                KdyLog.LogWarning("微博上传失败,获取Pid失败.input:{input},result:{result}", input, result);
                return result;
            }

            //取最后的pid 2022-12-28 fix
            //https://tvax2.sinaimg.cn/large/{location.Substring(lastIndex + 5)}.jpg
            var publicPrefix = KdyConfiguration.GetValue(
                KdyWebServiceConst.UploadConfig.WeiBoPublicPrefix, new List<string>()
                {
                    "tvax2"
                });
            var uploadResult = new KdyFileDto()
            {
                Url = $"https://{publicPrefix.RandomItem()}.sinaimg.cn/large/{newStr.GetStrMathExt("pid=", ";")}.jpg"
            };

            result = KdyResult.Success(uploadResult);
            return result;
        }

        public async Task<string> GetLoginCookie()
        {
            //todo:缓存待修改 需要放在get里面直接设置
            var cacheVal = await _kdyRedisCache.GetCache().GetStringAsync(KdyServiceCacheKey.WeiBoCookieKey);
            if (cacheVal.IsEmptyExt() == false)
            {
                return cacheVal;
            }

            var getCookieApi = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.WeiBoCookieApi);
            if (string.IsNullOrEmpty(getCookieApi))
            {
                return string.Empty;
            }

            string loginUrl = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.WeiBoCookieApiLoginUrl, "https://weibo.com/login.php?url=https%3A%2F%2Fweibo.com%2Fat%2Fweibo"),
                cookieKey = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.WeiBoCookieApiCookieKey, "SUB"),
                successFlag = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.WeiBoCookieApiSuccessFlag, "https://weibo.com/at/weibo");

            var url = $"{getCookieApi}/api/Login/weiBo";
            var pd = new
            {
                loginUrl,
                cookieKey,
                successFlag
            };
            var httpInput = new KdyRequestCommonInput(url, HttpMethod.Post)
            {
                Referer = getCookieApi,
                UserAgent = KdyWebServiceConst.DefaultUserAgent,
                ExtData = new KdyRequestCommonExtInput()
                {
                    PostData = pd.ToJsonStr(),
                    ContentType = "application/json"
                },
                TimeOut = 50
            };
            var httpResult = await _kdyRequestClientCommon.SendAsync(httpInput);
            if (httpResult.IsSuccess == false)
            {
                KdyLog.LogWarning("微博获取Cookie失败,返回:{0}", httpResult.ToJsonStr());
                return string.Empty;
            }

            var jObject = JObject.Parse(httpResult.Data);
            cacheVal = jObject.GetValueExt("data");
            await _kdyRedisCache.GetCache().SetStringAsync(KdyServiceCacheKey.WeiBoCookieKey, cacheVal, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
            });
            return cacheVal;
        }
    }
}
