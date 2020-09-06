using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 基础服务定义 实现
    /// </summary>
    public class BaseKdyService : IKdyService
    {
        /// <summary>
        /// 根据Key 获取配置信息
        /// </summary>
        /// <param name="key">配置Key</param>
        /// <returns></returns>
        public T GetConfig<T>(string key)
        {
            var config = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IConfiguration>();
            return config.GetValue<T>(key);
        }
    }
}
