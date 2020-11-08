using KdyWeb.BaseInterface.InterfaceFlag;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 登录信息 接口
    /// </summary>
    public interface ILoginUserInfo : IKdyScoped
    {
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
        /// 用户Id
        /// </summary>
        int? UserId { get; set; }
    }
}
