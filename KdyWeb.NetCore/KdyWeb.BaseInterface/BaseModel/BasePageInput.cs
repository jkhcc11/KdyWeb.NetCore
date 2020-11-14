using System.Collections.Generic;

namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// 基类分页 入参
    /// </summary>
    public abstract class BasePageInput : IPageInput
    {
        /// <summary>
        /// 最大分页
        /// </summary>
        public const int MaxPageSize = 100;

        /// <summary>
        /// 页数
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        private int _pageSize { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize <= 0)
                {
                    return 10;
                }

                return _pageSize;
            }
            //set => _pageSize = value > 100 ? 100 : (value <= 0 ? 10 : value);
            set
            {
                if (value <= 0)
                {
                    _pageSize = 10;
                    return;
                }

                if (value >= MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                    return;
                }

                _pageSize = value;
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public IList<KdyEfOrderConditions> OrderBy { get; set; }
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
        IList<KdyEfOrderConditions> OrderBy { get; set; }
    }
}
