using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 创建和更新服务器Cookie
    /// </summary>
    public class CreateAndUpdateServerCookieInput
    {
        public long? Id { get; set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        [Range(1, long.MaxValue)]
        public long SubAccountId { get; set; }

        /// <summary>
        /// 服务器Ip
        /// </summary>
        [StringLength(ServerCookie.ServerIpLength)]
        [Required]
        public string ServerIp { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        [StringLength(CloudParseUserChildren.CookieInfoLength)]
        [Required]
        public string CookieInfo { get; set; }
    }
}
