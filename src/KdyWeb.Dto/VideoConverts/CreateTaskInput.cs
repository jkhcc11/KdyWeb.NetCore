using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 创建任务
    /// </summary>
    public class CreateTaskInput
    {
        /// <summary>
        /// 任务名
        /// </summary>
        [StringLength(VideoConvertTask.TaskNameLength)]
        [Required]
        public string TaskName { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public ConvertTaskType TaskType { get; set; }

        /// <summary>
        /// 任务获取积分
        /// </summary>
        public decimal GiftPoints { get; set; }

        /// <summary>
        /// 资源链接类型
        /// </summary>
        public SourceLinkType SourceLinkType { get; set; }

        /// <summary>
        /// 资源链接
        /// </summary>
        [StringLength(VideoConvertTask.SourceLinkLength)]
        public string SourceLink { get; set; }

        /// <summary>
        /// 资源链接扩展信息
        /// </summary>
        /// <remarks>
        /// 提取码等
        /// </remarks>
        [StringLength(VideoConvertTask.SourceLinkExtLength)]
        public string SourceLinkExt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(VideoConvertTask.TaskRemarkLength)]
        public string TaskRemark { get; set; }

    }
}
