using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpApi.GameCheck.GenShin
{
    /// <summary>
    /// 签到信息 result
    /// </summary>
    public class QuerySignInfoResult
    {
        /// <summary>
        /// 累计签到天数
        /// </summary>
        [JsonProperty("total_sign_day")]
        public int TotalSignDay { get; set; }

        /// <summary>
        /// 今日日期 xxxx-yy-dd
        /// </summary>
        [JsonProperty("today")]
        public string Today { get; set; }

        /// <summary>
        /// 是否签到
        /// </summary>
        [JsonProperty("is_sign")]
        public bool IsSign { get; set; }

        /// <summary>
        /// 漏签天数
        /// </summary>
        [JsonProperty("sign_cnt_missed")]
        public int SignMissed { get; set; }
    }
}
