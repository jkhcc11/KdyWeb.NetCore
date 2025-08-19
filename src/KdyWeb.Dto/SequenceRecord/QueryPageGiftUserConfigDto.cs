using KdyWeb.Entity.SequenceRecord.Enum;
using System;
using AutoMapper;
using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页查询奖励用户配置
    /// </summary>
    [AutoMap(typeof(GiftUserConfig))]
    public class QueryPageGiftUserConfigDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 奖励用户类型
        /// </summary>
        public GiftUserTypeEnum GiftUserType { get; set; }

        /// <summary>
        /// 用户显示名(冗余)
        /// </summary>
        /// <remarks>
        ///  对应群昵称
        /// </remarks>
        public string UserShowName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

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
        public string Remark { get; set; }
    }
}
