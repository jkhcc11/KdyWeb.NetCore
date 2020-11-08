using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 剧集表
    /// </summary>
    public class VideoEpisode : BaseEntity<long>
    {
        /// <summary>
        /// 剧集名长度
        /// </summary>
        public const int EpisodeNameLength = 80;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="episodeName">剧集名</param>
        /// <param name="episodeUrl">剧集Url</param>
        public VideoEpisode(string episodeName, string episodeUrl)
        {
            EpisodeName = episodeName;
            EpisodeUrl = episodeUrl;
        }

        /// <summary>
        /// 剧集Url
        /// </summary>
        [StringLength(VideoMain.UrlLength)]
        [Required]
        public string EpisodeUrl { get; set; }

        /// <summary>
        /// 剧集名
        /// </summary>
        [StringLength(EpisodeNameLength)]
        [Required]
        public string EpisodeName { get; set; }

        /// <summary>
        /// 备注扩展
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 剧集组Id
        /// </summary>
        public long EpisodeGroupId { get; set; }

        /// <summary>
        /// 剧集组
        /// </summary>
        public virtual VideoEpisodeGroup VideoEpisodeGroup { get; set; }

    }
}
