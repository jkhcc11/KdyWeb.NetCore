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
        /// <param name="subAccountId">子账号Id</param>
        public BaseConfigInput(long subAccountId)
        {
            ChildUserId = subAccountId;
            ReqUserInfo = subAccountId + "";
        }

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

        /// <summary>
        /// 代理信息
        /// </summary>
        /// <remarks>
        ///  格式：服务器:端口:用户名:密码
        /// </remarks>
        public string? WebProxyInfo { get; set; }

        /// <summary>
        /// 解析代理信息
        /// </summary>
        /// <returns></returns>
        public WebProxyInfoItem? GetProxyInfo()
        {
            if (string.IsNullOrEmpty(WebProxyInfo))
            {
                return default;
            }

            var tempArray = WebProxyInfo.Split(':');

            var result = new WebProxyInfoItem(tempArray[0], int.Parse(tempArray[1]));
            if (tempArray.Length >= 4)
            {
                result.UserName = tempArray[2];
                result.UserPwd = tempArray[3];
            }

            return result;
        }
    }
}
