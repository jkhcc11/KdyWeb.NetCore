namespace KdyWeb.CloudParse.Input
{
    /// <summary>
    /// 代理信息
    /// </summary>
    public class WebProxyInfoItem
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="serverUrl">代理服务器地址</param>
        /// <param name="port">端口</param>
        public WebProxyInfoItem(string serverUrl, int port)
        {
            ServerUrl = serverUrl;
            Port = port;
        }

        /// <summary>
        /// 代理服务器地址
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户名（一般隧道代理）
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 密码（一般隧道代理）
        /// </summary>
        public string? UserPwd { get; set; }
    }
}
