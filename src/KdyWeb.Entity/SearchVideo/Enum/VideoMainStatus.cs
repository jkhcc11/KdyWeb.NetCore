using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 影片状态
    /// </summary>
    public enum VideoMainStatus : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Display(Name = "正常")]
        Normal = 0,

        /// <summary>
        /// 禁用
        /// </summary>
        [Display(Name = "禁用")]
        Ban,

        /// <summary>
        /// 登录
        /// </summary>
        [Display(Name = "登录")]
        Login,

        /// <summary>
        /// 下架
        /// </summary>
        [Display(Name = "下架")]
        Down = 10
    }
}
