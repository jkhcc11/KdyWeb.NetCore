using KdyWeb.Entity.VideoConverts;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 自主发布资源
    /// </summary>
    public class PublishVodInput
    {
        /// <summary>
        /// 资源名
        /// </summary>
        [Required(ErrorMessage = "请输入资源名")]
        [StringLength(VideoConvertTask.TaskNameLength)]
        public string VodName { get; set; }

        /// <summary>
        /// 评分人数
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PersonCount { get; set; }

        /// <summary>
        /// 豆瓣链接
        /// </summary>
        [Required(ErrorMessage = "请输入豆瓣链接")]
        [StringLength(VideoConvertTask.SourceLinkLength)]
        public string SourceLink { get; set; }

        /// <summary>
        /// 网盘资源信息
        /// </summary>
        [Required(ErrorMessage = "请输入网盘资源信息")]
        [StringLength(VideoConvertTask.TaskRemarkLength)]
        public string SourceInfo { get; set; }
    }
}
