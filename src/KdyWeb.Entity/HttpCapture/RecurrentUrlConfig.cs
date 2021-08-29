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
        /// 请求Url
        /// </summary>
        [StringLength(PageSearchConfig.OtherHostLength)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public ConfigHttpMethod HttpMethod { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [StringLength(PageSearchConfig.OtherHostLength)]
        public string Referer { get; set; }

        /// <summary>
        /// post数据
        /// </summary>
        [StringLength(PageSearchConfig.SearchDataLength)]
        public string PostData { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// 提示信息提取
        /// </summary>
        [StringLength(PageSearchConfig.XpathLength)]
        public string MsgXpath { get; set; }

        /// <summary>
        /// 循环周期
        /// </summary>
        [StringLength(UrlCronLength)]
        public string UrlCron { get; set; }

        /// <summary>
        /// 成功标记
        /// </summary>
        [StringLength(SuccessFlagLength)]
        public string SuccessFlag { get; set; }

        /// <summary>
        /// 配置状态
        /// </summary>
        public SearchConfigStatus SearchConfigStatus { get; set; }

        /// <summary>
        /// post请求类型
        /// </summary>
        [StringLength(ContentTypeLength)]
        public string ContentType { get; set; }
    }
}
