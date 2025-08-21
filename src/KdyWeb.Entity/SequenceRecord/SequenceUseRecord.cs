using System;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SequenceRecord.Enum;

namespace KdyWeb.Entity.SequenceRecord
{
    /// <summary>
    /// 接龙使用记录
    /// </summary>
    /// <remarks>
    /// 场地当天接龙的列表
    /// </remarks>
    public class SequenceUseRecord : BaseEntity<long>
    {
        public const int UseRecordRemarkLength = 20;

        /// <summary>
        /// 接龙使用记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="useDate">接龙使用时间</param>
        /// <param name="currentPrice">当天结算价格</param>
        public SequenceUseRecord(string userId, DateTime useDate, decimal currentPrice)
        {
            UserId = userId;
            UseDate = useDate;
            CurrentPrice = currentPrice;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        [StringLength(SequenceUserRecord.UserIdLength)]
        public string UserId { get; protected set; }

        /// <summary>
        /// 用户显示名(冗余)
        /// </summary>
        /// <remarks>
        ///  对应群昵称
        /// </remarks>
        [StringLength(SequenceUserRecord.UserShowNameLength)]
        public string? UserShowName { get; set; }

        /// <summary>
        /// 用户昵称(冗余)
        /// </summary>
        /// <remarks>
        ///  对应微信昵称
        /// </remarks>
        [StringLength(SequenceUserRecord.UserNickNameLength)]
        public string? UserNickName { get; set; }

        /// <summary>
        /// 接龙使用时间
        /// </summary>
        public DateTime UseDate { get; protected set; }

        /// <summary>
        /// 当天结算价格
        /// </summary>
        public decimal CurrentPrice { get; protected set; }

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
        [StringLength(VenuesConfig.ShortNameLength)]
        public string? VenuesShortName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(UseRecordRemarkLength)]
        public string? UseRecordRemark { get; set; }

        /// <summary>
        /// 更新当前价格
        /// </summary>
        /// <param name="giftUserType">奖励用户类型</param>
        /// <param name="currentPrice">当前价格</param>
        public void UpdateCurrentPrice(GiftUserTypeEnum? giftUserType, decimal currentPrice)
        {
            GiftUserType = giftUserType;
            CurrentPrice = currentPrice;
        }
    }
}
