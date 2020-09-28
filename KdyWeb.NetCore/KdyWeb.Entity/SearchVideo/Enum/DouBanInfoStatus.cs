using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 豆瓣信息状态
    /// </summary>
    public enum DouBanInfoStatus : byte
    {
        /// <summary>
        /// SearchWait
        /// </summary>
        [Display(Name = "待搜索")]
        SearchWait = 0,

        /// <summary>
        /// 搜索中
        /// </summary>
        [Display(Name = "搜索中")]
        Searching,

        /// <summary>
        /// 搜索完成
        /// </summary>
        [Display(Name = "搜索完成")]
        SearchEnd,

        /// <summary>
        /// 搜索失败
        /// </summary>
        [Display(Name = "搜索失败")]
        SearchFail
    }
}
