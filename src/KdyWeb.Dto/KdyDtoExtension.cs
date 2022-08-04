using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.Dto
{
    public static class KdyDtoExtension
    {
        /// <summary>
        /// 添加AutoMapper注入
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddKdyMapper(this IServiceCollection services)
        {
            //AutoMapper注入
            //https://www.codementor.io/zedotech/how-to-using-automapper-on-asp-net-core-3-0-via-dependencyinjection-zq497lzsq
            //services.AddAutoMapper(typeof(KdyMapperInit));
            var dtoAssembly = typeof(KdyMapperInit).Assembly;
            var entityAssembly = typeof(BaseEntity<>).Assembly;
            services.AddAutoMapper(dtoAssembly, entityAssembly);

            return services;
        }
    }
}
