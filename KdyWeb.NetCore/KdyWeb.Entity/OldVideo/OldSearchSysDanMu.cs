using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧 视频弹幕
    /// </summary>
    public class OldSearchSysDanMu : BaseEntity<int>
    {
        /// <summary>
        /// 弹幕时间节点
        /// </summary>
        public float DTime { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string DColor { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string DMsg { get; set; }

        /// <summary>
        /// 旧剧集Id
        /// </summary>
        public string DVideoId { get; set; }

        /// <summary>
        /// 弹幕形式
        /// </summary>
        public int DMode { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public int DSize { get; set; }

        public bool Deleted { get; set; }
    }
}
