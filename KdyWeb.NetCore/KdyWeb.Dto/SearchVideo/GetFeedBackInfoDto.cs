using AutoMapper;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.Utility;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 分页获取反馈信息 Dto
    /// </summary>
    [AutoMap(typeof(FeedBackInfo))]
    public class GetFeedBackInfoDto
    {
        /// <summary>
        /// 反馈类型
        /// </summary>
        public FeedBackInfoStatus FeedBackInfoStatus { get; set; }

        /// <summary>
        /// 反馈类型Str
        /// </summary>
        public string FeedBackInfoStatusStr => FeedBackInfoStatus.GetDisplayName();

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 1、反馈 -> 站点Url 发送邮件使用
        /// 2、录入 ->豆瓣Url、搜狗详情、百度百科等
        /// </remarks>
        public string OriginalUrl { get; set; }

        /// <summary>
        /// 反馈类型
        /// </summary>
        public UserDemandType DemandType { get; set; }

        /// <summary>
        /// 反馈类型Str
        /// </summary>
        public string DemandTypeStr => DemandType.GetDisplayName();

        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 用户Email
        /// </summary>
        public string UserEmail { get; set; }
    }
}
