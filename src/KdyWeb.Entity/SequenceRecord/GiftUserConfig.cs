using System;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SequenceRecord.Enum;

namespace KdyWeb.Entity.SequenceRecord
{
    /// <summary>
    /// 奖励用户配置
    /// </summary>
    public class GiftUserConfig : BaseEntity<long>
    {
        /// <summary>
        /// 奖励用户记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="giftUserType">奖励用户类型</param>
        /// <param name="giftStartTime">奖励开始时间</param>
        /// <param name="giftEndTime">奖励结束时间</param>
        public GiftUserConfig(string userId, GiftUserTypeEnum giftUserType,
            DateTime giftStartTime, DateTime giftEndTime)
        {
            UserId = userId;
            GiftUserType = giftUserType;
            GiftStartTime = giftStartTime;
            GiftEndTime = giftEndTime;
            IsEnable = true;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; protected set; }

        /// <summary>
        /// 奖励用户类型
        /// </summary>
        public GiftUserTypeEnum GiftUserType { get; protected set; }

        /// <summary>
        /// 用户显示名(冗余)
        /// </summary>
        /// <remarks>
        ///  对应群昵称
        /// </remarks>
        public string? UserShowName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; protected set; }

        /// <summary>
        /// 奖励开始时间
        /// </summary>
        public DateTime GiftStartTime { get; protected set; }

        /// <summary>
        /// 奖励结束时间
        /// </summary>
        public DateTime GiftEndTime { get; protected set; }

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
        /// 禁用
        /// </summary>
        public void Ban()
        {
            IsEnable = false;
        }

        /// <summary>
        /// 设置奖励结束时间
        /// </summary>
        public void SetGiftEndTime(DateTime giftEndTime)
        {
            GiftEndTime = giftEndTime;
        }
    }
}
