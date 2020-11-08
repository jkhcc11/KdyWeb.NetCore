using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.Repository
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
        /// DbSet
        /// </summary>
        protected DbSet<TEntity> DbSet;
        /// <summary>
        /// 写库
        /// </summary>
        protected DbSet<TEntity> WriteDbSet;
        /// <summary>
        /// 用户登录信息
        /// </summary>
        protected ILoginUserInfo LoginUserInfo;

        protected KdyRepository()
        {
            var unitOfWork = KdyBaseServiceProvider.HttpContextServiceProvide.GetService<IUnitOfWork>();
            DbSet = unitOfWork.GetCurrentDbContext(ReadWrite.Read).Set<TEntity>();
            WriteDbSet = unitOfWork.GetCurrentDbContext(ReadWrite.Write).Set<TEntity>();
            LoginUserInfo = KdyBaseServiceProvider.HttpContextServiceProvide.GetService<ILoginUserInfo>();
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
        /// 更新
        /// </summary>
        /// <returns></returns>
        public virtual TEntity Update(TEntity entity)
        {
            WriteDbSet.Update(entity);
            return entity;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <returns></returns>
        public virtual void Update(List<TEntity> entity)
        {
            //foreach (var item in entity)
            //{
            //    item.ModifyUserId = LoginUserInfo.UserId;
            //    // item.ModifyTime = DateTime.Now;
            //}

            WriteDbSet.UpdateRange(entity);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        public virtual void Delete(TEntity entity)
        {
            entity.ModifyUserId = LoginUserInfo.UserId;
            entity.ModifyTime = DateTime.Now;
            entity.IsDelete = true;
            WriteDbSet.Update(entity);
        }

        /// <summary>
        /// 硬删除
        /// </summary>
        /// <returns></returns>
        public virtual void DeleteAndRemove(TEntity entity)
        {
            WriteDbSet.Remove(entity);
            //return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
           // entity.CreatedUserId = LoginUserInfo.UserId;
            await WriteDbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// 批量创建
        /// </summary>
        /// <returns></returns>
        public async Task CreateAsync(List<TEntity> entity)
        {
            //foreach (var item in entity)
            //{
            //    item.CreatedUserId = LoginUserInfo.UserId;
            //}

            await WriteDbSet.AddRangeAsync(entity);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// 仓储扩展
    /// </summary>
    public static class KdyRepositoryExt
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <returns></returns>
        public static async Task<PageList<TEntity>> GetPageListAsync<TEntity>(this IQueryable<TEntity> dbQuery, object input)
        {
            var pageInput = input as IPageInput;
            if (pageInput == null)
            {
                return new PageList<TEntity>(0, 0);
            }

            var result = new PageList<TEntity>(pageInput.Page, pageInput.PageSize);
            dbQuery = dbQuery.CreateConditions(input);
            result.DataCount = await dbQuery.CountAsync();
            if (result.DataCount <= 0)
            {
                result.Data = new List<TEntity>();
                return result;
            }

            if (pageInput.OrderBy != null)
            {
                dbQuery = dbQuery.KdyOrderBy(pageInput);
            }

            result.Data = await dbQuery.KdyPageList(pageInput).ToListAsync();
            return result;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类</typeparam>
        /// <typeparam name="TDto">Dto</typeparam>
        /// <returns></returns>
        public static async Task<PageList<TDto>> GetDtoPageListAsync<TEntity, TDto>(this IQueryable<TEntity> dbQuery, object input)
            where TDto : class
        {
            var pageInput = input as IPageInput;
            if (pageInput == null)
            {
                return new PageList<TDto>(0, 0);
            }

            var dbResult = await dbQuery.GetPageListAsync(input);
            var result = new PageList<TDto>(pageInput.Page, pageInput.PageSize)
            {
                DataCount = dbResult.DataCount,
                Data = dbResult.Data.MapToListExt<TDto>()
            };
            return result;
        }
    }
}
