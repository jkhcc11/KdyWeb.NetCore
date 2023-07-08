using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 云盘用户 子账号
    /// </summary>
    public class CloudParseUserChildren : BaseEntity<long>
    {
        /// <summary>
        /// Cookie长度
        /// </summary>
        public const int CookieInfoLength = 1000;

        /// <summary>
        /// 别名长度
        /// </summary>
        public const int AliasLength = 50;

        public const int OldSubAccountInfoLength = 10;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="cloudParseCookieTypeId">Cookie类型Id</param>
        /// <param name="cookieInfo">cookie</param>
        public CloudParseUserChildren(long userId, long cloudParseCookieTypeId, string cookieInfo)
        {
            UserId = userId;
            CloudParseCookieTypeId = cloudParseCookieTypeId;
            CookieInfo = cookieInfo;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Cookie类型Id
        /// </summary>
        public long CloudParseCookieTypeId { get; set; }

        public virtual CloudParseCookieType CloudParseCookieType { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        [StringLength(CookieInfoLength)]
        public string CookieInfo { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        [StringLength(AliasLength)]
        public string Alias { get; set; }

        /// <summary>
        /// 旧子账号信息
        /// </summary>
        /// <remarks>
        ///  兼容旧版使用 xxxx_id
        /// </remarks>
        [StringLength(OldSubAccountInfoLength)]
        public string OldSubAccountInfo { get; set; }
    }
}
