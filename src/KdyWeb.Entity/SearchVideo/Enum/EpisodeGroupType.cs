using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 剧集组类型
    /// </summary>
    public enum EpisodeGroupType : byte
    {
        /// <summary>
        /// 视频播放
        /// </summary>
        [Display(Name = "视频播放")]
        VideoPlay = 1,

        /// <summary>
        /// 视频下载
        /// </summary>
        [Display(Name = "视频下载")]
        VideoDown = 2
    }
}
