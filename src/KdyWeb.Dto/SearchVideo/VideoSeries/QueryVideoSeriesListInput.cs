using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 查询影片系列列表 Input
    /// </summary>
    public class QueryVideoSeriesListInput : BasePageInput
    {
        /// <summary>
        /// 系列Id
        /// </summary>
        [KdyQuery(nameof(VideoSeriesList.SeriesId), KdyOperator.Equal)]
        public long? SeriesId { get; set; }
    }
}
