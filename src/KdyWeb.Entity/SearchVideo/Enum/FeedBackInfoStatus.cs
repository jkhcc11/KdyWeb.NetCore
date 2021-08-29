using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 反馈状态
    /// </summary>
    public enum FeedBackInfoStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [Display(Name = "待审核")]
        Pending = 0,

        /// <summary>
        /// 处理中
        /// </summary>
        [Display(Name = "处理中")]
        Processing = 5,

        /// <summary>
        /// 正常
        /// </summary>
        [Display(Name = "正常")]
        Normal = 10,

        /// <summary>
        /// 忽略
        /// </summary>
        [Display(Name = "忽略")]
        Ignore = 11,
    }
}
