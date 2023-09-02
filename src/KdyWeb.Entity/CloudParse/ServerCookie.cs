using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 服务器Cookie
    /// </summary>
    public class ServerCookie : BaseEntity<long>
    {
        /// <summary>
        ///ServerIp长度
        /// </summary>
        public const int ServerIpLength = 20;

        public ServerCookie(long subAccountId, string serverIp, string cookieInfo)
        {
            SubAccountId = subAccountId;
            ServerIp = serverIp;
            CookieInfo = cookieInfo;
            ServerCookieStatus = ServerCookieStatus.Init;
        }

        /// <summary>
        /// 子账号Id
        /// </summary>
        public long SubAccountId { get; set; }

        /// <summary>
        /// 服务器Ip
        /// </summary>
        [StringLength(ServerIpLength)]
        public string ServerIp { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        [StringLength(CloudParseUserChildren.CookieInfoLength)]
        public string CookieInfo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ServerCookieStatus ServerCookieStatus { get; set; }
    }
}
