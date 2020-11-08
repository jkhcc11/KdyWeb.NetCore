using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧版剧集
    /// </summary>
    public class OldSearchSysEpisode
    {
        /// <summary>
        /// 剧集Id
        /// </summary>
        [Required]
        [Key]
        public int EpId { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        [Required]
        public int KeyId { get; set; }

        /// <summary>
        /// 剧集Url
        /// </summary>
        public string EpisodeUrl { get; set; }

        /// <summary>
        /// 剧集名
        /// </summary>
        public string EpisodeName { get; set; }
    }
}
