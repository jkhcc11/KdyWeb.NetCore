using System.ComponentModel.DataAnnotations;
using AutoMapper;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建弹幕 Input
    /// </summary>
    [AutoMap(typeof(VideoDanMu), ReverseMap = true)]
    public class CreateDanMuInput
    {
        /// <summary>
        /// 视频时间点
        /// </summary>
        public float DTime { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [StringLength(VideoDanMu.DColorLength)]
        public string DColor { get; set; }

        /// <summary>
        /// 弹幕内容
        /// </summary>
        [StringLength(VideoDanMu.MsgLength)]
        [Required(ErrorMessage = "弹幕内容不能为空")]
        public string Msg { get; set; }

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
