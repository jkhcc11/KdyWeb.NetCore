using System;
using System.Linq.Expressions;
using System.Reflection;
using KdyWeb.BaseInterface.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KdyWeb.EntityFramework
{
    /// <summary>
    /// Ef 查询扩展
    /// </summary>
    public static class QueryExtension
    {
        /// <summary>
        /// 添加软删除过滤器
        /// </summary>
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            //https://www.cnblogs.com/willick/p/13358580.html
            var methodToCall = typeof(QueryExtension)
                .GetMethod(nameof(GetSoftDeleteFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityData.ClrType);//获取泛型方法
            if (methodToCall == null)
            {
                return;
            }

            //调用泛型方法,执行方法返回的结果为object
            var filter = methodToCall.Invoke(null, new object[] { });
            entityData.SetQueryFilter((LambdaExpression)filter);
        }

        /// <summary>
        /// 动态软删除表达式生成
        /// </summary>
        /// <returns></returns>
        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => x.IsDelete == false;
            return filter;
        }
    }
}
