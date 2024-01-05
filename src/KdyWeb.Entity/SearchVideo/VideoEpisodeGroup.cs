using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 剧集组
    /// </summary>
    public class VideoEpisodeGroup : BaseEntity<long>
    {
        public const string DownGroupName = "默认下载";
        public const string PlayGroupName = "默认播放";

        /// <summary>
        /// 剧集组名长度
        /// </summary>
        public const int GroupNameLength = 50;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="episodeGroupType">组类型</param>
        public VideoEpisodeGroup(EpisodeGroupType episodeGroupType, string groupName)
        {
            GroupName = groupName;
            EpisodeGroupType = episodeGroupType;
            EpisodeGroupStatus = EpisodeGroupStatus.Using;
        }

        /// <summary>
        /// 剧集组名
        /// </summary>
        [Required]
        [StringLength(GroupNameLength)]
        public string GroupName { get; set; }

        /// <summary>
        /// 剧集组状态
        /// </summary>
        [Required]
        public EpisodeGroupStatus EpisodeGroupStatus { get; set; }

        /// <summary>
        /// 剧集组类型
        /// </summary>
        [Required]
        public EpisodeGroupType EpisodeGroupType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越考前
        /// </remarks>
        public int OrderBy { get; set; }

        /// <summary>
        /// 影片主表Id
        /// </summary>
        public long MainId { get; set; }

        /// <summary>
        /// 影片主表
        /// </summary>
        public virtual VideoMain? VideoMain { get; set; }

        /// <summary>
        /// 剧集
        /// </summary>
        public virtual ICollection<VideoEpisode>? Episodes { get; set; }
    }
}
