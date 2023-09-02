using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using KdyWeb.BaseInterface.HttpBase;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;

namespace KdyWeb.CommonService.KdyHttp
{
    /// <summary>
    /// 通用Http请求 实现
    /// </summary>
    /// <remarks>
    /// 基于HttpClient
    /// </remarks>
    public class KdyRequestClientCommon : BaseKdyHttpClient<KdyRequestCommonResult, string, KdyRequestCommonInput, KdyRequestCommonExtInput>, IKdyRequestClientCommon
    {
        public KdyRequestClientCommon(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public override HttpRequestMessage RequestPar(KdyRequestCommonInput input)
        {
            var req = new HttpRequestMessage(input.Method, input.Url);
            if (input.ExtData == null)
            {
                return req;
            }

            if (string.IsNullOrEmpty(input.ExtData.ContentType) == false &&
                string.IsNullOrEmpty(input.ExtData.PostData) == false)
            {
                //组装post内容
                var postContent = new StringContent(input.ExtData.PostData);
                postContent.Headers.ContentType = new MediaTypeHeaderValue(input.ExtData.ContentType);

                req.Content = postContent;
            }
            else if (input.ExtData.FileBytes != null)
            {
                #region post文件
                var multiContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(input.ExtData.FileBytes);
                multiContent.Add(fileContent, input.ExtData.NameField, input.ExtData.FileName);

                if (input.ExtData.PostParDic != null && input.ExtData.PostParDic.Any())
                {
                    foreach (var key in input.ExtData.PostParDic)
                    {
                        multiContent.Add(new StringContent(key.Value), key.Key);
                    }
                }

                req.Content = multiContent;
                #endregion
            }

            if (input.ExtData.IsAjax)
            {
                req.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            if (input.ExtData.HeardDic != null &&
                input.ExtData.HeardDic.Any())
            {
                foreach (var headItem in input.ExtData.HeardDic)
                {
                    req.Headers.Add(headItem.Key, headItem.Value);
                    SpecialReqHeadHandler(headItem, req);
                }
            }

            return req;
        }

        /// <summary>
        /// 有些站点强制校验 不能有空格
        /// https://stackoverflow.com/questions/45194549/httpclient-httprequestmessage-accept-header-parameters-cannot-have-spaces
        /// https://github.com/dotnet/runtime/issues/21131
        /// </summary>
        internal void SpecialReqHeadHandler(KeyValuePair<string, string> inputKey, HttpRequestMessage req)
        {
            if (inputKey.Key.ToLower() != "accept")
            {
                return;
            }

            var tempMedia = MediaTypeWithQualityHeaderValue.Parse(inputKey.Value);
            foreach (var v in req.Headers.Accept)
            {
                if (v.MediaType.Contains(tempMedia.MediaType) == false)
                {
                    return;
                }

                //反射更改
                var field = v.GetType().GetTypeInfo()?.BaseType?.GetField("_mediaType", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null)
                {
                    return;
                }

                field.SetValue(v, inputKey.Value);
                v.Parameters.Clear();
            }
        }
    }
}
