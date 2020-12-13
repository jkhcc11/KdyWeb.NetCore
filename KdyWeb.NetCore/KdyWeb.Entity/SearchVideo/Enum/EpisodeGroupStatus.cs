using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 剧集组状态
    /// </summary>
    public enum EpisodeGroupStatus : byte
    {
        /// <summary>
        /// 使用中
        /// </summary>
        [Display(Name = "使用中")]
        Using,

        /// <summary>
        /// 禁用
        /// </summary>
        [Display(Name = "禁用")]
        Ban
    }
}
