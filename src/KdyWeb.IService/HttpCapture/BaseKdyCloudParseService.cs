using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.KdyHttp;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    ///  网盘解析 基础抽象
    /// </summary>
    public abstract class BaseKdyCloudParseService<TConfigEntity, TInput, TOut, TDownEntity> :
        IKdyCloudParseService<TInput, TOut, TDownEntity>
        where TConfigEntity : class, IBaseConfigEntity
        where TOut : class, IBaseResultOut
    {
        /// <summary>
        /// Redis
        /// </summary>
        protected readonly IKdyRedisCache KdyRedisCache;
        /// <summary>
        /// 配置
        /// </summary>
        protected TConfigEntity CloudConfig { get; set; }
        /// <summary>
        /// Http请求入参
        /// </summary>
        protected KdyRequestCommonInput KdyRequestCommonInput { get; set; }
        /// <summary>
        /// 通用Http请求
        /// </summary>
        protected readonly IKdyRequestClientCommon KdyRequestClientCommon;
        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogger KdyLog;

        protected BaseKdyCloudParseService(TConfigEntity cloudConfig)
        {
            KdyRedisCache = KdyBaseServiceProvider.ServiceProvide.GetService<IKdyRedisCache>();
            CloudConfig = cloudConfig;
            KdyRequestClientCommon = KdyBaseServiceProvider.ServiceProvide.GetService<IKdyRequestClientCommon>();
            KdyLog = KdyBaseServiceProvider.ServiceProvide.GetService<ILoggerFactory>().CreateLogger(GetType());
        }

        /// <summary>
        /// 通用Json转返回结果
        /// </summary>
        /// <returns></returns>
        protected abstract List<TOut> JArrayHandler(JObject jObject);

        /// <summary>
        /// 查询接口
        /// </summary>
        /// <returns></returns>
        public abstract Task<KdyResult<List<TOut>>> QueryFileAsync(BaseQueryInput<TInput> input);

        /// <summary>
        /// 获取实际下载地址
        /// </summary>
        /// <returns></returns>
        public abstract Task<KdyResult<string>> GetDownUrlForNoCacheAsync(BaseDownInput<TDownEntity> input);

        /// <summary>
        /// 检查下载地址有效性 默认为true
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> CheckDownUrlAsync(string url)
        {
            await Task.CompletedTask;
            return true;
        }

        /// <summary>
        /// 获取下载地址 检查了缓存
        /// </summary>
        /// <returns></returns>
        public virtual async Task<KdyResult<string>> GetDownUrlAsync(BaseDownInput<TDownEntity> input)
        {
            var cacheV = await KdyRedisCache.GetCache().GetStringAsync(input.CacheKey);
            if (string.IsNullOrEmpty(cacheV))
            {
                //无缓存
                return await GetDownUrlForNoCacheAsync(input);
            }

            //检查有效
            if (await CheckDownUrlAsync(cacheV))
            {
                return KdyResult.Success(cacheV, "get cache success");
            }

            //无效重新获取
            return await GetDownUrlForNoCacheAsync(input);
        }
    }
}
