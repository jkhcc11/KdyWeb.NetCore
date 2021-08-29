using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧 系列
    /// </summary>
    public class OldSearchSysSeries : BaseEntity<int>
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
