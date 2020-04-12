﻿using System.Collections.Generic;

namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// 分页数据返回
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    public class PageList<TEntity>
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数据大小
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        public ICollection<TEntity> Data { get; set; }
    }
}
