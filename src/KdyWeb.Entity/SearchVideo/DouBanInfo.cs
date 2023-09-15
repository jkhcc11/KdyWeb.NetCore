using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 豆瓣信息
    /// </summary>
    public class DouBanInfo : BaseEntity<int>
    {
        #region 常量
        /// <summary>
        /// 豆瓣详情Id
        /// </summary>
        public const int VideoDetailIdLength = 10;

        /// <summary>
        /// 名称长度
        /// </summary>
        public const int VideoTitleLength = 100;

        /// <summary>
        /// 海报长度
        /// </summary>
        public const int VideoImgLength = 200;

        /// <summary>
        /// 主演长度
        /// </summary>
        public const int VideoCastsLength = 200;

        /// <summary>
        /// 导演长度
        /// </summary>
        public const int VideoDirectorsLength = 200;

        /// <summary>
        /// 影片类型长度
        /// </summary>
        public const int VideoGenresLength = 200;

        /// <summary>
        /// 国家长度
        /// </summary>
        public const int VideoCountriesLength = 200;

        /// <summary>
        /// 又名 长度
        /// </summary>
        public const int AkaLength = 100;

        /// <summary>
        /// ImdbUrl长度
        /// </summary>
        public const int ImdbStrLength = 100;
        #endregion

        public DouBanInfo(string videoTitle)
        {
            VideoTitle = videoTitle;
            DouBanInfoStatus = DouBanInfoStatus.SearchWait;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(VideoTitleLength)]
        [Required]
        public string VideoTitle { get; set; }

        /// <summary>
        /// 豆瓣信息状态
        /// </summary>
        public DouBanInfoStatus DouBanInfoStatus { get; protected set; }

        /// <summary>
        /// 待搜索状态
        /// </summary>
        public string? OldStatus { get; set; }

        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 影片类型 电视或电影
        /// </summary>
        public string? OldVideoType { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        [StringLength(VideoImgLength)]
        [Required]
        public string? VideoImg { get; set; }

        /// <summary>
        /// 主演
        /// </summary>
        [StringLength(VideoCastsLength)]
        public string? VideoCasts { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        [StringLength(VideoDirectorsLength)]
        public string? VideoDirectors { get; set; }

        /// <summary>
        /// 影片类型 武侠，动作等
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        [StringLength(VideoGenresLength)]
        public string? VideoGenres { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public double VideoRating { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? VideoSummary { get; set; }

        /// <summary>
        /// 详情Id
        /// </summary>
        [StringLength(VideoDetailIdLength)]
        [Required]
        public string? VideoDetailId { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        [StringLength(VideoCountriesLength)]
        public string? VideoCountries { get; set; }

        /// <summary>
        /// 评分人数
        /// </summary>
        public int? RatingsCount { get; set; }

        /// <summary>
        /// 评论人数
        /// </summary>
        public int? CommentsCount { get; set; }

        /// <summary>
        /// 影评人数
        /// </summary>
        public int? ReviewsCount { get; set; }

        /// <summary>
        /// 又名 
        /// </summary>
        /// <remarks>多个名称，逗号隔开</remarks>
        [StringLength(AkaLength)]
        public string? Aka { get; set; }

        /// <summary>
        /// Imdb Url
        /// </summary>
        [StringLength(VideoImgLength)]
        public string? ImdbStr { get; set; }

        /// <summary>
        /// 设置状态
        /// </summary>
        public void SetStatus(DouBanInfoStatus douBanInfoStatus)
        {
            DouBanInfoStatus = douBanInfoStatus;
        }

        public void SetSearchEnd()
        {
            DouBanInfoStatus = DouBanInfoStatus.SearchEnd;
        }

        public void SetSearchFail()
        {
            DouBanInfoStatus = DouBanInfoStatus.SearchFail;
        }

        public void SetSearching()
        {
            DouBanInfoStatus = DouBanInfoStatus.Searching;
        }
    }
}
