using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧版剧集
    /// </summary>
    public class OldSearchSysEpisode : BaseEntity<int>
    {
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

        public virtual OldSearchSysMain Main { get; set; }
    }
}
