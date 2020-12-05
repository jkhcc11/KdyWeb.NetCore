using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        /// <summary>
        /// AutoMapper ProjectTo扩展
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <typeparam name="TDto">Dto</typeparam>
        /// <param name="queryable">IQueryable</param>
        /// <returns></returns>
        public static IQueryable<TDto> ProjectToExt<TEntity, TDto>(this IQueryable<TEntity> queryable)
        where TDto : class, new()
        {
            //todo:IConfigurationProvider 这个必须是引用了 AutoMapper Extensions.DependencyInjection 然后AddAutoMapper后才有
            var configuration = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IConfigurationProvider>();
            return queryable.ProjectTo<TDto>(configuration);
        }
    }
}
