using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 用户反馈类型
    /// </summary>
    public enum UserDemandType
    {
        /// <summary>
        /// 站点反馈
        /// </summary>
        [Display(Name = "站点反馈")]
        Feedback = 5,

        /// <summary>
        /// 用户录入
        /// </summary>
        [Display(Name = "用户录入")]
        Input = 10
    }
}
