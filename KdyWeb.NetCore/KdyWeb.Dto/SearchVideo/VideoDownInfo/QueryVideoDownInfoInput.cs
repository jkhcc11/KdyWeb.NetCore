using System;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 查询下载地址 Input
    /// </summary>
    public class QueryVideoDownInfoInput : BasePageInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [KdyQuery(nameof(VideoDownInfo.CreatedTime), KdyOperator.GtEqual)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [KdyQuery(nameof(VideoDownInfo.CreatedTime), KdyOperator.LessEqual)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [KdyQuery(nameof(VideoDownInfo.VideoYear), KdyOperator.Equal)]
        public int? Year { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(VideoDownInfo.VideoName), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
