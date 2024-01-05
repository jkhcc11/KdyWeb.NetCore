using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 通过豆瓣信息更新影片信息
    /// </summary>
    public class UpdateVodForDouBanInfoInput
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
        /// 剧集播放Url
        /// </summary>
        /// <remarks>
        /// 多个多行隔开
        /// </remarks>
        [Required(ErrorMessage = "剧集播放Url必填")]
        public List<VideoEpisodeDto> EpItems { get; set; }

        /// <summary>
        /// 影片Id
        /// </summary>
        public long VodId { get; set; }

        /// <summary>
        /// 剧集组Id
        /// </summary>
        public long EpGroupId { get; set; }
    }
}
