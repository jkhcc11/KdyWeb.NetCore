using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 批量创建Token
    /// </summary>
    public class BatchCreateOneApiTokenInput
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int TokenCount { get; set; }

        /// <summary>
        /// Api密钥
        /// </summary>
        [Required(ErrorMessage = "ApiToken必填")]
        public string ApiToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "日期格式错误")]
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// 过期时间input
        /// </summary>
        [JsonProperty("expired_time")]
        public long ExpiredTimeExt { get; set; }

        [Required(ErrorMessage = "名称前缀必填")]
        public string TokenNamePrefix { get; set; }

        /// <summary>
        /// Token名
        /// </summary>
        [JsonProperty("name")]
        public string TokenName { get; set; }

        /// <summary>
        /// 可用额度
        /// </summary>
        /// <remarks>
        ///  1刀 50W
        /// </remarks>
        [JsonProperty("remain_quota")]
        [Range(1, 50)]
        public int RemainQuota { get; set; }

        /// <summary>
        /// 是否无限制额度
        /// </summary>
        [JsonProperty("unlimited_quota")]
        public bool UnlimitedQuota { get; set; }
    }
}
