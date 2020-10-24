using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.InterfaceFlag;

namespace KdyWeb.BaseInterface.Repository
{
    /// <summary>
    /// 基础仓储 接口
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TKey">主键</typeparam>
    public interface IKdyRepository<TEntity, TKey> : IKdyScoped
        where TEntity : class
        where TKey : struct
    {
        /// <summary>
        /// 生成查询
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery();

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        Task<PageList<TEntity>> GetPageListAsync(int page, int pageSize, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        Task<int> DeleteAsync(TEntity entity);

        /// <summary>
        /// 硬删除
        /// </summary>
        /// <returns></returns>
        Task<int> DeleteAndRemoveAsync(TEntity entity);

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        Task CreateAsync(TEntity entity);

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类</typeparam>
        /// <typeparam name="TDto">Dto</typeparam>
        /// <param name="page">页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="whereExpression">表达式</param>
        /// <returns></returns>
        Task<PageList<TDto>> GetDtoPageListAsync<TDto>(int page, int pageSize, Expression<Func<TEntity, bool>> whereExpression);

    }

    /// <summary>
    /// Int基础仓储 接口
    /// </summary>
    public interface IKdyRepository<TEntity> : IKdyRepository<TEntity, int>
        where TEntity : class
    {

    }
}
