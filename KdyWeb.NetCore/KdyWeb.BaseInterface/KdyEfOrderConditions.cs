namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// EF条件表达式
    /// </summary>
    public class KdyEfOrderConditions
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="key">字段</param>
        /// <param name="orderBy">排序</param>
        public KdyEfOrderConditions(string key, KdyEfOrderBy orderBy)
        {
            Key = key;
            OrderBy = orderBy;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public KdyEfOrderBy OrderBy { get; set; }
    }

    /// <summary>
    /// 排序
    /// </summary>
    public enum KdyEfOrderBy
    {
        /// <summary>
        /// 升序
        /// </summary>
        Asc,

        /// <summary>
        /// 降序
        /// </summary>
        Desc
    }
}
