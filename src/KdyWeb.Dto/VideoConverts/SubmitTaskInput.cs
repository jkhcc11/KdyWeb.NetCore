using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.VideoConverts;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 提交任务
    /// </summary>
    public class SubmitTaskInput
    {
        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(VideoConvertTask.SourceLinkLength)]
        [Required(ErrorMessage = "请输入分享内容")]
        public string Context { get; set; }

        /// <summary>
        /// 任务ids
        /// </summary>
        [Required(ErrorMessage = "缺少编码")]
        public List<long> TaskIds { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(VideoConvertTask.TaskRemarkLength)]
        public string Remark { get; set; }
    }
}
