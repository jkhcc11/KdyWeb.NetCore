using System;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 修改奖励用户配置
    /// </summary>
    public class ModifyGiftUserConfigInput
    {
        /// <summary>
        /// 奖励用户配置Id
        /// </summary>
        public long Id { get; set; }

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
        /// 奖励有效次数
        /// </summary>
        /// <remarks>
        ///  1、折扣卡金额大于0不用管次数
        ///  2、只有擂主免和折扣免才需要管这个次数
        /// </remarks>
        public int? GiftValidCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
