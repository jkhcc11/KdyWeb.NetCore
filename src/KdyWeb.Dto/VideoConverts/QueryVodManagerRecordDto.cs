using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询影片管理者记录 Dto
    /// </summary>
    [AutoMap(typeof(VodManagerRecord))]
    public class QueryVodManagerRecordDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal CheckoutAmount { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal ActualAmount { get; set; }

        /// <summary>
        /// 记录类型
        /// </summary>
        public VodManagerRecordType RecordType { get; set; }

        /// <summary>
        /// 记录类型Str
        /// </summary>
        public string RecordTypeStr => RecordType.GetDisplayName();

        /// <summary>
        /// 是否已结算
        /// </summary>
        public bool IsCheckout { get; set; }

        /// <summary>
        /// 是否为有效记录
        /// </summary>
        /// <remarks>
        /// 重复提交的就是false
        /// </remarks>
        public bool IsValid { get; set; }

        /// <summary>
        /// 创建用户名
        /// </summary>
        public string CreatedUserName { get; set; }

        /// <summary>
        /// 修改用户名
        /// </summary>
        public string ModifyUserName { get; set; }
    }
}
