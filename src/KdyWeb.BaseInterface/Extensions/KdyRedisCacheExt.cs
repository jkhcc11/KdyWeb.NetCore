using System;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.KdyRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
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

            services.AddStackExchangeRedisCache(opt =>
            {
                //opt.ConfigurationOptions = new ConfigurationOptions
                //{
                //    //链接异常 后台重试
                //    AbortOnConnectFail = false
                //};
                opt.Configuration = redisConnStr;
                opt.InstanceName = pre;
            });
            var redisConnect = ConnectionMultiplexer.Connect(redisConnStr);
            services.AddSingleton<IConnectionMultiplexer>(redisConnect);

            return services.UseKdyRedisCache(opt =>
             {
                 // opt.ConnectionMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
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

        #region 序列化扩展

        #region StringSet
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

        #region hash set
        /// <summary>
        /// 删除HashSet字段值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static async Task<bool> DeleteHashSetAsync(this IDatabase db, string key, string field)
        {
            return await db.HashDeleteAsync(key, field);
        }

        /// <summary>
        /// 获取HashSet字段值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static async Task<string> GetHashSetAsync(this IDatabase db, string key, string field)
        {
            return await db.HashGetAsync(key, field);
        }

        /// <summary>
        /// 设置HashSet字段值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public static async Task<bool> SetHashSetAsync(this IDatabase db, string key, string field, string value)
        {
            return await db.HashSetAsync(key, field, value);
        }

        /// <summary>
        /// 获取HashSet字段值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static async Task<T> GetHashSetAsync<T>(this IDatabase db, string key, string field)
        {
            string cacheV = await db.HashGetAsync(key, field);
            if (string.IsNullOrEmpty(cacheV))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(cacheV);
        }

        /// <summary>
        /// 设置HashSet字段值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public static async Task<bool> SetHashSetAsync<T>(this IDatabase db, string key, string field, T value)
        {
            var cacheV = JsonConvert.SerializeObject(value);
            return await db.HashSetAsync(key, field, cacheV);
        }
        #endregion

        #region IDistributedCache
        /// <summary>
        ///  IDistributedCache 设置值 异步
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> SetValueAsync<T>(this IDistributedCache distributedCache, string key, T input,
            TimeSpan? ts = null)
        {
            var str = JsonConvert.SerializeObject(input);
            await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(str), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = ts
            });

            return true;
        }

        /// <summary>
        /// IDistributedCache 获取值 异步
        /// </summary>
        /// <returns></returns>
        public static async Task<T> GetValueAsync<T>(this IDistributedCache distributedCache, string key)
        {
            var v = await distributedCache.GetStringAsync(key);
            return string.IsNullOrEmpty(v) ? default : JsonConvert.DeserializeObject<T>(v);
        }

        #endregion

        #endregion
    }
}
