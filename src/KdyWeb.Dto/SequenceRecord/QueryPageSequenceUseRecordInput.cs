using System;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页获取接龙使用记录
    /// </summary>
    public class QueryPageSequenceUseRecordInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }

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
