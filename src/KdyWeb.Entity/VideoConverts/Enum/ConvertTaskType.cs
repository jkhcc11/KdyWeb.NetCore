using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.VideoConverts.Enum
{
    /// <summary>
    /// 转换任务类型
    /// </summary>
    public enum ConvertTaskType : byte
    {
        /// <summary>
        /// 转码
        /// </summary>
        [Display(Name = "转码")]
        Convert = 1,

        /// <summary>
        /// 迁移
        /// </summary>
        [Display(Name = "迁移")]
        Move
    }
}
