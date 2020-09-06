using System;
using KdyWeb.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// Map基类
    /// </summary>
    /// <typeparam name="TEntity">泛型 必需是类 且 继承IBaseKey</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public abstract class KdyBaseMap<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IBaseKey<TKey>
        where TKey : struct
    {
        /// <summary>
        /// 表名
        /// </summary>
        private readonly string _tableName;
        /// <summary>
        /// 表名
        /// </summary>
        /// <param name="tableName">表名 可不填写 默认与实体类一致</param>
        protected KdyBaseMap(string tableName = "")
        {
            _tableName = tableName;
        }

        /// <summary>
        /// 实现IEntityTypeConfiguration实体映射关系方法
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (string.IsNullOrEmpty(_tableName) == false)
            {
                builder.ToTable(_tableName);
            }

            //默认主键为Id
            builder.HasKey(a => a.Id);

            //EF并发控制字段
            if (typeof(IRowVersion).IsAssignableFrom(typeof(TEntity)))
            {
                builder.Property(nameof(IRowVersion.RowVersion)).IsRowVersion();
            }

            //long类型非自增
            if (typeof(long) == typeof(TKey))
            {
                builder.Property(a => a.Id).ValueGeneratedNever();
            }

            builder.Property(a => a.IsDelete).HasDefaultValue(false);

            MapperConfigure(builder);
        }

        /// <summary>
        /// 若多余Mapping关系，重写此方法
        /// </summary>
        public virtual void MapperConfigure(EntityTypeBuilder<TEntity> builder)
        {
        }
    }
}
