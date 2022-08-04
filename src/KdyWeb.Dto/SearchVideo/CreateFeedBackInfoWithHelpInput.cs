using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建求片反馈 入参
    /// </summary>
    public class CreateFeedBackInfoWithHelpInput
    {
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(FeedBackInfo.RemarkLength)]
        public string Remark { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 1、反馈 -> 站点Url 发送邮件使用
        /// 2、录入 ->豆瓣Url、搜狗详情、百度百科等
        /// </remarks>
        [StringLength(FeedBackInfo.OriginalUrlLength)]
        [Required(ErrorMessage = "Url必填")]
        public string OriginalUrl { get; set; }
    }
}
