namespace KdyWeb.Dto
{
    /// <summary>
    /// 更新字段值
    /// </summary>
    public class UpdateValueByFieldInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 待更新字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段新值
        /// </summary>
        public string Value { get; set; }
    }
}
