using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.KdyFile;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.KdyFile;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.KdyFile
{
    /// <summary>
    /// 普通文件上传 实现
    /// </summary>
    public class NormalFileService : BaseKdyFileService<NormalFileInput>, INormalFileService
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        public NormalFileService(IHttpClientFactory httpClientFactory, IKdyRequestClientCommon kdyRequestClientCommon) : base(httpClientFactory)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        public override async Task<KdyResult<KdyFileDto>> PostFileByBytes(NormalFileInput input)
        {
            var result = KdyResult.Error<KdyFileDto>(KdyResultCode.Error, "普通文件上传失败");
            var httpInput = new KdyRequestCommonInput(input.BaseUrl, HttpMethod.Post)
            {
                Cookie = input.Cookie,
                Referer = input.Referer,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36 Edg/85.0.564.44",
                ExtData = new KdyRequestCommonExtInput()
                {
                    FileBytes = input.FileBytes,
                    PostParDic = input.PostParDic,
                    NameField = input.NameField,
                    FileName = input.FileName
                },
                TimeOut = 15
            };
            var httpResult = await _kdyRequestClientCommon.SendAsync(httpInput);
            if (httpResult.IsSuccess == false ||
                httpResult.HttpCode == HttpStatusCode.Found ||
                httpResult.HttpCode == HttpStatusCode.BadRequest)
            {
                result.Msg = httpResult.ErrMsg;
                KdyLog.Warn("普通文件上传失败", new Dictionary<string, object>()
                {
                    {"PostInputPar",input }
                }, $"{this}");
                return result;
            }

            var jsonStr = httpResult.Data;
            if (input.CallBackRule.IsEmptyExt() == false)
            {
                //跨域提取
                var callBackJson = httpResult.Data.GetStrMathExt($"{input.CallBackRule}\\(", "\\)");
                if (string.IsNullOrEmpty(callBackJson))
                {
                    return result;
                }

                jsonStr = callBackJson;
            }

            var uploadResult = new KdyFileDto();
            string resultImgUrl;
            try
            {
                var jObject = JObject.Parse(jsonStr);
                resultImgUrl = $"{jObject.SelectToken(input.JsonRule)}";
                if (resultImgUrl.IsEmptyExt())
                {
                    result.Msg = $"普通文件上传失败，提取Json失败";
                    return result;
                }
            }
            catch (JsonReaderException e)
            {
                //提取失败默认返回结果
                resultImgUrl = jsonStr.Replace("\r", "")
                    .Replace("\n", "")
                    .Replace(" ", "");
            }

            if (input.BaseHost.IsEmptyExt() == false)
            {
                //有额外Host
                uploadResult.Url = $"{input.BaseHost}/{resultImgUrl.TrimStart('/')}";
                result = KdyResult.Success(uploadResult);
                return result;
            }

            if (resultImgUrl.StartsWith("http://") == false && 
                resultImgUrl.StartsWith("https://") == false)
            {
                //自动url拼接
                uploadResult.Url = $"https://{resultImgUrl}";
            }
            else
            {
                uploadResult.Url = resultImgUrl;
            }

            result = KdyResult.Success(uploadResult);
            return result;
        }
    }
}
