using KdyWeb.BaseInterface.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KdyWeb.Repository
{
    public static class KdyRepositoryExtension
    {
        /// <summary>
        /// 添加所有泛型仓储
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddKdyAllRepository(this IServiceCollection services)
        {
            services.TryAdd(ServiceDescriptor.Transient(typeof(IKdyRepository<>), typeof(CommonRepository<>)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IKdyRepository<,>), typeof(CommonRepository<,>)));

            return services;
        }
    }
}
