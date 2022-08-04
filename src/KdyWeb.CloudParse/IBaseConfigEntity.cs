namespace KdyWeb.CloudParse
{
    /// <summary>
    /// 基础信息
    /// </summary>
    public interface IBaseConfigEntity
    {
        /// <summary>
        /// Url用户信息
        /// </summary>
        /// <remarks>
        ///  格式：nickname_子账号Id    eg:tytest_152
        /// </remarks>
        string ReqUserInfo { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        long UserId { get; set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        int ChildUserId { get; }

        /// <summary>
        /// 解析Cookie 
        /// </summary>
        string ParseCookie { get; set; }
    }
}
