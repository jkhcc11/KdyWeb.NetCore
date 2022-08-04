using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace KdyWeb.BaseInterface.KdyRedis
{
    /// <summary>
    /// Redis缓存 实现
    /// </summary>
    public class KdyRedisCache : IKdyRedisCache
    {
        private readonly KdyRedisCacheOption _option;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public KdyRedisCache(IOptions<KdyRedisCacheOption> option, IConnectionMultiplexer connectionMultiplexer)
        {
            _option = option.Value;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public IDatabase GetDb(int i = 0)
        {
            return _connectionMultiplexer.GetDatabase(i);
        }

        public IServer GetServer()
        {
            foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                if (server.IsSlave == false)
                {
                    return server;
                }
            }

            throw new Exception($"{nameof(KdyRedisCache)} 未获取到Redis主节点");
        }

        public IDistributedCache GetCache()
        {
            return _option.DistributedCache;
        }
    }
}
