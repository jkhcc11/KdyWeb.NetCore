using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 用户订阅类型
    /// </summary>
    public enum UserSubscribeType : byte
    {
        /// <summary>
        /// 影片
        /// </summary>
        [Display(Name = "影片")]
        Vod = 0,

        /// <summary>
        /// 小说
        /// </summary>
        [Display(Name = "小说")]
        Book,

        /// <summary>
        /// 链接
        /// </summary>
        [Display(Name = "链接")]
        Link
    }
}
