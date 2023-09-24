using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse.Enum;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 获取访问前N条文件信息
    /// </summary>
    public class GetTopFileInfoInput : BaseDateRangeInput
    {
        /// <summary>
        /// 记录历史类型
        /// </summary>
        [EnumDataType(typeof(RecordHistoryType), ErrorMessage = "类型错误")]
        public RecordHistoryType? RecordHistoryType { get; set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        public long? SubAccountId { get; set; }

        /// <summary>
        /// 前N
        /// </summary>
        public int Top { get; set; } = 10;
    }
}
