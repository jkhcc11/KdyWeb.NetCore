using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 视频弹幕
    /// </summary>
    public class VideoDanMu : BaseEntity<long>
    {
        /// <summary>
        /// 颜色长度
        /// </summary>
        public const int DColorLength = 10;
        /// <summary>
        /// 弹幕内容长度
        /// </summary>
        public const int MsgLength = 200;

        /// <summary>
        /// 视频时间点
        /// </summary>
        public float DTime { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [StringLength(DColorLength)]
        public string? DColor { get; set; }

        /// <summary>
        /// 弹幕内容
        /// </summary>
        [StringLength(MsgLength)]
        public string? Msg { get; set; }

        /// <summary>
        /// 剧集Id
        /// </summary>
        public long EpId { get; set; }

        /// <summary>
        /// 弹幕模式
        /// </summary>
        public int DMode { get; set; }

        /// <summary>
        ///弹幕文字大小
        /// </summary>
        public int DSize { get; set; }
    }
}
