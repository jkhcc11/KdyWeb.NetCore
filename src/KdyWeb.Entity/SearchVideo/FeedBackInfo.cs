using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 反馈信息及录入
    /// </summary>
    public class FeedBackInfo : BaseEntity<int>
    {
        #region 常量
        /// <summary>
        /// 备注长度
        /// </summary>
        public const int RemarkLength = 500;

        /// <summary>
        /// 源Url长度
        /// </summary>
        public const int OriginalUrlLength = 200;

        /// <summary>
        /// 视频长度
        /// </summary>
        public const int VideoNameLength = 100;

        /// <summary>
        /// Email长度
        /// </summary>
        public const int UserEmailLength = 100;
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="demandType">反馈类型</param>
        /// <param name="originalUrl">源Url</param>
        public FeedBackInfo(UserDemandType demandType, string originalUrl)
        {
            OriginalUrl = originalUrl;
            DemandType = demandType;
            FeedBackInfoStatus = FeedBackInfoStatus.Pending;
        }

        /// <summary>
        /// 反馈类型
        /// </summary>
        public FeedBackInfoStatus FeedBackInfoStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(RemarkLength)]
        public string Remark { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 1、反馈 -> 站点Url 发送邮件使用
        /// 2、录入 ->豆瓣Url、搜狗详情、百度百科等
        /// </remarks>
        [StringLength(OriginalUrlLength)]
        public string OriginalUrl { get; set; }

        /// <summary>
        /// 反馈类型
        /// </summary>
        public UserDemandType DemandType { get; set; }

        #region 冗余
        /// <summary>
        /// 视频名称
        /// </summary>
        [StringLength(VideoNameLength)]
        public string VideoName { get; set; }

        /// <summary>
        /// 用户Email
        /// </summary>
        [StringLength(UserEmailLength)]
        public string UserEmail { get; set; }
        #endregion

    }
}
