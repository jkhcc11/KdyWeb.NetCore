using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.HttpCapture
{
    /// <summary>
    /// 循环Url配置
    /// </summary>
    public class RecurrentUrlConfig : BaseEntity<long>
    {
        /// <summary>
        /// Cron长度
        /// </summary>
        public const int UrlCronLength = 20;
        /// <summary>
        /// 成功标识长度
        /// </summary>
        public const int SuccessFlagLength = 20;
        /// <summary>
        /// post请求类型长度
        /// </summary>
        public const int ContentTypeLength = 50;

        /// <summary>
        /// 循环Url配置
        /// </summary>
        /// <param name="requestUrl">请求Url</param>
        /// <param name="httpMethod">请求方法</param>
        /// <param name="urlCron">循环周期</param>
        public RecurrentUrlConfig(string requestUrl,
            ConfigHttpMethod httpMethod, string urlCron)
        {
            RequestUrl = requestUrl;
            HttpMethod = httpMethod;
            UrlCron = urlCron;
            SearchConfigStatus = SearchConfigStatus.Normal;
        }

        /// <summary>
        /// 请求Url
        /// </summary>
        [StringLength(PageSearchConfig.OtherHostLength)]
        public string RequestUrl { get; protected set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public ConfigHttpMethod HttpMethod { get; protected set; }

        /// <summary>
        /// 来源
        /// </summary>
        [StringLength(PageSearchConfig.OtherHostLength)]
        public string? Referer { get; set; }

        /// <summary>
        /// post数据
        /// </summary>
        [StringLength(PageSearchConfig.SearchDataLength)]
        public string? PostData { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        public string? Cookie { get; set; }

        /// <summary>
        /// 提示信息提取
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        public string? MsgXpath { get; set; }

        /// <summary>
        /// 循环周期
        /// </summary>
        [StringLength(UrlCronLength)]
        public string UrlCron { get; protected set; }

        /// <summary>
        /// 成功标记
        /// </summary>
        [StringLength(SuccessFlagLength)]
        public string? SuccessFlag { get; set; }

        /// <summary>
        /// 配置状态
        /// </summary>
        public SearchConfigStatus SearchConfigStatus { get; set; }

        /// <summary>
        /// post请求类型
        /// </summary>
        [StringLength(ContentTypeLength)]
        public string? ContentType { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Ban()
        {
            SearchConfigStatus = SearchConfigStatus.Ban;
        }

        /// <summary>
        /// 获取任务唯一JobId
        /// </summary>
        /// <returns></returns>
        public string GetJobId()
        {
            return $"recurrent_{Id}";
        }

        /// <summary>
        /// 获取任务唯一JobId
        /// </summary>
        /// <returns></returns>
        public static string GetJobId(long id)
        {
            return $"recurrent_{id}";
        }
    }
}
