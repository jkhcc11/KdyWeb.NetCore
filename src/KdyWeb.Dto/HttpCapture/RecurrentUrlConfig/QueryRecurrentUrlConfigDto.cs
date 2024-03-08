using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 查询循环Job Dto
    /// </summary>
    [AutoMap(typeof(RecurrentUrlConfig))]
    public class QueryRecurrentUrlConfigDto : CreatedUserDto<long>
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
        /// 循环周期
        /// </summary>
        public string UrlCron { get; set; }

        /// <summary>
        /// 成功标记
        /// </summary>
        public string SuccessFlag { get; set; }

        /// <summary>
        /// 配置状态
        /// </summary>
        public SearchConfigStatus SearchConfigStatus { get; set; }

        /// <summary>
        /// post请求类型
        /// </summary>
        public string ContentType { get; set; }
    }
}
