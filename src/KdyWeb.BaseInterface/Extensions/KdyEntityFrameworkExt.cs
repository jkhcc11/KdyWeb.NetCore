using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.BaseInterface.Extensions
{
    /// <summary>
    /// EF扩展
    /// </summary>
    /// <remarks>
    /// 基于Linq.Dynamic.Core的一些扩展
    /// </remarks>
    public static class KdyEntityFrameworkExt
    {
        /// <summary>
        /// 排序扩展
        /// </summary>
        /// <returns></returns>
        public static IQueryable<TEntity> KdyThenOrderBy<TEntity>(this IOrderedQueryable<TEntity> query, ISortInput sortInput)
        {
            if (sortInput == null ||
                sortInput.OrderBy == null ||
                sortInput.OrderBy.Any() == false)
            {
                return query;
            }

            var orderByStr = new StringBuilder();

            foreach (var item in sortInput.OrderBy)
            {
                if (string.IsNullOrEmpty(item.Key))
                {
                    continue;
                }

                orderByStr.Append($"{item.Key} {item.OrderBy},");
            }

            return query.ThenBy(orderByStr.ToString().Trim(','));
        }

        /// <summary>
        /// 排序扩展
        /// </summary>
        /// <returns></returns>
        public static IQueryable<TEntity> KdyOrderBy<TEntity>(this IQueryable<TEntity> query, ISortInput sortInput)
        {
            if (sortInput == null ||
                sortInput.OrderBy == null ||
                sortInput.OrderBy.Any() == false)
            {
                return query;
            }

            var orderByStr = new StringBuilder();

            foreach (var item in sortInput.OrderBy)
            {
                if (string.IsNullOrEmpty(item.Key))
                {
                    continue;
                }

                orderByStr.Append($"{item.Key} {item.OrderBy},");
            }

            return query.OrderBy(orderByStr.ToString().Trim(','));

        }

        /// <summary>
        /// 分页扩展
        /// </summary>
        /// <returns></returns>
        public static IQueryable<TEntity> KdyPageList<TEntity>(this IQueryable<TEntity> query, IPageInput input)
        {
            if (input == null)
            {
                return query;
            }

            var skip = (input.Page - 1) * input.PageSize;
            return query.Skip(skip)
                .Take(input.PageSize);
        }

        /// <summary>
        /// 生成动态表达式
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <param name="query">实体类Queryable</param>
        /// <param name="input">Input入参</param>
        /// <returns></returns>
        public static IQueryable<TEntity> CreateConditions<TEntity>(this IQueryable<TEntity> query, object input)
        {
            var properties = input.GetType().GetProperties();
            if (properties.Any() == false)
            {
                return query;
            }

            //遍历所有公用属性
            foreach (var propItem in properties)
            {
                //判断是否有值
                var objValue = propItem.GetValue(input);
                if (objValue == null ||
                    string.IsNullOrEmpty(objValue.ToString()))
                {
                    continue;
                }

                //值类型默认值不生成表达式
                if ((objValue is long ||
                     objValue is int ||
                     objValue is decimal
                     ) && long.TryParse(objValue.ToString(), out long temp)
                       && temp == 0)
                {
                    continue;
                }


                //获取属性字段含有KdyQuery特性标签
                var kdyQueryList = propItem.GetCustomAttributes(typeof(KdyQueryAttribute), false)
                    .Distinct()
                    .Select(a => (KdyQueryAttribute)a)
                    .ToList();
                if (kdyQueryList.Any() == false)
                {
                    continue;
                }

                //或
                var orConditionsList = new List<string>();
                var value = new List<object>();
                for (int i = 0; i < kdyQueryList.Count; i++)
                {
                    var kdyQueryItem = kdyQueryList[i];
                    value.Add(objValue);

                    //非 谓词
                    var notPredicate = "";
                    switch (kdyQueryItem.Operator)
                    {
                        case KdyOperator.NotStartsWith:
                        case KdyOperator.NotEndsWith:
                        case KdyOperator.NotInclude:
                        case KdyOperator.NotLike:
                            {
                                notPredicate = "==false";
                                break;
                            }
                    }

                    switch (kdyQueryItem.Operator)
                    {
                        case KdyOperator.NotStartsWith:
                        case KdyOperator.StartsWith:
                            {
                                orConditionsList.Add($"{kdyQueryItem.Field}.StartsWith(@{i}){notPredicate}");
                                continue;
                            }
                        case KdyOperator.NotEndsWith:
                        case KdyOperator.EndsWith:
                            {
                                orConditionsList.Add($"{kdyQueryItem.Field}.EndsWith(@{i}){notPredicate}");
                                continue;
                            }
                        case KdyOperator.Include:
                        case KdyOperator.NotInclude:
                            {
                                orConditionsList.Add($"@{i}.Contains({kdyQueryItem.Field}){notPredicate}");
                                continue;
                            }
                        case KdyOperator.NotLike:
                        case KdyOperator.Like:
                            {
                                orConditionsList.Add($"{kdyQueryItem.Field}.Contains(@{i}){notPredicate}");
                                continue;
                            }
                    }

                    orConditionsList.Add($"{kdyQueryItem.Field}{kdyQueryItem.Operator.GetDisplayName()}@{i}");
                }

                if (orConditionsList.Count > 0)
                {
                    //大于1说明是多条件 或 
                    query = query.Where($"{string.Join(" Or ", orConditionsList)}", value.ToArray());
                }
            }

            return query;
        }
    }
}
