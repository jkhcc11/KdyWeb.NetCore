using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.KdyRedis;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

        #region 序列化扩展
        /// <summary>
        /// Redis设置值 异步
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> SetValueAsync<T>(this IDatabase db, string key, T input, TimeSpan? ts = null)
        {
            var str = JsonConvert.SerializeObject(input);
            return await db.StringSetAsync(key, str, ts);
        }

        /// <summary>
        /// Redis获取值 异步
        /// </summary>
        /// <returns></returns>
        public static async Task<T> GetValueAsync<T>(this IDatabase db, string key)
        {
            var v = await db.StringGetAsync(key);
            return v.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(v);
        }

        /// <summary>
        /// Redis设置值
        /// </summary>
        /// <returns></returns>
        public static bool SetValue<T>(this IDatabase db, string key, T input, TimeSpan? ts = null)
        {
            var str = JsonConvert.SerializeObject(input);
            return db.StringSet(key, str, ts);
        }

        /// <summary>
        /// Redis获取值
        /// </summary>
        /// <returns></returns>
        public static T GetValue<T>(this IDatabase db, string key)
        {
            var v = db.StringGet(key);
            return v.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(v);
        }
        #endregion

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
