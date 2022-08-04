using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace KdyWeb.HttpApi
{
    /// <summary>
    /// 重写缓存键值策略 移除sha1加密
    /// </summary>
    public class KdyCustomRedisProcessingStrategy : RedisProcessingStrategy
    {
        private readonly IRateLimitConfiguration _config;
        public KdyCustomRedisProcessingStrategy(IConnectionMultiplexer connectionMultiplexer, IRateLimitConfiguration config, ILogger<RedisProcessingStrategy> logger) :
            base(connectionMultiplexer, config, logger)
        {
            _config = config;
        }

        protected override string BuildCounterKey(ClientRequestIdentity requestIdentity, RateLimitRule rule, ICounterKeyBuilder counterKeyBuilder,
            RateLimitOptions rateLimitOptions)
        {
            var key = counterKeyBuilder.Build(requestIdentity, rule);
            if (rateLimitOptions.EnableEndpointRateLimiting && _config.EndpointCounterKeyBuilder != null)
            {
                key += _config.EndpointCounterKeyBuilder.Build(requestIdentity, rule);
            }

            return key;
            //var bytes = Encoding.UTF8.GetBytes(key);
            //using var algorithm = new SHA1Managed();
            //var hash = algorithm.ComputeHash(bytes);
            //return Convert.ToBase64String(hash);
        }
    }
}
