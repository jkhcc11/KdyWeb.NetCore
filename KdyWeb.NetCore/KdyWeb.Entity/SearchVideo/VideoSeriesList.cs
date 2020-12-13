using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 影片系列 列表
    /// </summary>
    public class VideoSeriesList : BaseEntity<long>
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
        /// 影片主键
        /// </summary>
        public int OldKeyId { get; set; }

        /// <summary>
        /// 系列
        /// </summary>
        public virtual VideoSeries VideoSeries { get; set; }

        /// <summary>
        /// 影片
        /// </summary>
        public virtual VideoMain VideoMain { get; set; }
    }
}
