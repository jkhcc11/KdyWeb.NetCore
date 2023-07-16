using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.CloudParse.Enum
{
    /// <summary>
    /// 服务器Cookie状态
    /// </summary>
    public enum ServerCookieStatus
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Display(Name = "初始化")]
        Init = 1,

        /// <summary>
        /// 正常
        /// </summary>
        [Display(Name = "正常")]
        Normal = 5
    }
}
