using System;
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
    public class KdyRequestCommon : BaseKdyHttp<KdyRequestCommonResult, string, KdyRequestCommonInput, KdyRequestCommonExtInput>, IKdyRequestCommon
    {
        public KdyRequestCommon(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
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

            if (input.ExtData.IsAjax)
            {
                req.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            return req;

        }
    }
}
