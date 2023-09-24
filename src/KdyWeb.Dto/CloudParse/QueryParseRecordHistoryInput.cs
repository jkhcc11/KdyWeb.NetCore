using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询记录
    /// </summary>
    public class QueryParseRecordHistoryInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(ParseRecordHistory.FileIdOrFileName), KdyOperator.Like)]
        public string KeyWord { get; set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        public long? SubAccountId { get; set; }

        /// <summary>
        /// 记录历史类型
        /// </summary>
        [EnumDataType(typeof(RecordHistoryType), ErrorMessage = "类型错误")]
        public RecordHistoryType? RecordHistoryType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public virtual (DateTime startTime, DateTime endTime) GetRange()
        {
            var startTime = StartTime.HasValue == false ?
                DateTime.Now.Date.AddDays(-7) : StartTime.Value;

            var endTime = EndTime.HasValue == false ?
                DateTime.Now.Date.AddDays(1) :
                EndTime.Value.AddDays(1);
            if ((endTime - startTime).TotalDays > 366)
            {
                //最多一年
                endTime = startTime.AddDays(366);
            }

            return (startTime, endTime);
        }
    }
}
