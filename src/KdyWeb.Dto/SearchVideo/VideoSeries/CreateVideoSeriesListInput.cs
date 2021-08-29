using System.Collections.Generic;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建影片系列列表 Input
    /// </summary>
    public class CreateVideoSeriesListInput
    {
        /// <summary>
        /// 影片系列Id
        /// </summary>
        public long SeriesId { get; set; }

        /// <summary>
        /// 影片Ids
        /// </summary>
        public List<long> VideoMainId { get; set; } = new List<long>();
    }
}
