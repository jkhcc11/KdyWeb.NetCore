using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.BaseInterface.Repository
{
    /// <summary>
    /// 基础仓储 抽象类
    /// </summary>
    /// <typeparam name="TEntity">数据库实体类</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public abstract class KdyRepository<TEntity, TKey> : IKdyRepository<TEntity, TKey>
        where TEntity : class, IBaseKey<TKey>
        where TKey : struct
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly DbContext _dbContext;
        /// <summary>
        /// DbSet
        /// </summary>
        protected DbSet<TEntity> DbSet;

        protected KdyRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// 生成带实体跟踪查询
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetQuery()
        {
            return DbSet.AsQueryable();
        }

        /// <summary>
        /// 生成不带实体跟踪查询
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAsNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await DbSet.FirstOrDefaultAsync(whereExpression);
        }

        /// <summary>
        /// 条件全部获取列表
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await GetQuery().Where(whereExpression).ToListAsync();
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="whereExpression">条件</param>
        /// <returns></returns>
        public virtual async Task<PageList<TEntity>> GetPageListAsync(int page, int pageSize, Expression<Func<TEntity, bool>> whereExpression)
        {
            var result = new PageList<TEntity>();
            var query = GetQuery();
            var skip = (page - 1) * pageSize;
            result.DataCount = query.Count(whereExpression);
            result.Page = page;
            result.PageSize = pageSize;
            if (result.DataCount <= 0)
            {
                result.Data = new List<TEntity>();
                return result;
            }

            result.Data = await query.Where(whereExpression).Skip(skip).Take(pageSize).ToListAsync();
            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.ModifyTime = DateTime.Now;
            DbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            entity.ModifyTime = DateTime.Now;
            entity.IsDelete = true;
            DbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 硬删除
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> DeleteAndRemoveAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public virtual async Task CreateAsync(TEntity entity)
        {
            entity.CreatedTime = DateTime.Now;
            await DbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<PageList<TDto>> GetDtoPageListAsync<TDto>(int page, int pageSize, Expression<Func<TEntity, bool>> whereExpression, IList<KdyEfOrderConditions> orderBy = null)
        {
            var result = new PageList<TDto>();
            var query = GetQuery();
            var skip = (page - 1) * pageSize;
            result.DataCount = query.Count(whereExpression);
            result.Page = page;
            result.PageSize = pageSize;
            if (result.DataCount <= 0)
            {
                result.Data = new List<TDto>();
                return result;
            }

            var dbQuery = query.Where(whereExpression);
            if (orderBy != null)
            {
                dbQuery = dbQuery.KdyOrderBy(orderBy);
            }
            else
            {
                dbQuery = dbQuery.KdyOrderBy(new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions("Id", KdyEfOrderBy.Desc)
                });
            }

            var dbList = await dbQuery
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            result.Data = dbList.MapToListExt<TDto>();
            return result;
        }
    }
}
