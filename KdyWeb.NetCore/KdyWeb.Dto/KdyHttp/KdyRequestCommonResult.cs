using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.KdyHttp
{
    /// <summary>
    /// 通用Http请求 结果
    /// </summary>
    public class KdyRequestCommonResult : IHttpOut<string>
    {
        public bool IsSuccess { get; set; }

        public string Cookie { get; set; }

        public HttpStatusCode? HttpCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public string Data { get; set; }
    }
}
