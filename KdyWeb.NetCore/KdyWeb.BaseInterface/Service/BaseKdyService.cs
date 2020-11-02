using KdyWeb.BaseInterface.KdyLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 基础服务定义 实现
    /// </summary>
    public abstract class BaseKdyService : IKdyService
    {
        /// <summary>
        /// 统一日志
        /// </summary>
        protected readonly IKdyLog KdyLog;
        /// <summary>
        /// 统一配置
        /// </summary>
        protected readonly IConfiguration KdyConfiguration;

        protected BaseKdyService()
        {
            KdyLog = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IKdyLog>();
            KdyConfiguration = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IConfiguration>();
        }

    }
}
