using KdyWeb.Entity.VideoConverts.Enum;
using Newtonsoft.Json;

namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 创建影片管理记录 Job Input
    /// </summary>
    public class CreateVodManagerRecordInput
    {
        /// <summary>
        /// 创建影片管理记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="recordType">记录类型</param>
        [JsonConstructor]
        public CreateVodManagerRecordInput(long userId, VodManagerRecordType recordType)
        {
            UserId = userId;
            RecordType = recordType;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string LoginUserName { get; set; }

        /// <summary>
        /// 记录类型
        /// </summary>
        public VodManagerRecordType RecordType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 业务Id
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        /// <remarks>
        ///  默认是自动计算，有些是批量统计总和
        /// </remarks>
        public decimal? CheckoutAmount { get; set; }
    }
}
