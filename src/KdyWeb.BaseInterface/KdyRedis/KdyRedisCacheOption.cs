using Microsoft.Extensions.Caching.Distributed;

namespace KdyWeb.BaseInterface.KdyRedis
{
    /// <summary>
    /// Redis缓存 选项
    /// </summary>
    public class KdyRedisCacheOption
    {
        ///// <summary>
        ///// Redis连接对象
        ///// </summary>
        //public IConnectionMultiplexer ConnectionMultiplexer { get; set; }

        /// <summary>
        /// 微软分布式缓存
        /// </summary>
        public IDistributedCache DistributedCache { get; set; }
    }
}
