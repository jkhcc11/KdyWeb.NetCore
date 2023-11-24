namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 更新用户备注
    /// </summary>
    public class UpdateUserRemarkInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
