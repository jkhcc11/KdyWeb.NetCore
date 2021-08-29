using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建影片系列 Input
    /// </summary>
    [AutoMap(typeof(VideoSeries), ReverseMap = true)]
    public class CreateVideoSeriesInput : BaseVideoSeriesInput
    {

        /// <summary>
        /// 系列列表
        /// </summary>
        public List<VideoSeriesListItem> SeriesList { get; set; }
    }

    /// <summary>
    /// 影片系列 列表
    /// </summary>
    public class VideoSeriesListItem
    {
        /// <summary>
        /// 影片Id
        /// </summary>
        public long KeyId { get; set; }
    }

    /// <summary>
    /// 影片系列Input 基类
    /// </summary>
    public class BaseVideoSeriesInput
    {
        /// <summary>
        /// 系列名
        /// </summary>
        [StringLength(VideoSeries.SeriesNameLength)]
        [Required]
        public string SeriesName { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        [StringLength(VideoSeries.SeriesImgLength)]
        public string SeriesImg { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(VideoSeries.SeriesRemarkLength)]
        public string SeriesRemark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 直播Url
        /// </summary>
        [StringLength(VideoSeries.LiveUrlLength)]
        public string LiveUrl { get; set; }

        /// <summary>
        /// 系列简介url
        /// </summary>
        [StringLength(VideoSeries.SeriesDesUrlLength)]
        public string SeriesDesUrl { get; set; }
    }
}
