using AutoMapper;
using AutoMapper.Configuration.Annotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;

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
        [SourceMember(nameof(CloudParseUserChildren.CloudParseCookieTypeId))]
        public long CookieTypeId { get; set; }

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

        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        /// 有些下载需要固定带上附加ID，如 分组ID等，这种不支持跨云盘切换，所以手动指定
        /// </remarks>
        public string BusinessId { get; set; }

    }
}
