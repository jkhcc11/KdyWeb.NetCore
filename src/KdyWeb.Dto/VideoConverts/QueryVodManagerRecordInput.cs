using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询影片管理者记录 input
    /// </summary>
    public class QueryVodManagerRecordInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(VodManagerRecord.CreatedUserName), KdyOperator.Equal)]
        [KdyQuery(nameof(VodManagerRecord.Remark), KdyOperator.Like)]
        public string KeyWord { get; set; }

        /// <summary>
        /// 记录类型
        /// </summary>
        [KdyQuery(nameof(VodManagerRecord.RecordType), KdyOperator.Equal)]
        public VodManagerRecordType? RecordType { get; set; }

        /// <summary>
        /// 是否已结算
        /// </summary>
        [KdyQuery(nameof(VodManagerRecord.IsCheckout), KdyOperator.Equal)]
        public bool? IsCheckout { get; set; }

        /// <summary>
        /// 是否为有效记录
        /// </summary>
        [KdyQuery(nameof(VodManagerRecord.IsValid), KdyOperator.Equal)]
        public bool? IsValid { get; set; }
    }
}
