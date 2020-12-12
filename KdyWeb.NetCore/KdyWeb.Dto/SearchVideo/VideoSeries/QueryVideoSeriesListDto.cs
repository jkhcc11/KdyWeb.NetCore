using System.Collections.Generic;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 查询影片系列列表 Dto
    /// </summary>
    [AutoMap(typeof(VideoSeriesList))]
    public class QueryVideoSeriesListDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 影片系列Id
        /// </summary>
        public long SeriesId { get; set; }

        /// <summary>
        /// 影片Id
        /// </summary>
        public long KeyId { get; set; }

        /// <summary>
        /// 影片
        /// </summary>
        public QueryVideoMainDto VideoMain { get; set; }
    }
}
