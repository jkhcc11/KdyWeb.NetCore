using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 阿里云Token缓存
    /// </summary>
    public class AilYunCloudTokenCache
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        [JsonProperty("access_token")]
        public string Token { get; set; }

        /// <summary>
        /// 默认DriveId
        /// </summary>
        [JsonProperty("default_drive_id")]
        public string DriveId { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [JsonProperty("nick_name")]
        public string NickName { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
    }
}
