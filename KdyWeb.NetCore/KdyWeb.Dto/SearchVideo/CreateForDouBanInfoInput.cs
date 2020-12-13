using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 通过豆瓣信息创建影片信息 input
    /// </summary>
    public class CreateForDouBanInfoInput
    {
        /// <summary>
        /// 豆瓣信息Id
        /// </summary>
        [Range(minimum: 1, maximum: 9999999)]
        public int DouBanInfoId { get; set; }

        /// <summary>
        /// 剧集组类型
        /// </summary>
        public EpisodeGroupType EpisodeGroupType { get; set; }

        /// <summary>
        /// 剧集播放Url
        /// </summary>
        /// <remarks>
        /// 单地址
        /// </remarks>
        [Required(ErrorMessage = "剧集播放Url必填")]
        public string EpUrl { get; set; }
    }
}
