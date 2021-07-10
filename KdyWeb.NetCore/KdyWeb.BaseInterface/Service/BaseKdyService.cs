using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.BaseInterface.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 基础服务定义 实现
    /// </summary>
    public abstract class BaseKdyService : IKdyService
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogger KdyLog;
        /// <summary>
        /// 统一配置
        /// </summary>
        protected readonly IConfiguration KdyConfiguration;
        /// <summary>
        /// 工作单元
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;
        /// <summary>
        /// 登录信息
        /// </summary>
        protected readonly ILoginUserInfo LoginUserInfo;
        /// <summary>
        /// 可以单独更新字段
        /// </summary>
        protected readonly List<string> CanUpdateFieldList;
        /// <summary>
        /// 缓存
        /// </summary>
        protected readonly IKdyRedisCache KdyRedisCache;

        protected BaseKdyService(IUnitOfWork unitOfWork)
        {
            //todo:UnitOfWork 用scope时 无法直接获取 先直接构造器注入 后面调整
            KdyLog = KdyBaseServiceProvider.ServiceProvide.GetService<ILoggerFactory>().CreateLogger(GetType());
            KdyConfiguration = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IConfiguration>();
            LoginUserInfo = KdyBaseServiceProvider.ServiceProvide.GetService<ILoginUserInfo>();
            KdyRedisCache = KdyBaseServiceProvider.ServiceProvide.GetService<IKdyRedisCache>();

            UnitOfWork = unitOfWork;
            //UnitOfWork = KdyBaseServiceProvider.HttpContextServiceProvide.GetService<IUnitOfWork>();
            CanUpdateFieldList = new List<string>();
        }

        /// <summary>
        /// 获取缓存
        /// todo:待改造
        /// </summary>
        /// <returns></returns>
        protected async Task<T> GetCacheValueAsync<T>(string cacheKey, Func<Task<T>> action, TimeSpan? cacheTime = null)
        {
            var redisDb = KdyRedisCache.GetDb(1);
            var cacheV = await redisDb.GetValueAsync<T>(cacheKey);
            if (cacheV != null)
            {
                return cacheV;
            }

            if (cacheTime == null)
            {
                cacheTime = TimeSpan.FromMinutes(5);
            }

            var result = KdyAsyncHelper.Run(action);
            await redisDb.SetValueAsync(cacheKey, result, cacheTime);
            return result;
        }

        public virtual void Dispose()
        {
            UnitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
