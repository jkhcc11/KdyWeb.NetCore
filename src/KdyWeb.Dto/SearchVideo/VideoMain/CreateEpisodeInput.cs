using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建剧集Input
    /// </summary>
    public class CreateEpisodeInput
    {
        /// <summary>
        /// 影片Id
        /// </summary>
        [Required(ErrorMessage = "影片Id必填")]
        public long MainId { get; set; }

        /// <summary>
        /// 剧集信息
        /// </summary>
        public List<EditEpisodeItem> EpItems { get; set; }
    }

    /// <summary>
    /// 剧集编辑Item
    /// </summary>
    public class EditEpisodeItem
    {
        /// <summary>
        /// 剧集Url
        /// </summary>
        [Required(ErrorMessage = "剧集Url必填")]
        public string EpisodeUrl { get; set; }

        /// <summary>
        /// 剧集名
        /// </summary>
        [Required(ErrorMessage = "剧集名必填")]
        public string EpisodeName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越考前
        /// </remarks>
        public int? OrderBy { get; set; }
    }
}
