using System;
using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpApi.LiveTv
{
    /// <summary>
    /// 获取实时流检查列表
    /// </summary>
    public class GetAllStreamsDto
    {
        /// <summary>
        /// 频道Id
        /// </summary>
        [JsonProperty("channel")]
        public string ChannelId { get; set; }

        /// <summary>
        /// HLS
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <remarks>
        /// online, blocked, timeout, error
        /// </remarks>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [JsonProperty("added_at")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 检查时间
        /// </summary>
        [JsonProperty("checked_at")]
        public DateTime CheckedTime { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Status == "online";
        }
    }
}
