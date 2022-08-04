using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.VideoConverts.Enum
{
    /// <summary>
    /// 转换订单状态
    /// </summary>
    public enum ConvertOrderStatus : byte
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Display(Name = "初始化")]
        Init = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        [Display(Name = "已完成")]
        Finish = 5,

        /// <summary>
        /// 作废
        /// </summary>
        [Display(Name = "作废")]
        Invalid = 6
    }
}
