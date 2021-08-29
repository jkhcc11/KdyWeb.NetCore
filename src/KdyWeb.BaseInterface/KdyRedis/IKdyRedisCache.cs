using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace KdyWeb.BaseInterface.KdyRedis
{
    /// <summary>
    /// Redis缓存 接口
    /// </summary>
    public interface IKdyRedisCache
    {
        /// <summary>
        /// 获取Redis Db
        /// </summary>
        /// <param name="i">db 默认为0</param>
        /// <returns></returns>
        IDatabase GetDb(int i = 0);

        /// <summary>
        /// 获取StackExchange 主 IServer 无节点将抛出异常
        /// </summary>
        /// <returns></returns>
        IServer GetServer();

        /// <summary>
        /// 获取微软分布式实例
        /// </summary>
        /// <returns></returns>
        IDistributedCache GetCache();
    }
}
