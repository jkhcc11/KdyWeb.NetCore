using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpApi.GameCheck.GenShin
{
    /// <summary>
    /// 实时便签
    /// </summary>
    public class DailyNoteResult
    {
        /// <summary>
        /// 当前树脂
        /// </summary>
        [JsonProperty("current_resin")]
        public int CurrentResin { get; set; }

        /// <summary>
        /// 最大树脂
        /// </summary>
        [JsonProperty("max_resin")]
        public int MaxResin { get; set; }

        /// <summary>
        /// 树脂恢复时间（秒）
        /// </summary>
        [JsonProperty("resin_recovery_time")]
        public int ResinRecoveryTime { get; set; }

        //public int FinishedTaskNum { get; set; }
        //public int TotalTaskNum { get; set; }
        /// <summary>
        /// 质量参变
        /// </summary>
        [JsonProperty("is_extra_task_reward_received")]
        public bool RewardReceived { get; set; }

        /// <summary>
        /// 剩余周本减半数
        /// </summary>
        [JsonProperty("remain_resin_discount_num")]
        public int DiscountNum { get; set; }

        //public int CurrentExpeditionNum { get; set; }
        /// <summary>
        /// 洞天恢复时间（秒）
        /// </summary>
        [JsonProperty("home_coin_recovery_time")]
        public int HomeCoinRecoveryTime { get; set; }
    }
}
