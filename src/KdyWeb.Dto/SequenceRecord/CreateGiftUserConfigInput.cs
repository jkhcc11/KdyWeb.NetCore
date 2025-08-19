using KdyWeb.Entity.SequenceRecord;
using KdyWeb.Entity.SequenceRecord.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 创建奖励用户配置
    /// </summary>
    public class CreateGiftUserConfigInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        [StringLength(SequenceUserRecord.UserIdLength)]
        public string UserId { get; set; }

        /// <summary>
        /// 奖励用户类型
        /// </summary>
        [EnumDataType(typeof(GiftUserTypeEnum))]
        public GiftUserTypeEnum GiftUserType { get; set; }

        /// <summary>
        /// 奖励开始时间
        /// </summary>
        public DateTime GiftStartTime { get; set; }

        /// <summary>
        /// 奖励结束时间
        /// </summary>
        public DateTime GiftEndTime { get; set; }

        /// <summary>
        /// 奖励价格
        /// </summary>
        public decimal GiftPrice { get; set; }

        /// <summary>
        /// 场馆Id
        /// </summary>
        /// <remarks>
        /// 奖励绑定场馆使用
        /// </remarks>
        public long VenuesId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(GiftUserConfig.GiftUserConfigRemarkLength)]
        public string Remark { get; set; }
    }
}
