using System;
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
    /// 基于WebRequest
    /// </remarks>
    public class KdyRequestCommon : BaseKdyHttpWebRequest<KdyRequestCommonResult, string, KdyRequestCommonInput, KdyRequestCommonExtInput>, IKdyRequestCommon
    {
        protected override void SetRequest(HttpWebRequest request, KdyRequestCommonInput input)
        {
            if (input.ExtData == null)
            {
                return;
            }

            request.Method = input.Method.ToString();
            if (input.Method == HttpMethod.Post)
            {
             
                if (string.IsNullOrEmpty(input.ExtData.PostData) == false)
                {
                    request.ContentType = input.ExtData.ContentType;
                    byte[] bs = input.EnCoding.GetBytes(input.ExtData.PostData);
                    request.ContentLength = bs.Length;
                    using var reqStream = request.GetRequestStream();
                    reqStream.Write(bs, 0, bs.Length);
                }
            }

            if (input.ExtData.IsAjax)
            {
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

        }
    }
}
