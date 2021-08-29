using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.HttpCapture
{
    /// <summary>
    ///站点页面搜索配置 状态
    /// </summary>
    public enum SearchConfigStatus : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Display(Name = "正常")]
        Normal = 1,

        /// <summary>
        /// 禁用
        /// </summary>
        [Display(Name = "禁用")]
        Ban,
    }
}
