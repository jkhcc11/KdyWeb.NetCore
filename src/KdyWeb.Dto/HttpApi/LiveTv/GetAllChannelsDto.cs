using System.Collections.Generic;
using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpApi.LiveTv
{
    /// <summary>
    /// 所有频道列表
    /// </summary>
    public class GetAllChannelsDto
    {
        /// <summary>
        /// 频道Id
        /// </summary>
        [JsonProperty("id")]
        public string ChannelId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        [JsonProperty("alt_names")]
        public List<string> AltNames { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        [JsonProperty("logo")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// 站点地址
        /// </summary>
        [JsonProperty("website")]
        public string WebSite { get; set; }

        /// <summary>
        /// 是否中国
        /// </summary>
        /// <returns></returns>
        public bool IsChina()
        {
            return Country == "CN" ||
                   Country == "HK" ||
                   Country == "TW";
        }
    }
}
