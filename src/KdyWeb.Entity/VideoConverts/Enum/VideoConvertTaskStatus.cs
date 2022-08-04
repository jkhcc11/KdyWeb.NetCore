using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.VideoConverts.Enum
{
    /// <summary>
    /// 视频转码任务状态
    /// </summary>
    public enum VideoConvertTaskStatus : byte
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [Display(Name = "待处理")]
        Waiting = 1,

        /// <summary>
        /// 进行中
        /// </summary>
        [Display(Name = "进行中")]
        Padding = 5,

        /// <summary>
        /// 审核中
        /// </summary>
        [Display(Name = "审核中")]
        Auditing = 8,

        /// <summary>
        /// 已完成
        /// </summary>
        [Display(Name = "已完成")]
        Finish = 10
    }
}
