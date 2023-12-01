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
        Move,

        /// <summary>
        /// 找资源
        /// </summary>
        /// <remarks>
        /// 管理员发布
        /// </remarks>
        [Display(Name = "找资源")]
        FindVod = 10,

        /// <summary>
        /// 发布资源
        /// </summary>
        /// <remarks>
        ///  用户发布
        /// </remarks>
        [Display(Name = "发布资源")]
        PublishVod
    }
}
