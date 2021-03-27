using System;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{

    /// <summary>
    /// 查询视频播放记录 Input
    /// </summary>
    public class QueryVideoHistoryInput : BasePageInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [KdyQuery(nameof(VideoHistory.CreatedTime), KdyOperator.GtEqual)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [KdyQuery(nameof(VideoHistory.CreatedTime), KdyOperator.LessEqual)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype? Subtype { get; set; }
    }
}
