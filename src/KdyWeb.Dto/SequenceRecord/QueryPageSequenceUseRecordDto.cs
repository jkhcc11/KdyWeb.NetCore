using KdyWeb.Entity.SequenceRecord.Enum;
using System;
using AutoMapper;
using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页获取接龙使用记录
    /// </summary>
    [AutoMap(typeof(SequenceUseRecord))]
    public class QueryPageSequenceUseRecordDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户显示名(冗余)
        /// </summary>
        /// <remarks>
        ///  对应群昵称
        /// </remarks>
        public string UserShowName { get; set; }

        /// <summary>
        /// 用户昵称(冗余)
        /// </summary>
        /// <remarks>
        ///  对应微信昵称
        /// </remarks>
        public string UserNickName { get; set; }

        /// <summary>
        /// 接龙使用时间
        /// </summary>
        public DateTime UseDate { get; set; }

        /// <summary>
        /// 当天结算价格
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// 奖励用户类型
        /// </summary>
        /// <remarks>
        /// 如果有奖励用户
        /// </remarks>
        public GiftUserTypeEnum? GiftUserType { get; set; }

        /// <summary>
        /// 场馆Id
        /// </summary>
        public long VenuesId { get; set; }

        /// <summary>
        /// 场馆缩写（冗余）
        /// </summary>
        public string VenuesShortName { get; set; }
    }
}
