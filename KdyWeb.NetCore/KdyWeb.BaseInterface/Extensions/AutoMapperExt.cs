using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.BaseInterface.Extensions
{
    /// <summary>
    /// AutoMapper扩展
    /// </summary>
    public static class AutoMapperExt
    {
        /// <summary>
        ///  AutoMapper自动映射
        /// </summary>
        /// <typeparam name="TSource">源实体 泛型</typeparam>
        /// <typeparam name="TTarget">目标实体 泛型</typeparam>
        /// <param name="source">源实体类</param>
        /// <returns></returns>
        public static TTarget MapToExt<TSource, TTarget>(this TSource source)
        {
            var mapper = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IMapper>();
            return mapper.Map<TSource, TTarget>(source);
        }

        /// <summary>
        ///  AutoMapper自动映射
        /// </summary>
        /// <typeparam name="TTarget">目标实体 泛型</typeparam>
        /// <param name="source">源实体类</param>
        /// <returns></returns>
        public static TTarget MapToExt<TTarget>(this object source)
        {
            var mapper = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IMapper>();
            return mapper.Map<TTarget>(source);
        }

        /// <summary>
        ///  AutoMapper自动映射
        /// </summary>
        /// <typeparam name="TTarget">目标实体 泛型</typeparam>
        /// <param name="source">源实体类</param>
        /// <returns></returns>
        public static IList<TTarget> MapToListExt<TTarget>(this object source)
        {
            var mapper = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IMapper>();
            return mapper.Map<List<TTarget>>(source);
        }
    }
}
