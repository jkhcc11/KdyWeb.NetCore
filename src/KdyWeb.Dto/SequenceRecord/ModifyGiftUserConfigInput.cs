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
        public long GiftUserConfigId { get; set; }

        /// <summary>
        /// 奖励结束时间
        /// </summary>
        public DateTime GiftEndTime { get; set; }
    }
}
