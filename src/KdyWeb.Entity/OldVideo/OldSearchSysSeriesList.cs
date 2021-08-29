using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧 系列列表
    /// </summary>
    public class OldSearchSysSeriesList : BaseEntity<int>
    {
        /// <summary>
        /// 剧集Id
        /// </summary>
        public int SeriesId { get; set; }

        /// <summary>
        /// 影片主键
        /// </summary>
        public int KeyId { get; set; }
    }
}
