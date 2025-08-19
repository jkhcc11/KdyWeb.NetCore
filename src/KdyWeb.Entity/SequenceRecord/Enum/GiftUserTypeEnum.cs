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
        /// <remarks>
        ///  可以跟其他叠加，最后使用
        /// </remarks>
        [Description("扣卡")]
        Card = 1,

        /// <summary>
        /// 时间区间价格
        /// </summary>
        /// <remarks>
        /// 可以叠加，其次使用
        /// </remarks>
        [Description("折扣奖")]
        TimePrice = 2,

        /// <summary>
        /// 免费次数
        /// </summary>
        /// <remarks>
        ///  可以叠加，优先使用
        /// </remarks>
        [Description("冠亚奖")]
        Free = 3,

        /// <summary>
        /// 擂主价格
        /// </summary>
        /// <remarks>
        ///  可以叠加，优先使用
        ///  这里有多个配置,如果免费则优先
        /// </remarks>
        [Description("擂主奖")]
        RingmasterPrice = 4,

        /// <summary>
        /// Vip
        /// </summary>
        /// <remarks>
        /// 15免1 人员
        /// </remarks>
        /// <remarks>
        /// 可以叠加，只能有一个有效
        /// </remarks>
        [Description("Vip免")]
        VipUser = 99
    }
}
