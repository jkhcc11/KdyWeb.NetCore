using System;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 弹幕搜索 Input
    /// </summary>
    public class SearchDanMuInput : BasePageInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [KdyQuery(nameof(VideoMain.CreatedTime), KdyOperator.GtEqual)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [KdyQuery(nameof(VideoMain.CreatedTime), KdyOperator.LessEqual)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(VideoDanMu.Msg), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
