using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.WebParse;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    /// 自用站点解析 抽象基类
    /// </summary>
    /// <typeparam name="TParseInput">入参泛型</typeparam>
    /// <typeparam name="TConfig">配置泛型</typeparam>
    /// <typeparam name="TParseOut">出参泛型</typeparam>
    public abstract class BaseKdyWebParseService<TConfig, TParseInput, TParseOut> : IKdyWebParseService<TParseInput, TParseOut>
        where TParseInput : IKdyWebParseInput
        where TParseOut : class, IKdyWebParseOut, new()
        where TConfig : class, IKdyWebParseConfig, new()
    {
        /// <summary>
        /// 缓存前缀
        /// </summary>
        protected const string CachePrefix = "ParseWeb:";
        /// <summary>
        /// 基础配置
        /// </summary>
        protected readonly TConfig BaseConfig;
        /// <summary>
        /// Redis
        /// </summary>
        protected readonly IKdyRedisCache KdyRedisCache;
        /// <summary>
        /// 缓存时间（分）
        /// </summary>
        protected int CacheTime { get; set; }
        protected BaseKdyWebParseService()
        {
            KdyRedisCache = KdyBaseServiceProvider.ServiceProvide.GetService<IKdyRedisCache>();
            BaseConfig = KdyBaseServiceProvider.ServiceProvide.GetService<IOptions<TConfig>>()?.Value;
            CacheTime = 10;
        }

        /// <summary>
        /// 获取解析结果(有缓存)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<KdyResult<TParseOut>> GetResultAsync(TParseInput input)
        {
            var db = KdyRedisCache.GetDb();
            var cacheKey = $"{CachePrefix}{input.DetailUrl}";
            var cache = await db.GetValueAsync<TParseOut>(cacheKey);
            if (cache != null)
            {
                return KdyResult.Success(cache);
            }

            var result = await GetNoCacheResultAsync(input);
            if (result.IsSuccess)
            {
                await db.SetValueAsync(cacheKey, result.Data, TimeSpan.FromMinutes(CacheTime));
            }

            return result;
        }

        /// <summary>
        /// 获取解析结果(无缓存 源结果)
        /// </summary>
        /// <returns></returns>
        public abstract Task<KdyResult<TParseOut>> GetNoCacheResultAsync(TParseInput input);
    }
}
