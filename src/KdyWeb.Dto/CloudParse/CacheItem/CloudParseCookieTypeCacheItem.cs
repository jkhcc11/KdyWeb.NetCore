using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Dto.CloudParse.CacheItem
{
    /// <summary>
    /// 子账号类型缓存
    /// </summary>
    [AutoMap(typeof(CloudParseCookieType))]
    public class CloudParseCookieTypeCacheItem : BaseEntityDto<long>
    {
        /// <summary>
        /// 显示文案
        /// </summary>
        public string ShowText { get; set; }
    }
}
