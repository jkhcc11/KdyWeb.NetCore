using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建豆瓣信息Dto
    /// </summary>
    [AutoMap(typeof(DouBanInfo))]
    public class GetDouBanInfoForIdDto : CreatedUserDto<int>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string VideoTitle { get; set; }

        /// <summary>
        /// 豆瓣信息状态
        /// </summary>
        public DouBanInfoStatus DouBanInfoStatus { get; set; }

        /// <summary>
        /// 豆瓣信息状态 Str
        /// </summary>
        public string DouBanInfoStatusStr => DouBanInfoStatus.GetDisplayName();

        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 影片类型 Str
        /// </summary>
        public string SubtypeStr => Subtype.GetDisplayName();

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string VideoImg { get; set; }

        /// <summary>
        /// 主演
        /// </summary>
        public string VideoCasts { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        public string VideoDirectors { get; set; }

        /// <summary>
        /// 影片类型 武侠，动作等
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        public string VideoGenres { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public double VideoRating { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string VideoSummary { get; set; }

        /// <summary>
        /// 详情Id
        /// </summary>
        public string VideoDetailId { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        public string VideoCountries { get; set; }

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
        public string Aka { get; set; }

        /// <summary>
        /// Imdb Url
        /// </summary>
        public string ImdbStr { get; set; }
    }
}
