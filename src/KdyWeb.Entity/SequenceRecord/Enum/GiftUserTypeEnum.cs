using System.ComponentModel;

namespace KdyWeb.Entity.SequenceRecord.Enum
{
    /// <summary>
    /// 奖励用户类型
    /// </summary>
    /// <remarks>
    /// 所有奖励都不可重叠使用
    /// </remarks>
    public enum GiftUserTypeEnum
    {
        /// <summary>
        /// 年卡扣卡
        /// </summary>
        [Description("扣卡")]
        Card = 1,

        /// <summary>
        /// 免费次数
        /// </summary>
        [Description("冠亚奖")]
        Free = 2,

        /// <summary>
        /// 时间区间价格
        /// </summary>
        [Description("折扣奖")]
        TimePrice = 3,

        /// <summary>
        /// Vip
        /// </summary>
        /// <remarks>
        /// 15免1 人员
        /// </remarks>
        [Description("Vip免")]
        VipUser = 99
    }
}
