namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// 分页入参
    /// </summary>
    public abstract class BasePageInput
    {
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

                if (value >= 100)
                {
                    _pageSize = 100;
                    return;
                }

                _pageSize = value;
            }
        }
    }
}
