using System.ComponentModel.DataAnnotations;
using AutoMapper;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 初始化循环UrlJob Input
    /// </summary>
    [AutoMap(typeof(RecurrentUrlConfig))]
    public class RecurrentUrlJobInput
    {
        /// <summary>
        /// 请求Url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public ConfigHttpMethod HttpMethod { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// post数据
        /// </summary>
        public string PostData { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// 提示信息提取
        /// </summary>
        public string MsgXpath { get; set; }

        /// <summary>
        /// 成功标记
        /// </summary>
        public string SuccessFlag { get; set; }
    }
}
