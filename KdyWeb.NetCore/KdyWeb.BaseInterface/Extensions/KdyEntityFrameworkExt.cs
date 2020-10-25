using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KdyWeb.BaseInterface.Extensions
{
    /// <summary>
    /// EF扩展
    /// </summary>
    public static class KdyEntityFrameworkExt
    {
        /// <summary>
        /// 排序扩展
        /// </summary>
        /// <returns></returns>
        public static IQueryable<T> KdyOrderBy<T>(this IQueryable<T> query, IEnumerable<KdyEfOrderConditions> orderConditions)
        {
            //https://www.cnblogs.com/liyouming/p/9402113.html
            foreach (var item in orderConditions)
            {
                //找到实体类中Key对应的属性
                var entity = typeof(T);
                var propertyInfo = entity.GetProperty(item.Key);
                if (propertyInfo == null)
                {
                    continue;
                }

                var parameter = Expression.Parameter(entity);
                Expression propertySelector = Expression.Property(parameter, propertyInfo);

                if (propertyInfo.PropertyType == typeof(int))
                {
                    var orderExpression = Expression.Lambda<Func<T, int>>(propertySelector, parameter);
                    query = item.OrderBy == KdyEfOrderBy.Desc ? query.OrderByDescending(orderExpression) : query.OrderBy(orderExpression);
                    continue;
                }

                if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    var orderExpression = Expression.Lambda<Func<T, DateTime>>(propertySelector, parameter);
                    query = item.OrderBy == KdyEfOrderBy.Desc ? query.OrderByDescending(orderExpression) : query.OrderBy(orderExpression);
                }
                else
                {
                    var orderExpression = Expression.Lambda<Func<T, object>>(propertySelector, parameter);
                    query = item.OrderBy == KdyEfOrderBy.Desc ? query.OrderByDescending(orderExpression) : query.OrderBy(orderExpression);
                }
            }
            return query;
        }
    }
}
