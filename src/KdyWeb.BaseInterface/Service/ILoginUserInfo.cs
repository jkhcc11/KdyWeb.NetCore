using KdyWeb.BaseInterface.InterfaceFlag;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 登录信息 接口
    /// </summary>
    public interface ILoginUserInfo : IKdyTransient
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        bool IsLogin { set; get; }

        /// <summary>
        /// 浏览器Agent
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        string UserNick { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        string UserEmail { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        long? UserId { get; set; }

        /// <summary>
        /// 是否超管
        /// </summary>
        bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <returns></returns>
        long GetUserId();

        /// <summary>
        /// 登录Token
        /// </summary>
        string LoginToken { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        string RoleName { get; set; }

        /// <summary>
        /// 是否资源管理
        /// </summary>
        bool IsVodAdmin { get; set; }

        /// <summary>
        /// 是否普通用户（非资源管理和超管）
        /// </summary>
        bool IsNormal { get; set; }
    }
}
