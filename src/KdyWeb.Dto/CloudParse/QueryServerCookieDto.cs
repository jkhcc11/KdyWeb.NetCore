using AutoMapper;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询服务器Cookie列表
    /// </summary>
    [AutoMap(typeof(ServerCookie))]
    public class QueryServerCookieDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 子账号Id
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long SubAccountId { get; set; }

        /// <summary>
        /// 子账号别名
        /// </summary>
        public string SubAccountAlias { get; set; }

        /// <summary>
        /// 服务器Ip
        /// </summary>
        public string ServerIp { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        public string CookieInfo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ServerCookieStatus ServerCookieStatus { get; set; }
    }
}
