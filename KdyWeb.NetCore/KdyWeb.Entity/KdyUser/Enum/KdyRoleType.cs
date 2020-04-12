using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity
{
    /// <summary>
    /// 角色类型
    /// </summary>
    public enum KdyRoleType : byte
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        [Display(Name = "普通用户")]
        Normal = 1,

        /// <summary>
        /// 影片管理者
        /// </summary>
        [Display(Name = "影片管理者")]
        VideoAdmin = 5,

        /// <summary>
        /// 超管
        /// </summary>
        [Display(Name = "超管")]
        SupperAdmin = 10,

        /// <summary>
        /// 直播源管理
        /// </summary>
        [Display(Name = "直播源管理")]
        LiveAdmin = 15
    }
}
