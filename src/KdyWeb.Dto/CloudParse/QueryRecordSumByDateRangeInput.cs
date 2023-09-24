using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询时间范围内记录按天汇总
    /// </summary>
    public class QueryRecordSumByDateRangeInput : BaseDateRangeInput
    {
        /// <summary>
        /// 记录历史类型
        /// </summary>
        [EnumDataType(typeof(RecordHistoryType), ErrorMessage = "类型错误")]
        public RecordHistoryType? RecordHistoryType { get; set; }
    }
}
