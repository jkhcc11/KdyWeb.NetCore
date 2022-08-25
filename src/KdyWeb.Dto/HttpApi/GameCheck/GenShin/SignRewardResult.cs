using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpApi.GameCheck.GenShin
{
    /// <summary>
    /// bbs签到 result
    /// </summary>
    public class SignRewardResult
    {
        /// <summary>
        /// 风控码 375
        /// </summary>
        /// <remarks>
        /// 待测试
        /// </remarks>
        [JsonProperty("risk_code")]
        public int RiskCode { get; set; }

        /// <summary>
        ///  待测试
        /// </summary>
        /// <remarks>
        /// 待测试
        /// </remarks>
        [JsonProperty("success")]
        public int BusinessCode { get; set; }

        public bool IsSuccess => RiskCode == 0 && BusinessCode == 0;
    }
}
