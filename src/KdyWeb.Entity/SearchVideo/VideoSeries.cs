using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 影片系列
    /// </summary>
    public class VideoSeries : BaseEntity<long>
    {
        /// <summary>
        /// 系列名长度
        /// </summary>
        public const int SeriesNameLength = 50;
        /// <summary>
        /// 海报长度
        /// </summary>
        public const int SeriesImgLength = 200;
        /// <summary>
        /// 备注长度
        /// </summary>
        public const int SeriesRemarkLength = 500;
        /// <summary>
        /// 直播Url长度
        /// </summary>
        public const int LiveUrlLength = 200;
        /// <summary>
        /// 系列简介url长度
        /// </summary>
        public const int SeriesDesUrlLength = 200;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="seriesName">系列名</param>
        /// <param name="seriesImg">海报</param>
        public VideoSeries(string seriesName, string seriesImg)
        {
            SeriesName = seriesName;
            SeriesImg = seriesImg;
        }

        /// <summary>
        /// 系列名
        /// </summary>
        [StringLength(SeriesNameLength)]
        [Required]
        public string SeriesName { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        [StringLength(SeriesImgLength)]
        public string SeriesImg { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(SeriesRemarkLength)]
        public string SeriesRemark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 直播Url
        /// </summary>
        [StringLength(LiveUrlLength)]
        public string LiveUrl { get; set; }

        /// <summary>
        /// 系列简介url
        /// </summary>
        [StringLength(SeriesDesUrlLength)]
        public string SeriesDesUrl { get; set; }

        /// <summary>
        /// 系列列表
        /// </summary>
        public virtual ICollection<VideoSeriesList> SeriesList { get; set; }
    }
}
