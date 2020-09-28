using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 影片类型
    /// </summary>
    /// <remarks>
    /// 电影、电视剧、纪录片、综艺、动画
    /// </remarks>
    public enum Subtype : byte
    {
        /// <summary>
        /// 电影
        /// </summary>
        [Display(Name = "电影")]
        Movie = 5,

        /// <summary>
        /// 电视剧
        /// </summary>
        [Display(Name = "电视剧")]
        Tv,

        /// <summary>
        /// 记录片
        /// </summary>
        [Display(Name = "记录片")]
        Documentary,

        /// <summary>
        /// 综艺
        /// </summary>
        [Display(Name = "综艺")]
        TvShow,

        /// <summary>
        /// 动画
        /// </summary>
        [Display(Name = "动画")]
        Animation
    }
}
