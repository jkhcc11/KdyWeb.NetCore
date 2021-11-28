using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 胜天用户信息缓存
    /// </summary>
    public class StUserInfoCache
    {
        /// <summary>
        /// 根目录Id
        /// </summary>
        [JsonProperty("rootDirId")]
        public string RootDir { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
    }
}
