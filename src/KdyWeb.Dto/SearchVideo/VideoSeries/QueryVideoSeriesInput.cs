using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 查询影片系列 Input
    /// </summary>
    public class QueryVideoSeriesInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(VideoSeries.SeriesName), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
