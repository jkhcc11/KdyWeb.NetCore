namespace KdyWeb.CloudParse
{
    /// <summary>
    /// 基础信息
    /// </summary>
    public interface IBaseConfigEntity
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <remarks>
        ///  主账号用户名，子账号别名或子账号Id <br/>
        ///  仅用于日志记录
        /// </remarks>
        string ReqUserInfo { get; set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        long ChildUserId { get; }

        /// <summary>
        /// 解析Cookie 
        /// </summary>
        string ParseCookie { get; set; }
    }
}
