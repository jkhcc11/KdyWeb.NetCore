using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Dto.KdyFile;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.KdyFile;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.KdyFile
{
    /// <summary>
    /// 微博上传 实现
    /// </summary>
    public class WeiBoFileService : BaseKdyFileService<BaseKdyFileInput>, IWeiBoFileService
    {
        private readonly string _postHost = "https://picupload.weibo.com";
        private readonly string _publicHost = "https://tva2.sinaimg.cn";
        private readonly IConfiguration _configuration;
        private readonly IKdyRedisCache _kdyRedisCache;
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;

        public WeiBoFileService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
            IKdyRedisCache kdyRedisCache, IKdyRequestClientCommon kdyRequestClientCommon) : base(httpClientFactory)
        {
            _configuration = configuration;
            _kdyRedisCache = kdyRedisCache;
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        public override async Task<KdyResult<KdyFileDto>> PostFileByBytes(BaseKdyFileInput input)
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
                KdyLog.Warn("微博上传失败");
                return result;
            }

            var newStr = httpResult.LocationUrl.TrimEnd(';') + ';';
            if (newStr.Contains("pid") == false)
            {
                KdyLog.Warn("微博上传失败,获取Pid失败");
                return result;
            }

            //取最后的pid
            //https://tva2.sinaimg.cn/large/{location.Substring(lastIndex + 5)}.jpg
            var uploadResult = new KdyFileDto()
            {
                Url = $"{_publicHost}/large/{newStr.GetValueExt("pid=", ";")}.jpg"
            };

            result = KdyResult.Success(uploadResult);
            return result;
        }

        public override async Task<KdyResult<KdyFileDto>> PostFileByUrl(BaseKdyFileInput input)
        {
            try
            {
                input.FileBytes = await GetFileBytesByUrl(input.FileUrl);
                input.FileUrl = string.Empty;
                return await PostFileByBytes(input);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless()
                    .AddTags($"{this}")
                    .Submit();
                return KdyResult.Error<KdyFileDto>(KdyResultCode.Error, $"微博 Url上传异常【{ex.Message}】");
            }
        }

        public async Task<string> GetLoginCookie()
        {
            string cacheVal = await _kdyRedisCache.GetCache().GetStringAsync(KdyServiceCacheKey.WeiBoCookieKey);
            if (cacheVal.IsEmptyExt() == false)
            {
                return cacheVal;
            }

            var config = _configuration
                .GetSection(KdyWebServiceConst.WeiBoConfigKey)
                .Get<WeiBoConfig>();

            var url = $"https://login.sina.com.cn/sso/login.php?client=ssologin.js(v1.4.15)&_={StringExt.GetUnixExt()}";
            var httpInput = new KdyRequestCommonInput(url, HttpMethod.Post)
            {
                Referer = "https://sina.com.cn",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36 Edg/85.0.564.44",
                ExtData = new KdyRequestCommonExtInput()
                {
                    PostData = $"entry=sso&gateway=1&from=null&savestate=30&useticket=0&pagerefer=&vsnf=1&su={config.UserName.ToBase64Ext(Encoding.UTF8)}&service=sso&sp={config.UserPwd}&sr=1536*864&encoding=UTF-8&cdult=3&domain=sina.com.cn&prelt=0&returntype=TEXT"
                }
            };
            var httpResult = await _kdyRequestClientCommon.SendAsync(httpInput);
            if (httpResult.IsSuccess == false)
            {
                KdyLog.Warn("微博登录失败");
                return string.Empty;
            }

            if (httpResult.Data.Contains("nick") == false)
            {
                return string.Empty;
            }

            if (httpResult.CookieDic.ContainsKey("SUB"))
            {
                cacheVal = $"SUB={httpResult.CookieDic["SUB"]}";
            }

            KdyLog.Info("微博登录成功");
            await _kdyRedisCache.GetCache().SetStringAsync(KdyServiceCacheKey.WeiBoCookieKey, cacheVal, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(20)
            });
            return cacheVal;

        }
    }
}
