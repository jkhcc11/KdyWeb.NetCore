using System;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 获取影片统计信息 Input
    /// </summary>
    public class GetCountInfoBySubtypeInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
