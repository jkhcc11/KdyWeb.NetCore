using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 通过豆瓣信息创建影片信息（新版）
    /// </summary>
    public class CreateForDouBanInfoNewInput
    {
        /// <summary>
        /// 系列Id
        /// </summary>
        public long? SeriesId { get; set; }

        /// <summary>
        /// 豆瓣信息Id
        /// </summary>
        [Range(minimum: 1, maximum: 9999999)]
        public int DouBanInfoId { get; set; }

        /// <summary>
        /// 剧集组类型
        /// </summary>
        public EpisodeGroupType EpisodeGroupType { get; set; } = EpisodeGroupType.VideoPlay;

        /// <summary>
        /// 剧集播放Url
        /// </summary>
        /// <remarks>
        /// 多个多行隔开
        /// </remarks>
        [Required(ErrorMessage = "剧集播放Url必填")]
        public List<VideoEpisodeDto> EpItems { get; set; }
    }
}
