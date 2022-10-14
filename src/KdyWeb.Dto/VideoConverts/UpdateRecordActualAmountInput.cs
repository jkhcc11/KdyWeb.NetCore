namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 更新记录实际金额
    /// </summary>
    public class UpdateRecordActualAmountInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long KeyId { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal ActualAmount { get; set; }
    }
}
