namespace KdyWeb.CloudParse.Input
{
    /// <summary>
    /// 基础配置输入
    /// </summary>
    public class BaseConfigInput : IBaseConfigEntity
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="reqUserInfo">Url用户信息</param>
        /// <param name="parseCookie">解析Cookie</param>
        /// <param name="subAccountId">子账号Id</param>
        public BaseConfigInput(string reqUserInfo, string parseCookie, long subAccountId)
        {
            ReqUserInfo = reqUserInfo;
            ParseCookie = parseCookie;
            ChildUserId = subAccountId;
        }

        public string ReqUserInfo { get; set; }

        public string ParseCookie { get; set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        public long ChildUserId { get; set; }
    }
}
