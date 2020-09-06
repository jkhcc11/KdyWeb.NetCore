using System;
using KdyWeb.BaseInterface.KdyRedis;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace KdyWeb.BaseInterface.Extensions
{

    /// <summary>
    /// 缓存扩展
    /// </summary>
    public static class KdyRedisCacheExt
    {
        /// <summary>
        /// Redis配置Key
        /// </summary>
        public const string RedisConnKey = "RedisConnStr";

        /// <summary>
        /// 使用Redis缓存
        /// </summary>
        public static IServiceCollection UseRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnStr = configuration.GetConnectionString(RedisConnKey);
            if (string.IsNullOrEmpty(redisConnStr))
            {
                throw new Exception($"{nameof(KdyRedisCacheExt)}未配置Redis连接字符串");
            }

            //获取前缀
            var pre = configuration.GetValue("RedisConfig:Prefix", "KdyRedis:");
            var option = new RedisCacheOptions()
            {
                Configuration = redisConnStr,
                InstanceName = pre
            };
            return services.UseKdyRedisCache(opt =>
             {
                 opt.ConnectionMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
                 opt.DistributedCache = new RedisCache(option);
             });
        }

        /// <summary>
        /// 使用StackExchange.Redis 微软DistributedRedisCache 不能获取实例
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private static IServiceCollection UseKdyRedisCache(this IServiceCollection services, Action<KdyRedisCacheOption> option)
        {
            services.Configure(option);
            services.AddSingleton<IKdyRedisCache, KdyRedisCache>();
            return services;
        }
    }
}
