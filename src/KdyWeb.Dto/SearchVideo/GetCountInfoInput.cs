using System;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 获取反馈统计信息 input
    /// </summary>
    public class GetCountInfoInput
    {
        /// <summary>
        /// 反馈类型
        /// </summary>
        public FeedBackInfoStatus? FeedBackInfoStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
