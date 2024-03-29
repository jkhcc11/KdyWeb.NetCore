﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.HttpBase;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.KdyHttp;

namespace KdyWeb.Service.KdyHttp
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

            return req;
        }
    }
}
