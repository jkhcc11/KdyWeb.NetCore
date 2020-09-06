using System;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity
{
    /// <summary>
    /// 是否激活
    /// </summary>
    public interface IsActivate
    {
        /// <summary>
        /// 是否激活
        /// </summary>
        bool IsActivate { set; get; }
    }

    /// <summary>
    /// EF行版本并发字段
    /// </summary>
    public interface IRowVersion
    {
        /// <summary>
        /// 并发控制字段
        /// </summary>
        byte[] RowVersion { get; set; }
    }

    /// <summary>
    /// 主键接口
    /// </summary>
    public interface IBaseKey<TKey> : IBaseTimeKey
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDelete { get; set; }
    }

    /// <summary>
    /// 创建时间接口
    /// </summary>
    public interface IBaseTimeKey
    {
        /// <summary>
        /// 创建用户Id
        /// </summary>
        int? CreatedUserId { get; set; }

        /// <summary>
        /// 修改用户Id
        /// </summary>
        int? ModifyUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? ModifyTime { get; set; }
    }

    /// <summary>
    /// 基类 包含是否删除，创建修改时间
    /// </summary>
    public class BaseEntity<TKey> : IBaseKey<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Required]
        public TKey Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int? CreatedUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        public int? ModifyUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
