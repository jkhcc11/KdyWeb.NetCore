using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 光鸭Token缓存
    /// </summary>
    public class GuangYaCloudTokenCache
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        [JsonProperty("access_token")]
        public string Token { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
