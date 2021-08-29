using KdyWeb.BaseInterface.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Core;

namespace KdyWeb.BaseInterface.Extensions
{
    /// <summary>
    /// 雪花算法Id扩展
    /// </summary>
    public static class IdGenerateExt
    {
        /// <summary>
        /// 初始化Id生成
        /// </summary>
        public static IServiceCollection InitIdGenerate(this IServiceCollection services, IConfiguration configuration)
        {
            //雪花算法配置
            var section = configuration.GetSection("IdGenerate");
            var workId = section.GetValue("WorkId", 1);
            var centerId = section.GetValue("CenterId", 1);

           // services.AddSingleton<IIdGenerate<long>>(a => new IdGenerateForLong(workId, centerId));
           services.AddSingleton<IdWorker>(a => new IdWorker(workId, centerId)); 
           return services;
        }
    }
}
