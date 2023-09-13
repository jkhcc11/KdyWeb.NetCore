using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
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

        protected KdyRepository(IUnitOfWork unitOfWork)
        {
            // var unitOfWork = KdyBaseServiceProvider.HttpContextServiceProvide.GetService<IUnitOfWork>();
            DbSet = unitOfWork.GetCurrentDbContext(ReadWrite.Read).Set<TEntity>();
            WriteDbSet = unitOfWork.GetCurrentDbContext(ReadWrite.Write).Set<TEntity>();
            LoginUserInfo = KdyBaseServiceProvider.ServiceProvide.GetService<ILoginUserInfo>();
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

        public IQueryable<TEntity> GetWriteQuery()
        {
            return WriteDbSet.AsQueryable();
        }

        public IQueryable<TEntity> GetWriteAsNoTracking()
        {
            return WriteDbSet.AsNoTracking();
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression)
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
            entity.ModifyUserName = LoginUserInfo.UserName;
            entity.ModifyUserId = LoginUserInfo.UserId;
            entity.ModifyTime = DateTime.Now;
            entity.IsDelete = true;
            WriteDbSet.Update(entity);
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <returns></returns>
        public virtual void Delete(List<TEntity> entity)
        {
            foreach (var item in entity)
            {
                item.ModifyUserName = LoginUserInfo.UserName;
                item.ModifyUserId = LoginUserInfo.UserId;
                item.ModifyTime = DateTime.Now;
                item.IsDelete = true;
            }

            WriteDbSet.UpdateRange(entity);
        }

        /// <summary>
        /// 条件软删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            var list = await WriteDbSet.Where(whereExpression).ToListAsync();
            if (list.Any())
            {
                Delete(list);
            }
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
}
