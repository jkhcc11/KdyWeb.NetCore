using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 豆瓣信息查询 Dto
    /// </summary>
    [AutoMap(typeof(DouBanInfo))]
    public class QueryDouBanInfoDto : CreatedUserDto<long>
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
        /// 影片类型Str
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
        /// 影片类型 武侠，动作等
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        public string VideoGenres { get; set; }

        /// <summary>
        /// 又名 
        /// </summary>
        /// <remarks>多个名称，逗号隔开</remarks>
        public string Aka { get; set; }

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
        /// 评分
        /// </summary>
        public double VideoRating { get; set; }

        /// <summary>
        /// 评分人数
        /// </summary>
        public int? RatingsCount { get; set; }
    }
}
