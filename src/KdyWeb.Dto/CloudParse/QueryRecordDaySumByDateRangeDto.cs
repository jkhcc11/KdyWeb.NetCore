using System;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询时间范围内记录按天汇总
    /// </summary>
    public class QueryRecordDaySumByDateRangeDto
    {
        /// <summary>
        /// 访问量
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
    }
}
