using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace KdyWeb.IService.CloudParse
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

        protected BaseKdyCloudParseService()
        {
            KdyRedisCache = KdyBaseServiceProvider.ServiceProvide.GetService<IKdyRedisCache>();
            KdyRequestClientCommon = KdyBaseServiceProvider.ServiceProvide.GetService<IKdyRequestClientCommon>();
            KdyLog = KdyBaseServiceProvider.ServiceProvide.GetService<ILoggerFactory>().CreateLogger(GetType());
        }

        protected BaseKdyCloudParseService(TConfigEntity cloudConfig) : this()
        {
            CloudConfig = cloudConfig;
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
                KdyLog.LogInformation("{userInfo},获取缓存地址成功,{cacheV}", CloudConfig.ReqUserInfo, cacheV);
                return KdyResult.Success(cacheV, "get cache success");
            }

            //无效重新获取
            return await GetDownUrlForNoCacheAsync(input);
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <remarks>
        /// 根据各网盘缓存情况删除
        /// </remarks>
        /// <returns></returns>
        public virtual async Task<bool> ClearCacheAsync()
        {
            var nameCacheKey = GetCacheKeyWithFileName();
            var nameDb = GetNameCacheDb();
            return await nameDb.KeyDeleteAsync(nameCacheKey);
        }

        /// <summary>
        /// 根据Url获取动态过期时间
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="expiresFlag">过期时间标识 eg:Expires</param>
        /// <param name="defaultCacheMinutes">默认缓存分钟</param>
        /// <returns></returns>
        protected virtual TimeSpan GetExpiresByUrl(string url, string expiresFlag, int defaultCacheMinutes = 130)
        {
            var expires = url.GetStrMathExt($"{expiresFlag}=", "&");
            var ts = TimeSpan.FromMinutes(defaultCacheMinutes);
            if (expires.IsEmptyExt() == false)
            {
                var expiresTime = Convert.ToInt32(expires)
                    .ToDataTimeByTimestamp()
                    .AddMinutes(-5);
                ts = expiresTime - DateTime.Now;
            }

            if (ts.TotalSeconds <= 0)
            {
                //太短 只留一分钟
                ts = TimeSpan.FromMinutes(1);
            }

            return ts;
        }

        /// <summary>
        /// 根据关键字获取文件信息
        /// </summary>
        /// <returns></returns>
        public abstract Task<KdyResult<BaseResultOut>> GetFileInfoByKeyWordAsync(string keyWord);

        /// <summary>
        /// 获取名称缓存DB
        /// </summary>
        /// <remarks>
        /// 全局名称缓存，每次切换或者变更时需要移除缓存
        /// </remarks>
        /// <returns></returns>
        protected virtual IDatabase GetNameCacheDb()
        {
            return KdyRedisCache.GetDb(6);
        }

        /// <summary>
        /// 获取文件名缓存Key
        /// </summary>
        /// <remarks>
        ///  文件名->文件Id对应关系得HashSet Key
        /// </remarks>
        /// <returns></returns>
        protected virtual string GetCacheKeyWithFileName()
        {
            return $"{CacheKeyConst.FileNameCache}:{CloudConfig.ChildUserId}";
        }
    }
}
