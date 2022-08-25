using System.Collections.Generic;
using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpApi.GameCheck.GenShin
{
    /// <summary>
    /// 获取用户绑定角色信息
    /// </summary>
    public class QueryUserBindInfoByCookieResult
    {
        /// <summary>
        /// 角色列表
        /// </summary>
        [JsonProperty("list")]
        public List<QueryUserBindInfoByCookieResultItem> RoleInfo { get; set; }
    }

    /// <summary>
    /// 角色Item
    /// </summary>
    public class QueryUserBindInfoByCookieResultItem
    {
        /// <summary>
        /// 用户Id(Roid Id)
        /// </summary>
        [JsonProperty("game_uid")]
        public string Uid { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        [JsonProperty("region")]
        public string RegionId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [JsonProperty("nickname")]
        public string NickName { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// 区域名（服务器）
        /// </summary>
        [JsonProperty("region_name")]
        public string RegionName { get; set; }
    }
}
