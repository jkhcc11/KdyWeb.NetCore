using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// 基类分页 入参
    /// </summary>
    public abstract class BasePageInput : IPageInput
    {
        /// <summary>
        /// 页数
        /// </summary>
        [Range(1, 10000, ErrorMessage = "分页错误")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        [Range(1, 100, ErrorMessage = "分页大小错误")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 排序
        /// </summary>
        public IList<KdyEfOrderConditions>? OrderBy { get; set; }
    }

    /// <summary>
    /// 分页入参 接口
    /// </summary>
    public interface IPageInput : ISortInput
    {
        /// <summary>
        /// 页数
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize { get; set; }
    }

    /// <summary>
    /// 排序入参 接口
    /// </summary>
    public interface ISortInput
    {
        /// <summary>
        /// 排序
        /// </summary>
        IList<KdyEfOrderConditions>? OrderBy { get; set; }
    }

    public abstract class BaseDateRangeInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public virtual (DateTime startTime, DateTime endTime) GetRange()
        {
            var startTime = StartTime.HasValue == false ?
                DateTime.Now.Date.AddDays(-7) : StartTime.Value;

            var endTime = EndTime.HasValue == false ?
                DateTime.Now.Date.AddDays(1) :
                EndTime.Value.AddDays(1);
            if ((endTime - startTime).TotalDays > 366)
            {
                //最多一年
                endTime = startTime.AddDays(366);
            }

            return (startTime, endTime);
        }
    }
}
