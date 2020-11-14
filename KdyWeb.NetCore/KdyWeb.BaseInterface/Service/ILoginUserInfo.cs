using KdyWeb.BaseInterface.InterfaceFlag;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 登录信息 接口
    /// </summary>
    public interface ILoginUserInfo : IKdySingleton
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
        long? UserId { get; set; }
    }
}
