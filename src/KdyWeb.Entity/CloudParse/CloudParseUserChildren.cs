using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 云盘用户 子账号
    /// </summary>
    public class CloudParseUserChildren : BaseEntity<int>
    {
        /// <summary>
        /// Cookie长度
        /// </summary>
        public const int CookieInfoLength = 1000;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="cookieType">Cookie类型</param>
        /// <param name="cookieInfo">cookie</param>
        public CloudParseUserChildren(long userId, CloudParseCookieType cookieType, string cookieInfo)
        {
            UserId = userId;
            CookieType = cookieType;
            CookieInfo = cookieInfo;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///  Cookie类型
        /// </summary>
        public CloudParseCookieType CookieType { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        [StringLength(CookieInfoLength)]
        public string CookieInfo { get; set; }

        /// <summary>
        /// 云盘用户
        /// </summary>
        public virtual CloudParseUser CloudParseUser { get; set; }
    }
}
