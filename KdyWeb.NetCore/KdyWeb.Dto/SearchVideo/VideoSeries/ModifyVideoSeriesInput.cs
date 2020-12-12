namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 修改影片系列 Input
    /// </summary>
    public class ModifyVideoSeriesInput : BaseVideoSeriesInput
    {
        /// <summary>
        /// 剧集Id
        /// </summary>
        public long SeriesId { get; set; }
    }
}
