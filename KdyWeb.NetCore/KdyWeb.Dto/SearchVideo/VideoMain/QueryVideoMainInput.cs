using System;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 分页查询影视库 Input
    /// </summary>
    public class QueryVideoMainInput : BasePageInput
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
        /// 影片类型
        /// </summary>
        [KdyQuery(nameof(VideoMain.Subtype), KdyOperator.Equal)]
        public Subtype? Subtype { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [KdyQuery(nameof(VideoMain.VideoYear), KdyOperator.Equal)]
        public int? Year { get; set; }

        /// <summary>
        /// 类型 动作，喜剧等
        /// </summary>
        [KdyQuery("VideoMainInfo.VideoGenres", KdyOperator.Like)]
        public string Genres { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(VideoMain.KeyWord), KdyOperator.Like)]
        [KdyQuery(nameof(VideoMain.Aka), KdyOperator.Like)]
        [KdyQuery(nameof(VideoMain.SourceUrl), KdyOperator.StartsWith)]
        public string KeyWord { get; set; }
    }
}
