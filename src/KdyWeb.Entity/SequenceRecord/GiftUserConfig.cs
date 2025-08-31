using System;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SequenceRecord.Enum;

namespace KdyWeb.Entity.SequenceRecord
{
    /// <summary>
    /// 奖励用户配置
    /// </summary>
    public class GiftUserConfig : BaseEntity<long>
    {
        public const int GiftUserConfigRemarkLength = 100;

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
        [StringLength(SequenceUserRecord.UserIdLength)]
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
        [StringLength(SequenceUserRecord.UserShowNameLength)]
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
        /// 奖励有效次数
        /// </summary>
        /// <remarks>
        ///  1、折扣卡金额大于0不用管次数
        ///  2、只有擂主免和折扣免才需要管这个次数
        /// </remarks>
        public int? GiftValidCount { get; set; }

        /// <summary>
        /// 奖励已用次数
        /// </summary>
        /// <remarks>
        /// 1、只有擂主免和折扣免才需要管这个次数
        /// 2、只有改了日期、时才可以重置次次数
        /// </remarks>
        public int GiftUseCount { get; protected set; }

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
        [StringLength(GiftUserConfigRemarkLength)]
        public string? Remark { get; set; }

        /// <summary>
        /// 奖励扣费排序（奖励叠加时扣费顺序）
        /// </summary>
        /// <remarks>
        /// 多个奖励同时存在时使用顺序（越大越靠前扣费）
        /// </remarks>
        public int GiftOrderBy { get; set; }

        /// <summary>
        /// 是否变更(忽略dB查询，仅为了标识需要更新)
        /// </summary>
        public bool IsChange { get; protected set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Ban()
        {
            IsEnable = false;
        }

        /// <summary>
        /// 启用
        /// </summary>
        public void Enable()
        {
            IsEnable = true;
        }

        /// <summary>
        /// 设置奖励开始时间
        /// </summary>
        public void SetGiftStartTime(DateTime giftStartTime)
        {
            GiftStartTime = giftStartTime;
        }

        /// <summary>
        /// 设置奖励结束时间
        /// </summary>
        public void SetGiftEndTime(DateTime giftEndTime)
        {
            GiftEndTime = giftEndTime;
        }

        /// <summary>
        /// 初始化次数
        /// </summary>
        public void InitGiftUseCount()
        {
            GiftUseCount = 0;
        }

        /// <summary>
        /// 增加使用次数
        /// </summary>
        public void AddGiftUseCount()
        {
            GiftUseCount++;
            IsChange = true;
        }

        /// <summary>
        /// 减少使用次数
        /// </summary>
        /// <remarks>
        ///  一般用于手动重置时
        /// </remarks>
        public void SubtractUseCount()
        {
            if (GiftUseCount <= 0)
            {
                return;
            }

            GiftUseCount--;
        }

        /// <summary>
        /// 判断奖励是否可用
        /// </summary>
        /// <remarks>
        ///  判断次数的需要管，其他不用管
        /// </remarks>
        /// <returns></returns>
        public bool IsCan()
        {
            if (GiftUserType.IsTotalCount(GiftPrice) == false)
            {
                return true;
            }

            //统计次数且 实际使用小于有效次数
            return GiftUseCount < GiftValidCount;

        }
    }
}
