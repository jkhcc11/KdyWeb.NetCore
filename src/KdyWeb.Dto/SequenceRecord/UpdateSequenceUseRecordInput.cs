using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.SequenceRecord.Enum;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 更新接龙使用记录
    /// </summary>
    public class UpdateSequenceUseRecordInput
    {
        /// <summary>
        /// Key
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 当天结算价格
        /// </summary>
        [Range(0, 999)]
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// 奖励用户类型
        /// </summary>
        /// <remarks>
        /// 如果有奖励用户
        /// </remarks>
        [EnumDataType(typeof(GiftUserTypeEnum))]
        public GiftUserTypeEnum? GiftUserType { get; set; }
    }
}
