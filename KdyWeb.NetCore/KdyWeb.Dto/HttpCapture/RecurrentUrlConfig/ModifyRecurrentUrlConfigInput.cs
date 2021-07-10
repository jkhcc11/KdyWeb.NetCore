using System.ComponentModel.DataAnnotations;
using AutoMapper;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 修改循环Url配置 Input
    /// </summary>
    public class ModifyRecurrentUrlConfigInput : BaseRecurrentUrlConfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// 循环Url配置基类Input
    /// </summary>
    public class BaseRecurrentUrlConfig
    {
        /// <summary>
        /// 请求Url
        /// </summary>
        [StringLength(PageSearchConfig.OtherHostLength)]
        [Required(ErrorMessage = "请求Url必填")]
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
        [StringLength(RecurrentUrlConfig.UrlCronLength)]
        [Required(ErrorMessage = "Cron表达式必填")]
        public string UrlCron { get; set; }

        /// <summary>
        /// 成功标记
        /// </summary>
        [StringLength(RecurrentUrlConfig.SuccessFlagLength)]
        public string SuccessFlag { get; set; }

        /// <summary>
        /// 配置状态
        /// </summary>
        [Required(ErrorMessage = "配置状态必填")]
        public SearchConfigStatus SearchConfigStatus { get; set; }

        /// <summary>
        /// post请求类型
        /// </summary>
        [StringLength(RecurrentUrlConfig.ContentTypeLength)]
        public string ContentType { get; set; }
    }

    /// <summary>
    /// 修改循环Url配置 Profile
    /// </summary>
    public class ModifyRecurrentUrlConfigInputProfile : Profile
    {
        public ModifyRecurrentUrlConfigInputProfile()
        {
            CreateMap<ModifyRecurrentUrlConfigInput, RecurrentUrlConfig>()
                .ForMember(a => a.Id, b => b.Ignore());
        }
    }
}
