using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KdyWeb.BaseInterface.BaseModel;
using Newtonsoft.Json;

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

        public Dictionary<string, string> CookieDic { get; set; } = new Dictionary<string, string>();

        public string LocationUrl { get; set; }

        public string GetString()
        {
            var str = JsonConvert.SerializeObject(this);
            if (str.Length > 1024)
            {
                return str.Substring(0, 1024);
            }

            return str;
        }
    }
}
