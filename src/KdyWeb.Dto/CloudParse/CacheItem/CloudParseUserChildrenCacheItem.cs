using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Dto.CloudParse.CacheItem
{
    /// <summary>
    /// 子账号缓存
    /// </summary>
    [AutoMap(typeof(CloudParseUserChildren))]
    public class CloudParseUserChildrenCacheItem : BaseEntityDto<long>
    {
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
        public string CookieInfo { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 旧子账号信息
        /// </summary>
        /// <remarks>
        ///  兼容旧版使用 xxxx_id
        /// </remarks>
        public string OldSubAccountInfo { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        /// <remarks>
        ///  用于解析日志输出 没有别名就是用户名+数字
        /// </remarks>
        public string ShowName { get; set; }
    }
}
