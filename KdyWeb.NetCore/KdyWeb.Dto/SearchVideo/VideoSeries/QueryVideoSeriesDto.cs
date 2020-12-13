using System.Collections.Generic;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 查询影片系列 Dto
    /// </summary>
    [AutoMap(typeof(VideoSeries))]
    public class QueryVideoSeriesDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 系列名
        /// </summary>
        public string SeriesName { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string SeriesImg { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string SeriesRemark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 直播Url
        /// </summary>
        public string LiveUrl { get; set; }

        /// <summary>
        /// 系列简介url
        /// </summary>
        public string SeriesDesUrl { get; set; }
    }
}
